using System.Text.Json.Serialization;
using TCGBase;

namespace Minecraft
{
    public class ColoredSummon : AbstractTriggerable
    {
        public override string NameID { get => "coloredsummon"; protected set { } }

        public override string Tag => SenderTag.RoundOver.ToString();

        public DamageRecord Damage { get; }
        public List<ActionRecordBase> With { get; } 
        [JsonConstructor]
        public ColoredSummon(DamageRecord damage, List<ActionRecordBase>? with = null)
        {
            Damage = damage;
            With = with??new();
        }
        /// <summary>
        /// 不考虑CurrCharacter为-1
        /// </summary>
        public override void Trigger(PlayerTeam me, Persistent persitent, AbstractSender sender, AbstractVariable? variable)
        {
            var damage = Damage;

            int color = persitent.Data.First();
            if (color > 0 && color < 9)
            {
                damage = new((DamageElement)color, damage.Amount, damage.TargetIndexOffset, damage.TargetArea, damage.Team, damage.SubDamage);
            }

            me.DoDamage(damage, persitent, this, ()=>
            {
                foreach (var action in With)
                {
                    action.GetHandler(this)?.Invoke(me,persitent,sender,variable);
                }
            });
        }
    }
}
