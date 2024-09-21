namespace GenshinTCGCardMaker
{
    public enum GameActionType
    {
        GainDice,
        LoseDice,
        ConvertDice,
        GainCard,
        LoseCard,
        DrawCard,
        DiscardCard,
        GainEffect,
        LoseEffect,
        DoDamage,
        DoHeal
    }
    public enum DiceType
    {
        Paimon,
        Random, //获得n种随机/随机失去
        Normal, //获得n个随机/按照顺序失去
        //TODO: element
    }
    public enum GameActionTeam
    {
        Me,
        Enemy,
        Both
    }
    public class GameAction
    {
    }
}
