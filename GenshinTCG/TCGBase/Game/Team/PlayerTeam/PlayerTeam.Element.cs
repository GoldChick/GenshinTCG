﻿namespace TCGBase
{
    public partial class PlayerTeam
    {
        public ReactionTags GetReaction(int currElement, int elementToApply, out int nextElement)
        {
            ReactionTags reactiontag = ReactionTags.None;
            //角色身上附着的元素(只允许附着 无0 冰1 水2 火3 雷4 草6 <b>冰+草5</b>
            nextElement = currElement;

            if (elementToApply > 0)
            {
                nextElement = 0;

                int reactionType = elementToApply * 10 + currElement;
                switch (reactionType)
                {
                    case 12 or 21 or 25://冻结
                        reactiontag = ReactionTags.Frozen;
                        break;

                    case 13 or 31 or 35://融化
                        reactiontag = ReactionTags.Melt;
                        break;

                    case 14 or 41 or 45://超导
                        reactiontag = ReactionTags.SuperConduct;
                        break;

                    case 23 or 32://蒸发
                        reactiontag = ReactionTags.Vaporize;
                        break;

                    case 24 or 42://感电
                        reactiontag = ReactionTags.ElectroCharged;
                        break;

                    case 34 or 43://超载
                        reactiontag = ReactionTags.Overloaded;
                        break;

                    case 51 or 52 or 53 or 54 or 55://结晶
                        reactiontag = ReactionTags.Crystallize;
                        break;

                    //NOTE:冰草共存优先反应冰

                    case 62 or 26://绽放
                        reactiontag = ReactionTags.Bloom;
                        break;

                    case 63 or 36://燃烧
                        reactiontag = ReactionTags.Burning;
                        break;

                    case 64 or 46://激化
                        reactiontag = ReactionTags.Catalyze;
                        break;

                    case 71 or 72 or 73 or 74 or 75://扩散
                        reactiontag = ReactionTags.Swirl;
                        break;

                    case 61 or 16://不反应，但是冰草共存
                        nextElement = 5;
                        break;

                    case 10 or 20 or 30 or 40 or 60://不反应，但是改变附着
                        nextElement = elementToApply;
                        break;

                    default://不反应，也不改变附着
                        nextElement = currElement;
                        break;
                }

                //冰草共存检测是否反应掉了冰
                if (currElement == 5 && reactiontag != ReactionTags.None)
                {
                    nextElement = 6;
                }
            }

            return reactiontag;
        }
        public void AttachElement()
        {

        }
        internal void GetDamageReaction(DamageVariable dvToPerson, out DamageVariable? mul)
        {
            var cha = Characters[dvToPerson.TargetIndex];
            ReactionTags tag = GetReaction(cha.Element, dvToPerson.Element, out int nextElement);
            dvToPerson.Damage += tag switch
            {
                ReactionTags.Vaporize or ReactionTags.Melt or ReactionTags.Overloaded=> 2,
                ReactionTags.Frozen or ReactionTags.SuperConduct or ReactionTags.ElectroCharged or
                ReactionTags.Bloom or ReactionTags.Burning or ReactionTags.Catalyze or ReactionTags.Crystallize => 1,
                _ => 0
            };
            mul = tag switch
            {
                ReactionTags.SuperConduct or ReactionTags.ElectroCharged => new(DamageSource.NoWhere, -1, 1, dvToPerson.TargetIndex, true),
                ReactionTags.Swirl => new(DamageSource.Addition, (cha.Element - 1) % 4 + 1, 1, dvToPerson.TargetIndex, true),
                _ => null
            };
            cha.Element = nextElement;
            dvToPerson.Reaction = tag;
        }
    }
}