namespace TCGBase
{
    public static class TagsExtendMethod
    {
        internal static TargetEnumForNetEvent ToNetEvent(this TargetEnum e) => e switch
        {
            //TargetEnum.Card_Enemy => TargetEnumForNetEvent.Card_Enemy,
            //TargetEnum.Card_Me => TargetEnumForNetEvent.Card_Me,
            TargetEnum.Character_Enemy => TargetEnumForNetEvent.Character_Enemy,
            TargetEnum.Character_Me => TargetEnumForNetEvent.Character_Me,
            TargetEnum.Summon_Enemy => TargetEnumForNetEvent.Summon_Enemy,
            TargetEnum.Summon_Me => TargetEnumForNetEvent.Summon_Me,
            TargetEnum.Support_Enemy => TargetEnumForNetEvent.Support_Enemy,
            TargetEnum.Support_Me => TargetEnumForNetEvent.Support_Me,
            _ => throw new Exception("Tags.ToInternal():传入了未知的TargetEnum!")
        };
    }
}
