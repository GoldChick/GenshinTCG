namespace TCGBase
{
    public record DamageRecord
    {
        public DamageElement Element { get; }
        public int Damage { get; }
        public int TargetIndexOffset { get; }
        public DamageTargetArea TargetArea { get; }
        public DamageTargetTeam TargetTeam { get; }
        public DamageRecord? SubDamage { get; internal set; }
        public DamageRecord(DamageElement element, int damage, int targetIndexOffset = 0, DamageTargetArea targetArea = DamageTargetArea.TargetOnly, DamageTargetTeam targetTeam = DamageTargetTeam.Enemy, DamageRecord? subDamage = null)
        {
            Element = element;
            Damage = damage;
            TargetIndexOffset = targetIndexOffset;
            TargetArea = targetArea;
            TargetTeam = targetTeam;
            SubDamage = subDamage;
        }
    }
}
