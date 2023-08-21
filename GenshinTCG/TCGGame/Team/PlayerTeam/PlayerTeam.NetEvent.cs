using TCGCard;

namespace TCGGame
{
    public partial class PlayerTeam
    {
        protected override bool IsTargetValid(TargetEnum e, int arg) => e switch
        {
            TargetEnum.Card_Me => arg >= 0 && arg < CardsInHand.Count,
            TargetEnum.Character_Enemy => arg >= 0, //TODO: no enemy check now
            TargetEnum.Character_Me => arg >= 0 && arg < Characters.Length,
            //TargetEnum.Dice => arg >= 0 && arg < Characters.Length,
            TargetEnum.MultiCard => arg >= 0,
            //TargetEnum.MultiDice => arg >= 0 && arg < Characters.Length,
            TargetEnum.Summon_Enemy => arg >= 0 && arg < Summons.Count,
            TargetEnum.Support_Enemy => arg >= 0 && arg < Supports.Count,
            _ => false
        };
    }
}
