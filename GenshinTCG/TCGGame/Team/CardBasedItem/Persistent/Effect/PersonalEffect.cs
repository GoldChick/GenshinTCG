using TCGCard;
using TCGRule;

namespace TCGGame
{
    public class PersonalEffect : AbstractPersistent<IEffect>
    {
        /// <summary>
        /// 在team中的characters对应的id
        /// </summary>
        public int Owner { get; }
        public PersonalEffect(int owner, string nameid) : base(nameid, Registry.Instance.Effects.GetValueOrDefault(nameid))
        {
            Owner = owner;
        }
        public PersonalEffect(int owner, IEffect ef) : base(ef.NameID, ef)
        {
            Owner = owner;
        }
    }
}
