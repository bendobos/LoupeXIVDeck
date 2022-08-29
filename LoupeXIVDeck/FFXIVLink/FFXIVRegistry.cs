namespace Loupedeck.LoupeXIVDeckPlugin
{
    using StructureMap;

    public class FFXIVRegistry : Registry
    {
        public FFXIVRegistry()
        {
            this.For<IFFXIVPluginLink>().Use<FFXIVPluginLink>();
            this.For<IFFXIVApi>().Use<FFXIVApi>();
        }
    }
}
