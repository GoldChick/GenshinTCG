using TCGBase;
using TCGCard;
using TCGGame;
using TCGUtil;

namespace Genshin3_3
{
    internal class LeaveItToMeEffect : IEffect
    {
        public bool Stackable => false;

        public bool Update => false;

        public int MaxUseTimes => 1;

        public bool Visible => true;

        public string NameID => "leaveittome_effect";

        public string[] Tags => new string[] { };

        public bool EffectTrigger(AbstractTeam me, AbstractTeam enemy, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
        {
            if (sender is SwitchSender)
            {
                Logger.Error("switchsender 触发了 交给我吧！");
                return true;
            }
            return false;
        }
    }
}
