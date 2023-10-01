using TCGCard;
using TCGRule;

namespace TCGGame
{
    public class PersonalEffect : AbstractPersistent<AbstractCardEffect>
    {
        /// <summary>
        /// 在team中的characters对应的id
        /// </summary>
        public int Owner { get; }
        public PersonalEffect(int owner, string nameid) : base(nameid, Registry.Instance.Effects.GetValueOrDefault(nameid))
        {
            Owner = owner;
        }
        public PersonalEffect(int owner, AbstractCardEffect ef) : base(ef.NameID, ef)
        {
            Owner = owner;
        }
    }
}
