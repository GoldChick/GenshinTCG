using System.Text;
using TCGBase;

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
        private readonly MessageSet[] supports = new MessageSet[] { new MessageSet(12), new MessageSet(12), new MessageSet(12), new MessageSet(12) };
        //length:17
        private readonly List<MessageSet> characters;
        private readonly MessageSet[] summons = new MessageSet[] { new MessageSet(12), new MessageSet(12), new MessageSet(12), new MessageSet(12) };
        private readonly MessageSet teamEffects = new(17);
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
        public void Render(ReadonlyRegion region, List<string> left, List<string> right)
        {
            List<string> strings = new();
            Array.ForEach(summons, s => s.Update(strings));
            Array.ForEach(supports, s => s.Update(strings));

            for (int i = 0; i < region.Summons.Count; i++)
            {
                var p = region.Summons[i];
                strings = new()
                {
                        p.Name,
                        $"可用:{p.Infos[0]}"
                };
                summons[i].Update(strings);
            }
            for (int i = 0; i < region.Supports.Count; i++)
            {
                var p = region.Supports[i];
                strings = new()
                {
                        p.Name,
                        $"可用:{p.Infos[0]}"
                };
                supports[i].Update(strings);
            }
            for (int i = 0; i < 3; i++)
            {
                var p = region.Characters[i];
                strings = new()
                {
                    p.Name,
                    $"HP:{p.HP}/{p.MaxHP} MP:{p.MP}/{p.MaxMP}",
                    $"Element:{p.Element}"
                };
                if (p.Weapon != null)
                {
                    strings.Add($"Weapon:{p.Weapon.Name} 可用:{p.Weapon.Infos[0]}");
                }
                if (p.Artifact != null)
                {
                    strings.Add($"Artifact:{p.Artifact.Name} 可用:{p.Artifact.Infos[0]}");
                }
                if (p.Talent != null)
                {
                    strings.Add($"Talent:{p.Talent.Name} 可用:{p.Talent.Infos[0]}");
                }
                if (p.Effects.Count > 0)
                {
                    strings.Add("Effects:");
                    for (int j = 0; j < p.Effects.Count; j++)
                    {
                        strings.Add($"{p.Effects[j].Name} 可用:{p.Effects[j].Infos[0]}");
                    }
                }
                characters[i].Update(strings);
            }
            currCharacter = region.CurrCharacter;
            strings.Clear();
            if (region.Effects.Count > 0)
            {
                strings.Add("TeamEffects:");
                for (int j = 0; j < region.Effects.Count; j++)
                {
                    strings.Add($"{region.Effects[j].Name} 可用:{region.Effects[j].Infos[0]}");
                }
            }
            teamEffects.Update(strings);

            MessageSet leftm = new(6);
            leftm.Update(left);
            Render(leftm, right);
        }
        public void RenderMe(ReadonlyGame gm)
        {
            List<string> left = new()
            {
                "骰子数:",
                $"{gm.Dices.Count}",
                "卡牌数:",
                $"{gm.Cards.Count}",
                "剩余卡牌数:",
                $"{gm.LeftCardsNum}"
            };
            if (gm.CurrTeam == gm.MeID)
            {
                left.Add("");
                left.Add("");
                left.Add("");
                left.Add("行动中");
            }
            List<string> right = gm.Cards.ToList();
            right.Insert(0, "Cards:");
            Render(gm.Me, left, right);
        }
        public void RenderEnemy(ReadonlyGame gm)
        {
            List<string> left = new()
            {
                "骰子数:",
                $"{gm.EnemyDiceNum}",
                "卡牌数:",
                $"{gm.EnemyCardNum}",
                "剩余卡牌数:",
                $"{gm.EnemyLeftCardsNum}"
            };
            if (gm.CurrTeam != gm.MeID)
            {
                left.Add("");
                left.Add("");
                left.Add("");
                left.Add("行动中");
            }
            List<string> right = gm.Dices.Select(p => $"{(ElementCategory)p}").ToList();
            right.Insert(0, "Dices:");
            Render(gm.Enemy, left, right);
        }
    }
}
