﻿namespace TCGBase
{
    /// <summary>
    /// 受到伤害后产生的sender，可能并不一定用于直接触发<br/>
    /// </summary>
    public class HurtSender : AbstractSender
    {
        public override string SenderName => Tags.SenderTags.AFTER_HURT;
        public int Element { get; init; }
        public int Damage { get; internal set; }//set只是为了合并
        public int TargetIndex { get; init; }
        public string? Reaction { get; init; }
        internal HurtSender(int element, int damage, int targetIndex, string? reaction)
        {
            Element = element;
            Damage = damage;
            TargetIndex = targetIndex;
            Reaction = reaction;
        }
        internal HurtSender(DamageVariable dv, string? reaction)
        {
            Element = dv.Element;
            Damage = dv.Damage;
            TargetIndex = dv.TargetIndex;
            Reaction = reaction;
        }
        //public HurtSender(int element, int damage, int reaction)
        //{
        //    Element = element;
        //    Damage = damage;
        //    Reaction = reaction;
        //}
    }
}
