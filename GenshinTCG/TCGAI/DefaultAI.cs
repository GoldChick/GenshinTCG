
namespace TCGAI
{
    public class DefaultAI : AbstractAI
    {
        public DefaultAI(IGameInfo program) : base(program)
        {
            Name = "default";
        }

        public override AIEvent Act()
        {
            return CreateEvent.UseSKill(0);
        }

        public override AIEvent Blend()
        {
            throw new NotImplementedException();
        }

        public override AIEvent Pass() => CreateEvent.Pass();

        public override AIEvent ReRollCard()
        {
            throw new NotImplementedException();
        }

        public override AIEvent ReRollDice()
        {
            throw new NotImplementedException();
        }

        public override AIEvent Switch() => CreateEvent.Switch(0);

        public override AIEvent UseCard()
        {
            throw new NotImplementedException();
        }

        public override AIEvent UseSkill()
        {
            throw new NotImplementedException();
        }
    }
}
