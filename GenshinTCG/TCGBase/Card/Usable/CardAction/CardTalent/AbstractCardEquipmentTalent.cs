namespace TCGBase
{
    public abstract class AbstractCardEquipmentTalent : AbstractCardEquipment, ICardTalent
    {
        public override int MaxUseTimes => 0;
        /// <summary>
        /// 默认的TriggerDic为空，但也不排除特殊情况，如[迪西雅]天赋
        /// </summary>
        public override PersistentTriggerDictionary TriggerDic => new();
        public override string NameID => $"talent_{CharacterNameID}";
        /// <summary>
        /// 不指定namespace，则和本身（这张卡）的一样
        /// </summary>
        public virtual string? CharacterNamespace { get => null; }
        /// <summary>
        /// 所属的角色的nameid
        /// </summary>
        public abstract string CharacterNameID { get; }
        public override bool CanBeArmed(List<AbstractCardCharacter> chars) => chars.Any(c => $"{CharacterNamespace ?? Namespace}:{CharacterNameID}".Equals($"{c.Namespace}:{c.NameID}"));
        /// <summary>
        /// 默认实现为需要是本人的天赋，并且为被动技能/该角色在前台
        /// </summary>
        public override bool CanBeUsed(PlayerTeam me, int[] targetArgs)
        {
            var c = me.Characters[targetArgs[0]];
            var card = c.Card;
            return c.Alive && $"{CharacterNamespace ?? Namespace}:{CharacterNameID}".Equals($"{card.Namespace}:{card.NameID}");
        }
    }
}
