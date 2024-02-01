//using TCGBase;

//namespace Minecraft
//{
//    public class Effect_CatalyzeField : AbstractCardEffect
//    {
//        public override string NameID => "effect_catalyze";
//        public override int MaxUseTimes => 2;

//        public override PersistentTriggerList TriggerList => new()
//        {
//            {SenderTag.DamageIncrease,(me,p,s,v)=>
//                {
//                    if (s.TeamID==me.TeamIndex && v is DamageVariable dv && dv.TargetIndex==me.CurrCharacter && (dv.Element==4||dv.Element==6))
//                    {
//                        dv.Damage++;
//                        p.AvailableTimes--;
//                    }
//                }
//            }
//        };
//    }
//}
