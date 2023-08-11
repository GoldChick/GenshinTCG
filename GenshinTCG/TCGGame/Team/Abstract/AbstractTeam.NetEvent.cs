using TCGBase;

namespace TCGGame
{
    public abstract partial class AbstractTeam
    {
        public bool CheckEventRequire(NetEvent evtwithaction)
        {
            switch (evtwithaction.Action.Type)
            {
                case ActionType.Trival:
                    break;
                case ActionType.ReRollDice:
                    break;
                case ActionType.ReRollCard:
                    break;
                case ActionType.Switch:
                    break;
                case ActionType.UseSKill:
                    break;
                case ActionType.UseCard:
                    break;
                case ActionType.Blend:
                    break;
                case ActionType.Pass:
                    break;
            }
            return false;
        }
    }
}
