//大蝴蝶增伤，但是不想写了

//using System.Text.Json.Serialization;
//using TCGBase;

//namespace Minecraft
//{
//    public class ColoredDamageIncrease : AbstractTriggerable, IWhenThenAction
//    {
//        public override string NameID { get => "coloreddamageincrease"; protected set { } }

//        public override string Tag => SenderTag.DamageIncrease.ToString();

//        public int Value { get; }
//        public int Consume { get; }
//        public List<ConditionRecordBase> When { get; }

//        [JsonConstructor]
//        public ColoredDamageIncrease(DamageRecord damage, int consume = 1)
//        {
//            Damage = damage;
//            Consume = consume;
//        }
//        /// <summary>
//        /// 不考虑CurrCharacter为-1
//        /// </summary>
//        public override void Trigger(PlayerTeam me, Persistent persitent, AbstractSender sender, AbstractVariable? variable)
//        {
//        }
//    }
//}
