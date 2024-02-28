using TCGBase;

namespace Minecraft
{
    public class Minecraft_Util : AbstractModUtil
    {
        public override string NameSpace => "minecraft";

        public override string Description => "built-in triggerable";

        public override string Author => "Gold_Chick";

        public override string[] GetDependencies() => Array.Empty<string>();

        public override AbstractRegister GetRegister() => new Minecraft_Register();
    }
}
