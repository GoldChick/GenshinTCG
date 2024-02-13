namespace TCGBase
{
    //TODO: 类似 CostRecord，是否考虑合并一下
    public class SingleCostVariable : AbstractVariable
    {
        private int _count;
        public ElementCategory Element { get; }
        public int Count { get => _count; set => _count = int.Max(0, value); }

        public SingleCostVariable(ElementCategory element, int count)
        {
            Element = element;
            Count = count;
        }
    }

    public class CostModifierTriggerable : AbstractTriggerable
    {
        public override string NameID { get; protected set; }

        public override string Tag => _element.ToString();
        public int Value { get; }

        private ElementCategory _element;
        private DiceModifierType Demand => _element switch
        {
            ElementCategory.Trival => DiceModifierType.Any,
            ElementCategory.Void => DiceModifierType.Void,
            _ => DiceModifierType.Color
        };
        public override void Trigger(PlayerTeam me, Persistent persitent, AbstractSender sender, AbstractVariable? variable)
        {
            //dms: void=>color=>any=>if
            //scv: trival / color=>void
            if (me.TeamIndex == sender.TeamID && sender is DiceModifierSender dms && variable is SingleCostVariable scv)
            {
                if (dms.DiceModType == Demand)
                {
                    switch (Demand)
                    {
                        case DiceModifierType.Void:
                            if (scv.Element == ElementCategory.Void)
                            {
                                //yes
                            }
                            break;
                        case DiceModifierType.Color:
                            if (scv.Element == ElementCategory.Void)
                            {
                                //yes
                            }
                            else if ((int)scv.Element > 0 && (int)scv.Element <= 7 && scv.Element == _element)
                            {
                                //yes
                            }
                            break;
                        case DiceModifierType.Any:
                            //yes
                            break;
                        case DiceModifierType.If:
                            if (Value >= scv.Count)
                            {
                                //yes
                            }
                            break;
                        default:
                            break;
                    }
                }

                //NOTE: conditions
                if (true)
                {
                    scv.Count -= Value;
                }
            }
        }
    }
}
