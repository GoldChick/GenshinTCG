using System.Text.RegularExpressions;
using TCGAI;
using TCGBase;
using TCGUtil;

namespace GenshinTCG.TCGAI
{
    /// <summary>
    /// 用于测试的手动输入行动的ai
    /// </summary>
    internal class ConsoleAI : AbstractAI
    {
        public override NetEvent Act(ActionType demand, string help_txt = "")
        {
            bool canReAct = demand == ActionType.Trival;
            if (canReAct)
            {
                Logger.Print($"{Name}行动阶段!输入0-7，不输入将视为pass。");

                string? input = Console.ReadLine() ?? "0";
                input=Regex.Replace(input, @"[^\w]", "", RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }

            throw new NotImplementedException();
        }

        public override NetEvent Blend()
        {
            throw new NotImplementedException();
        }

        public override NetEvent Pass()
        {
            throw new NotImplementedException();
        }

        public override NetEvent ReplaceAssist()
        {
            throw new NotImplementedException();
        }

        public override NetEvent ReRollCard()
        {
            throw new NotImplementedException();
        }

        public override NetEvent ReRollDice()
        {
            throw new NotImplementedException();
        }

        public override NetEvent Switch()
        {
            throw new NotImplementedException();
        }

        public override NetEvent UseCard()
        {
            throw new NotImplementedException();
        }

        public override NetEvent UseSkill()
        {
            throw new NotImplementedException();
        }
    }
}
