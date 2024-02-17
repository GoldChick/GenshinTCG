namespace TCGBase
{
    public partial class Game
    {
        /// <summary>
        /// 输入的dv满足targetRelative=false<br/>
        /// 在其中进行effect的调用
        /// </summary>
        private List<HealVariable> InnerHealCompute(HurtSourceSender sourceSender, HealRecord start)
        {
            List<HealVariable> list = new();

            HealRecord? curr = start;
            do
            {
                var currteamid = 1 - sourceSender.TeamID ^ (int)curr.Team;
                var currteam = Teams[currteamid];
                if (curr.TargetArea == TargetArea.TargetOnly)
                {
                    list.Add(new HealVariable(currteamid, curr.Amount, DamageSource.Direct, ((currteam.CurrCharacter + curr.TargetIndexOffset) % currteam.Characters.Length + currteam.Characters.Length) % currteam.Characters.Length));
                }
                else
                {
                    if (currteam.CurrCharacter == -1)
                    {
                        throw new ArgumentOutOfRangeException(nameof(start), "InnerHurtCompute:怎么有队伍的CurrCharacter是-1啊？");
                    }
                    list.AddRange(Enumerable.Range(0, currteam.Characters.Length)
                        .OrderBy(i => ((i - currteam.CurrCharacter - curr.TargetIndexOffset) % currteam.Characters.Length + currteam.Characters.Length) % currteam.Characters.Length)
                        .Skip(1).Where(i => currteam.Characters[i].Alive)
                        .Select(i => new HealVariable(currteamid, curr.Amount, DamageSource.Direct, i)));
                }
                curr = curr.SubHeal;
            } while (curr != null);
            return list;
        }
        internal void InnerHeal(HealRecord? heal, HurtSourceSender sourceSender)
        {
            if (heal != null)
            {
                sourceSender.ModifierName = SenderTag.AfterHeal;
                foreach (var hv in InnerHealCompute(sourceSender, heal))
                {
                    var cha = Teams[hv.TargetTeam].Characters[hv.TargetIndex];
                    int amount = int.Min(hv.Amount, cha.CharacterCard.MaxHP - cha.HP);
                    cha.HP += amount;
                    BroadCast(ClientUpdateCreate.CharacterUpdate.HealUpdate(hv.TargetTeam, hv.TargetIndex, hv.Amount));
                    EffectTrigger(sourceSender, hv);
                }
            }
        }
    }
}
