using System.Text.Json.Serialization;
using TCGBase;

namespace Minecraft
{
    /// <summary>
    /// 出现[风元素扩散反应]，向data中添加1 2 3 4（最多1次）
    /// </summary>
    public class ColorWhenHurt : AbstractTriggerable, IWhenThenAction
    {
        public override string NameID { get => "colorwhenhurt"; protected set { } }

        public override string Tag => SenderTag.AfterHurt.ToString();

        public bool Once { get; }
        public bool NeedSwirl { get; }
        public List<ConditionRecordBase> When { get; }
        public ColorWhenHurt(bool once = true, bool needswirl = true, List<ConditionRecordBase>? when = null)
        {
            Once = once;
            NeedSwirl = needswirl;
            When = when ?? new List<ConditionRecordBase>();
        }
        /// <summary>
        /// 不考虑CurrCharacter为-1
        /// </summary>
        public override void Trigger(PlayerTeam me, Persistent persitent, AbstractSender sender, AbstractVariable? variable)
        {
            if ((this as IWhenThenAction).IsConditionValid(me, persitent, sender, variable) && variable is DamageVariable dv)
            {
                if (persitent.Data.Count == 0 || !Once)
                {
                    if (NeedSwirl)
                    {
                        switch (dv.Reaction)
                        {
                            case ReactionTags.SwirlCryo:
                                persitent.Data.Clear();
                                persitent.Data.Add(1);
                                break;
                            case ReactionTags.SwirlHydro:
                                persitent.Data.Clear();
                                persitent.Data.Add(2);
                                break;
                            case ReactionTags.SwirlPyro:
                                persitent.Data.Clear();
                                persitent.Data.Add(3);
                                break;
                            case ReactionTags.SwirlElectro:
                                persitent.Data.Clear();
                                persitent.Data.Add(4);
                                break;
                        }
                    }
                    else
                    {
                        switch (dv.Element)
                        {
                            case DamageElement.Cryo:
                                persitent.Data.Clear();
                                persitent.Data.Add(1);
                                break;
                            case DamageElement.Hydro:
                                persitent.Data.Clear();
                                persitent.Data.Add(2);
                                break;
                            case DamageElement.Pyro:
                                persitent.Data.Clear();
                                persitent.Data.Add(3);
                                break;
                            case DamageElement.Electro:
                                persitent.Data.Clear();
                                persitent.Data.Add(4);
                                break;
                        }
                    }
                }
            }
        }
    }
}
