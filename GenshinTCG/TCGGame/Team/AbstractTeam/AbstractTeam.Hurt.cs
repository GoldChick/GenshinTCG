using TCGBase;
using TCGUtil;

namespace TCGGame
{
    public abstract partial class AbstractTeam
    {
        /// <param name="action">伤害结算后，死亡结算前结算的东西</param>
        public void MultiHurt(DamageVariable[] dvs, Action? action = null)
        {

        }
        /// <param name="action">伤害结算后，死亡结算前结算的东西</param>
        public void Hurt(DamageVariable dv, Action? action = null)
        {

        }
        public void Hurt(int element, int baseDamage, DamageSource source, int relativeIndex)
        {
            if (Characters.All(c => !c.Alive))
            {
                throw new Exception($"AbstractTeam.Hurt(): All Characters Died");
            }
            Character target;
            int curr = CurrCharacter;
            do
            {
                curr = (curr + relativeIndex) % Characters.Length;
                target = Characters[curr];
            } while (!target.Alive);

            //element damage经过normalize
            DamageVariable d = new(element, baseDamage, source, curr);

            EffectTrigger(Game, TeamIndex, new SimpleSender(Tags.SenderTags.ELEMENT_ENCHANT), d);
            Game.Teams[1 - TeamIndex].EffectTrigger(Game, 1 - TeamIndex, new SimpleSender(Tags.SenderTags.DAMAGE_ADD), d);
            EffectTrigger(Game, TeamIndex, new SimpleSender(Tags.SenderTags.HURT_ADD), d);
            Game.Teams[1 - TeamIndex].EffectTrigger(Game, 1 - TeamIndex, new SimpleSender(Tags.SenderTags.DAMAGE_MUL), d);
            EffectTrigger(Game, TeamIndex, new SimpleSender(Tags.SenderTags.HURT_MUL), d);

            target.HP -= d.BaseDamage + (d.Element == -1 ? 0 : d.DamageModifier);
            //TODO:元素反应
            if (target.HP == 0)
            {
                EffectTrigger(Game, TeamIndex, new DieSender(curr, true), null);
                if (target.HP == 0)
                {
                    EffectTrigger(Game, TeamIndex, new DieSender(curr), null);
                    target.Alive = false;
                    //TODO:掉装备
                    //TODO:全死了之后如何结束  
                    //TODO:选择时间
                    Game.RequestAndHandleEvent(TeamIndex, 30000, ActionType.SwitchForced, "Character Died");
                }
            }
        }
        public static class Reaction
        {
            /// <param name="currElement">角色身上附着的元素(只允许附着 无0 冰1 水2 火3 雷4 草6 <b>冰+草5</b>)</param>
            ///<param name="nextElement">经过反应处理后，身上将会是的元素种类</param>
            /// <returns></returns>
            public static string? GetReaction(int currElement, int inputElement, out int nextElement)
            {
                string? reaction = null;

                nextElement = currElement;

                if (inputElement > 0)
                {
                    nextElement = 0;

                    int reactionType = inputElement * 10 + currElement;
                    switch (reactionType)
                    {
                        case 12 or 21://冻结
                        case 25:
                            reaction = Tags.ReactionTags.FROZEN;
                            break;

                        case 13 or 31://融化
                        case 35:
                            reaction = Tags.ReactionTags.MELT;
                            break;

                        case 14 or 41://超导
                        case 45:
                            reaction = Tags.ReactionTags.SUPERCONDUCT;
                            break;

                        case 23 or 32://蒸发
                            reaction = Tags.ReactionTags.VAPORIZE;
                            break;

                        case 24 or 42://感电
                            reaction = Tags.ReactionTags.ELECTRO_CHARGED;
                            break;

                        case 34 or 43://超载
                            reaction = Tags.ReactionTags.OVERLOADED;
                            break;

                        case 51 or 52 or 53 or 54://结晶
                        case 55:
                            reaction = Tags.ReactionTags.CRYSTALLIZE;
                            break;

                        //NOTE:冰草共存优先反应冰

                        case 62 or 26://绽放
                            reaction = Tags.ReactionTags.BLOOM;
                            break;

                        case 63 or 36://燃烧
                            reaction = Tags.ReactionTags.BURNING;
                            break;

                        case 64 or 46://激化
                            reaction = Tags.ReactionTags.CATALYZE;
                            break;

                        case 71 or 72 or 73 or 74://扩散
                        case 75:
                            reaction = Tags.ReactionTags.SWIRL;
                            break;

                        case 61 or 16://不反应，但是冰草共存
                            nextElement = 5;
                            break;

                        case 10 or 20 or 30 or 40://不反应，但是改变附着
                            nextElement = inputElement;
                            break;

                        default://不反应，也不改变附着
                            nextElement = currElement;
                            break;
                    }

                    //冰草共存检测是否反应掉了冰
                    if (currElement == 5 && reaction != null)
                    {
                        nextElement = 6;
                    }
                }

                return reaction;
            }
        }

    }
}
