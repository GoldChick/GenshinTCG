namespace TCGBase
{
    public class Character
    {
        public AbstractCardCharacter Card { get; }
        /// <summary>
        /// 注册后的、带有namespace的nameid
        /// </summary>
        public string NameID { get; }
        /// <summary>
        /// 表示自己在team中的位置
        /// </summary>
        public int Index { get; init; }
        private int _hp;
        private int _mp;
        public PersistentSet<AbstractCardPersistentEffect> Weapon { get; init; }
        public PersistentSet<AbstractCardPersistentEffect> Artifact { get; init; }
        public PersistentSet<CardPersistentTalent> Talent { get; init; }

        public PersistentSet<AbstractCardPersistentEffect> Effects { get; init; }

        public int HP
        {
            get { return _hp; }
            set { _hp = int.Clamp(value, 0, Card.MaxHP); }
        }
        public int MP { get => _mp; set => _mp = int.Clamp(value, 0, Card.MaxMP); }

        public bool Alive;
        /// <summary>
        /// 濒死状态，生命值降为0，又不被“免于被击倒”治疗会使其为true<br/>
        /// 真死了之后又为false
        /// </summary>
        public bool Predie;
        /// <summary>
        /// 为active时可以使用技能
        /// </summary>
        public bool Active;
        public int Element;
        public Character(RegistryObject<AbstractCardCharacter> character, int index)
        {
            NameID = character.NameID;
            Card = character.Value;
            Index = index;

            Weapon = new(index, 1);
            Artifact = new(index, 1);
            Talent = new(index, 1);
            Effects = new(index);

            HP = Card.MaxHP;
            Alive = true;
            Active = true;
        }
        public void EffectTrigger(PlayerTeam me, AbstractSender sender, AbstractVariable? variable)
        {
            if (Alive)
            {
                Weapon?.EffectTrigger(me, sender, variable);
                Artifact?.EffectTrigger(me, sender, variable);
                Talent?.EffectTrigger(me, sender, variable);
                Effects.EffectTrigger(me, sender, variable);
            }
        }
    }
}
