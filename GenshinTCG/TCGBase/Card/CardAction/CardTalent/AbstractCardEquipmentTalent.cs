namespace TCGBase
{
    public abstract class AbstractCardEquipmentTalent : AbstractCardEquipment, ICardTalent
    {
        protected AbstractCardEquipmentTalent()
        {
            Variant = -3;
        }
        public override int MaxUseTimes => 0;
        /// <summary>
        /// 默认的TriggerDic为空，但也不排除特殊情况，如[迪西雅]天赋
        /// </summary>
        public override PersistentTriggerList TriggerList => new();
        public override string NameID => $"talent_{CharacterNameID}";
        /// <summary>
        /// 不指定namespace，则和本身（这张卡）的一样
        /// </summary>
        public virtual string CharacterNamespace { get => Namespace; }
        /// <summary>
        /// 所属的角色的nameid<br/>
        /// </summary>
        public abstract string CharacterNameID { get; }
        public override bool CanBeArmed(List<AbstractCardCharacter> chars) => chars.Any(((ICardTalent)this).IsFor);
        /// <summary>
        /// 默认实现为需要是本人的天赋，并且为被动技能/该角色在前台
        /// </summary>
        public override bool CanBeUsed(PlayerTeam me, int[] targetArgs)
        {
            var c = me.Characters[targetArgs[0]];
            return c.Alive && ((ICardTalent)this).IsFor(c.Card);
        }
    }
}
