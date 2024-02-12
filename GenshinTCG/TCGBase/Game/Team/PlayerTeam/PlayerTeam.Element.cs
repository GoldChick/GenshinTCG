using Minecraft;
namespace TCGBase
{
    public partial class PlayerTeam
    {
        private static ReactionTags GetReaction(int currElement, DamageElement elementToApply, out int nextElementState)
        {
            ReactionTags reactiontag = ReactionTags.None;
            //角色身上附着的元素(只允许附着 无0 冰1 水2 火3 雷4 草6 <b>冰+草5</b>
            nextElementState = currElement;

            if (elementToApply is not (DamageElement.Trival or DamageElement.Pierce))
            {
                nextElementState = 0;

                int reactionType = (int)elementToApply * 10 + currElement;
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
                        nextElementState = 5;
                        break;

                    case 10 or 20 or 30 or 40 or 60://不反应，但是改变附着
                        nextElementState = (int)elementToApply;
                        break;

                    default://不反应，也不改变附着
                        nextElementState = currElement;
                        break;
                }

                //冰草共存检测是否反应掉了冰
                if (currElement == 5 && reactiontag != ReactionTags.None)
                {
                    nextElementState = 6;
                }
            }

            return reactiontag;
        }
        private bool ReactionItemGenerate(int targetindex, ReactionTags tag, AbstractCustomTriggerable source, int initialelement)
        {
            var egv = new ElementGenerateVariable(false, null);
            Game.EffectTrigger(new PreHurtSender(TeamIndex, source, SenderTag.ElementItemGenerate, initialelement), egv);

            egv.GenerateAction?.Invoke(this, targetindex);
            if (!egv.OverrideInitialGenerator)
            {
                switch (tag)
                {
                    case ReactionTags.Frozen:
                        //TODO:frozen?
                        break;
                    case ReactionTags.Overloaded:
                        return targetindex == CurrCharacter;
                    case ReactionTags.Crystallize:
                        //Enemy.AddPersistent(new Effect_Crystal());
                        break;
                    case ReactionTags.Bloom:
                        //Enemy.AddPersistent(new Effect_DendroCore());
                        break;
                    case ReactionTags.Burning:
                        //Enemy.AddSummon(new Summon_Burning());
                        break;
                    case ReactionTags.Catalyze:
                        //Enemy.AddPersistent(new Effect_CatalyzeField());
                        break;
                }
            }
            return false;
        }
        private int GetDamageReaction(DamageVariable dvToPerson)
        {
            var cha = Characters[dvToPerson.TargetIndex];
            ReactionTags tag = GetReaction(cha.Element, dvToPerson.Element, out int nextElement);
            dvToPerson.Damage += tag switch
            {
                ReactionTags.Vaporize or ReactionTags.Melt or ReactionTags.Overloaded => 2,
                ReactionTags.Frozen or ReactionTags.SuperConduct or ReactionTags.ElectroCharged or
                ReactionTags.Bloom or ReactionTags.Burning or ReactionTags.Catalyze or ReactionTags.Crystallize => 1,
                _ => 0
            };
            dvToPerson.SubDamage = tag switch
            {
                ReactionTags.SuperConduct or ReactionTags.ElectroCharged => new(DamageSource.Indirect, DamageElement.Pierce, 1, dvToPerson.TargetIndex, DamageTargetArea.TargetExcept, dvToPerson.DamageTargetTeam),
                ReactionTags.Swirl => new(DamageSource.Indirect, (DamageElement)((cha.Element - 1) % 4 + 1), 1, dvToPerson.TargetIndex, DamageTargetArea.TargetExcept, dvToPerson.DamageTargetTeam),
                _ => null
            };
            int initialelement = cha.Element;

            cha.Element = nextElement;
            dvToPerson.Reaction = tag;
            return initialelement;
        }
        public void AttachElement(AbstractCustomTriggerable source, DamageElement element, params int[] targetRelativeIndexs)
        {
            var chas = targetRelativeIndexs.Distinct().Select(i => Characters[(i + CurrCharacter) % Characters.Length]);
            var tags = chas.Select(c => (GetReaction(c.Element, element, out int nextElement), nextElement));
            var overload = chas.Select((c, index) => ReactionItemGenerate(c.PersistentRegion, tags.ElementAt(index).Item1, source, c.Element)).Any(p => p);
            for (int i = 0; i < chas.Count(); i++)
            {
                chas.ElementAt(i).Element = tags.ElementAt(i).nextElement;
            }
            //TODO:broadcast
            if (overload)
            {
                TrySwitchToIndex(1, true);
            }
        }

    }
}
