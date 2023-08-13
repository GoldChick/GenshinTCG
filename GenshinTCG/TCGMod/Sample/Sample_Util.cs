using TCGMod;
using TCGRule;

namespace Sample
{
    public class Sample_Util : AbstractModUtil
    {
        public override string NameSpace => "sample";

        public override string Description => throw new NotImplementedException();

        public override string Author => throw new NotImplementedException();

        public override string[] GetDependencies()
        {
            throw new NotImplementedException();
        }

        public override AbstractRegister GetRegister() => new Sample_Register();
    }
}
