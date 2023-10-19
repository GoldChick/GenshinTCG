using System.Text;
using TCGBase;
using TCGGame;

namespace TCGClient
{
    public class MessageSet
    {
        private List<string> _messages;
        private readonly int _length;
        private readonly string _default;
        public MessageSet(int defaultCharNum)
        {
            _messages = new();
            _length = defaultCharNum;
            _default = new StringBuilder(defaultCharNum).Insert(0, " ", defaultCharNum).ToString();
        }
        /// <summary>
        /// 过长的字符串会被截断
        /// </summary>
        /// <param name="messages"></param>
        public void Update(List<string> messages)
        {
            _messages = messages.Select(p =>
            {
                int utf8_count = Encoding.UTF8.GetByteCount(p);
                utf8_count -= (utf8_count - p.Length) / 2;
                //中文占2字符
                while (utf8_count > _length)
                {
                    p = p[..(p.Length - 1)];
                    utf8_count = Encoding.UTF8.GetByteCount(p);
                    utf8_count -= (utf8_count - p.Length) / 2;
                }
                return p.PadRight(_length - utf8_count + p.Length, ' ');
            }).ToList();
        }
        /// <returns>以 | 作为结尾的字符数量为 defaultCharNum+1 的字符串</returns>
        public string Get(int index) => (index >= 0 && _messages.Count > index ? _messages.ElementAt(index) : _default) + "|";
    }
    internal class TeamRender
    {
        private int currPrintHeight;
        private MessageSet[] supports = new MessageSet[] { new MessageSet(12), new MessageSet(12), new MessageSet(12), new MessageSet(12) };
        //length:17
        private List<MessageSet> characters;
        private readonly MessageSet[] summons = new MessageSet[] { new MessageSet(12), new MessageSet(12), new MessageSet(12), new MessageSet(12) };
        private MessageSet teamEffects = new(17);
        private int currCharacter;
        public TeamRender()
        {
            characters = new() { new MessageSet(17), new MessageSet(17), new MessageSet(17) };
        }
        /// <summary>
        /// leftbar=>support=>character=>summon=>rightbar
        /// </summary>
        private void Render(MessageSet left, List<string> right)
        {
            //TODO: left & right bar
            string message = "";
            for (currPrintHeight = 0; currPrintHeight < 12; currPrintHeight++)
            {
                message += left.Get(currPrintHeight);
                message += supports[0].Get(currPrintHeight);
                message += supports[1].Get(currPrintHeight);
                for (int c = 0; c < 3; c++)
                {
                    message += characters[c].Get(currPrintHeight);
                }
                message += summons[0].Get(currPrintHeight);
                message += summons[1].Get(currPrintHeight);
                message += right.ElementAtOrDefault(currPrintHeight);
                Console.WriteLine(message);
                message = "";
            }
            #region supports & summons seperate bar
            {
                message += "      |";
                message += "===== ===== = ===== =====|";
                for (int c = 0; c < 3; c++)
                {
                    message += characters[c].Get(currPrintHeight);
                }
                message += "===== ===== = ===== =====|";
                message += right.ElementAtOrDefault(currPrintHeight);
                Console.WriteLine(message);
                message = "";
            }
            #endregion
            for (currPrintHeight = 13; currPrintHeight < 15; currPrintHeight++)
            {
                message += "      |";
                message += supports[2].Get(currPrintHeight - 13);
                message += supports[3].Get(currPrintHeight - 13);
                for (int c = 0; c < 3; c++)
                {
                    message += characters[c].Get(currPrintHeight);
                }
                message += summons[2].Get(currPrintHeight - 13);
                message += summons[3].Get(currPrintHeight - 13);
                message += right.ElementAtOrDefault(currPrintHeight);
                Console.WriteLine(message);
                message = "";
            }
            #region character & teameffect seperate
            {
                message += "      |";
                message += supports[2].Get(2);
                message += supports[3].Get(2);
                message += new StringBuilder(52).Insert(0, "=", 34).Insert(17 * currCharacter, "↑", 17).Insert(34, '|').Insert(17, '|').Append("|").ToString();
                message += summons[2].Get(2);
                message += summons[3].Get(2);
                message += right.ElementAtOrDefault(currPrintHeight);
                Console.WriteLine(message);
                message = "";
            }
            #endregion
            for (currPrintHeight = 16; currPrintHeight < 25; currPrintHeight++)
            {
                message += "      |";
                message += supports[2].Get(currPrintHeight - 13);
                message += supports[3].Get(currPrintHeight - 13);

                message += characters[0].Get(-1);
                message += teamEffects.Get(currPrintHeight - 16);
                message += characters[2].Get(-1);

                message += summons[2].Get(currPrintHeight - 13);
                message += summons[3].Get(currPrintHeight - 13);
                message += right.ElementAtOrDefault(currPrintHeight);
                Console.Write(message);
                Console.WriteLine("");
                message = "";
            }
        }
        public void Render(PlayerTeam pt, List<string> left, List<string> right)
        {
            List<string> strings = new();
            Array.ForEach(summons, s => s.Update(strings));
            Array.ForEach(supports, s => s.Update(strings));

            for (int i = 0; i < pt.Summons.Count; i++)
            {
                AbstractPersistent p = pt.Summons[i];
                strings = new()
                {
                        p.NameID,
                        $"可用:{p.CardBase.Info(p)[0]}"
                };
                summons[i].Update(strings);
            }
            for (int i = 0; i < pt.Supports.Count; i++)
            {
                AbstractPersistent p = pt.Supports[i];
                strings = new()
                {
                        p.NameID,
                        $"可用次数:{p.CardBase.Info(p)[0]}"
                };
                supports[i].Update(strings);
            }
            for (int i = 0; i < 3; i++)
            {
                var p = pt.Characters[i];
                strings = new()
                {
                    p.Card.NameID,
                    $"HP:{p.HP}/{p.Card.MaxHP} MP:{p.MP}/{p.Card.MaxMP}",
                    $"Element:{p.Element}"
                };
                if (p.Weapon != null)
                {
                    strings.Add($"Weapon:{p.Weapon.NameID} 可用:{p.Weapon.Card.Info(p.Weapon)[0]}");
                }
                //TODO: equipment
                if (p.Effects.Count > 0)
                {
                    strings.Add("Effects:");
                    //TODO: when 溢出
                    for (int j = 0; j < p.Effects.Count; j++)
                    {
                        strings.Add($"{p.Effects[j].NameID} 可用次数:{p.Effects[j].Card.Info(p.Effects[j])[0]}");
                    }
                }
                characters[i].Update(strings);
            }
            currCharacter = pt.CurrCharacter;
            strings.Clear();
            if (pt.Effects.Count > 0)
            {
                strings.Add("TeamEffects:");
                //TODO: when 溢出
                for (int j = 0; j < pt.Effects.Count; j++)
                {
                    strings.Add($"{pt.Effects[j].NameID} 可用次数:{pt.Effects[j].Card.Info(pt.Effects[j])[0]}");
                }
            }
            teamEffects.Update(strings);

            left = new()
            {
                "骰子数:",
                $"{pt.GetDices().Sum()}",
                "卡牌数:",
                $"{pt.CardsInHand.Count}"
            };
            if (pt.Game.CurrTeam == pt.TeamIndex)
            {
                left.Add("");
                left.Add("");
                left.Add("");
                left.Add("行动中");
            }

            MessageSet leftm = new(6);
            leftm.Update(left);
            Render(leftm, right);
        }
        public void RenderMe(PlayerTeam pt)
        {
            List<string> right = pt.CardsInHand.Select(c => c.Card.NameID).ToList();
            right.Insert(0, "Cards:");
            Render(pt, null, right);
        }
        public void RenderEnemy(PlayerTeam me, PlayerTeam enemy)
        {
            List<string> right = me.GetDices().Select((p, index) => $"{(ElementCategory)index}:{p}").ToList();
            right.Insert(0, "Dices:");
            Render(enemy, null, right);
        }
    }
}
