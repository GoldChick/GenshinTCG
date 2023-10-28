﻿using TCGBase;

namespace TCGBase
{
    public class Effect : Persistent<AbstractCardPersistentEffect>
    {
        public Effect(string nameid) : base(nameid, Registry.Instance.Effects.GetValueOrDefault(nameid))
        {
        }
        public Effect(AbstractCardPersistentEffect ef, AbstractPersistent? bind=null) : base(ef.NameID, ef,bind)
        {
        }
    }
}