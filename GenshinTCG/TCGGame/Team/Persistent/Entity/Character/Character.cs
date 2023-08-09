namespace TCGGame
{
    public class Character : AbstractEntity
    {
        public override EntityType Type => EntityType.Character;
        
        public string Artifact;
        public string Weapon;
        public string Nature;

        public List<Effect> Effects=new();
        

        public void EffectAct()
        {

        }
    }

}
