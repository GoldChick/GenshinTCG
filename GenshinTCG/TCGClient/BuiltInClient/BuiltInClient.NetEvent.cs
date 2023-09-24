using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.RegularExpressions;
using TCGBase;
using TCGUtil;

namespace TCGClient
{
    internal partial class BuiltInClient
    {
        public NetEvent Act(ActionType demand)
        {
            bool canReAct = demand == ActionType.Trival;
            if (canReAct)
            {
                Logger.Print($"行动阶段!输入0-5，不输入将视为pass。", ConsoleColor.DarkCyan);
                Logger.Print($"0:空过");
                Logger.Print($"1:使用技能");
                Logger.Print($"2:切换角色");
                Logger.Print($"3:使用卡牌");
                Logger.Print($"4:调和卡牌");

                Logger.Print($"5:查看场上");

                NetAction? ac = null;
                while (ac == null)
                {
                    if (!int.TryParse(Regex.Replace(Console.ReadLine() ?? "0", @"[^\w]", "", RegexOptions.None, TimeSpan.FromSeconds(1.5)), out int input_num))
                    {
                        input_num = 0;
                    }
                    ac = int.Clamp(input_num, 0, 5) switch
                    {
                        1 => UseSkill(),
                        2 => Switch(),
                        3 => UseCard(),
                        5 => Print(),
                        _ => new NetAction(ActionType.Pass)
                    };
                }
                var req = MePt.GetEventDiceRequirement(ac);
                //TODO:没有target selector

                Logger.Warning($"返回的actiontype={ac.Type}");
                return new(ac, req.Cost.Costs.Sum() > 0 ? SelectDices(req.Cost.Costs) : null);
            }
            else
            {
                return demand switch
                {
                    ActionType.Switch => new(Switch()),
                    ActionType.SwitchForced => new(Switch(true)),
                    _ => throw new NotImplementedException("指定demand的还没做呃呃呃")
                };
            }
        }
        /// <param name="forced">是否是要求的强制切人</param>
        public NetAction Switch(bool forced = false)
        {
            Logger.Print("BuiltInClient.Switch():默认切换到下一个角色!");
            return new NetAction(forced ? ActionType.SwitchForced : ActionType.Switch, (Me.CurrCharacter + 1) % Me.Characters.Length);
        }
        public NetAction UseCard()
        {
            var cards = MePt.CardsInHand;
            Logger.Print($"使用技能!输入0-{cards.Count - 1}，不输入将视为0。", ConsoleColor.DarkCyan);
            cards.ForEach(c => c.Print());
            if (!int.TryParse(Regex.Replace(Console.ReadLine() ?? "0", @"[^\w]", "", RegexOptions.None, TimeSpan.FromSeconds(1.5)), out int input_num))
            {
                input_num = 0;
            }
            //TODO:默认使用第一张卡
            return new NetAction(ActionType.UseCard, int.Clamp(input_num, 0, cards.Count - 1));
        }
        public NetAction UseSkill()
        {
            //TODO:默认使用第一个技能
            var cha = Me.Characters[Me.CurrCharacter].Card;
            var skills = cha.Skills;
            Logger.Print($"使用技能!输入0-{skills.Length - 1}，不输入将视为0。", ConsoleColor.DarkCyan);
            for (int i = 0; i < skills.Length; i++)
            {
                Logger.Print($"{i}: {cha.NameID}.{skills[i].NameID} {JsonSerializer.Serialize(skills[i].Tags)}");
            }
            if (!int.TryParse(Regex.Replace(Console.ReadLine() ?? "0", @"[^\w]", "", RegexOptions.None, TimeSpan.FromSeconds(1.5)), out int input_num))
            {
                input_num = 0;
            }
            return new NetAction(ActionType.UseSKill, int.Clamp(input_num, 0, skills.Length - 1));
        }
        public NetEvent Blend()
        {
            throw new NotImplementedException("NO BLEND NOW");
        }

        public NetEvent ReplaceAssist()
        {
            throw new NotImplementedException("NO REPLACE NOW");
        }

        public NetEvent ReRollCard()
        {
            throw new NotImplementedException("NO REROLL NOW");
        }

        public NetEvent ReRollDice()
        {
            throw new NotImplementedException("NO REROLL NOW");
        }

        public int[] SelectDices([NotNull] int[] req)
        {
            Logger.Print("----骰子种类----：万能冰水火雷岩草风");
            Logger.Print($"现在拥有的骰子数： {JsonSerializer.Serialize(MePt.GetDices())}");
            Logger.Print($"需要使用的骰子数： {JsonSerializer.Serialize(req)}");

            int[] ints = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            Logger.Print($"选择使用骰子!输入8个数字（空格分割），代表万能、冰水火雷岩草风的骰子。", ConsoleColor.DarkCyan);
            for (int i = 0; i < 8; i++)
            {
                if (!int.TryParse(Regex.Replace(Console.ReadLine() ?? "0", @"[^\w]", "", RegexOptions.None, TimeSpan.FromSeconds(1.5)), out int input_num))
                {
                    input_num = 0;
                }
                ints[i] = input_num;
            }
            return ints;
        }
        public NetAction? Print()
        {
            Logger.Print("Team Me Info:", ConsoleColor.DarkCyan);
            Me.Print();
            Logger.Print("Team Enemy Info:", ConsoleColor.DarkRed);
            Enemy.Print();
            return null;
        }
    }
}
