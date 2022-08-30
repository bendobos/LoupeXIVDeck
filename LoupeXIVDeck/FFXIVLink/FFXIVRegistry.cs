namespace Loupedeck.LoupeXIVDeckPlugin
{
    using StructureMap;

    public class FFXIVRegistry : Registry
    {
        public FFXIVRegistry()
        {
            this.For<IFFXIVPluginLink>().Use<FFXIVPluginLink>().Singleton();
            this.For<IFFXIVApi>().Use<FFXIVApi>().Singleton();
        }
    }
}
