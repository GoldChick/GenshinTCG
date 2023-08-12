using TCGMod;
using TCGRule;

namespace Genshin3_3
{
    public class Genshin_3_3_Util : AbstractModUtil
    {
        public override string NameSpace => "genshin3_3";

        public override string Description => throw new NotImplementedException();

        public override string Author => throw new NotImplementedException();

        public override string[] GetDependencies()
        {
            throw new NotImplementedException();
        }

        public override AbstractRegister GetRegister() => new Genshin3_3_Register();
    }
}
