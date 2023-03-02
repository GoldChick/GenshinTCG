using TCGInfo;

namespace TCGCard
{
    public interface ICardWeapon : ICardAssist
    {
        public int BaseDamage { get; }

        int GetAdditionalDamage(IInfo[] igame);//一定条件下额外攻击
        void GetAdditonalEffect(IInfo[] igame);//一定条件下触发的额外效果
    }
}