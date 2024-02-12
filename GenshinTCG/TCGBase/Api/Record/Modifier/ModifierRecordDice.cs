namespace TCGBase
{
    /*
        目前观察到的现象：
        作为技能的 x无色+x某元素：
        首先减费某元素，依次寻找对应元素、任意元素
        其次减费无色，依次寻找无色元素、有色元素、任意元素

        例
        铃铛+魔女+灼灼+烟熏鸡：普攻不消耗灼灼
        冰圣遗物+铃铛+火花+薯条：普攻不消耗薯条
     */
    public enum ModifierDiceType
    {
        Card,
        Skill,
        Switch
    }
    //TODO: card skill switch
    public record class ModifierRecordDice : ModifierRecordBase
    {
        protected ModifierRecordDice(ModifierRecordBase original) : base(original)
        {
        }

        public override EventPersistentHandler? Get()
        {
            EventPersistentHandler handler = (me, p, s, v) =>
            {
                if (s is AbstractUseDiceSender uds)
                {

                }
            };
            return handler;
        }
    }
}
