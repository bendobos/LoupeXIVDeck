namespace Loupedeck.LoupeXIVDeckPlugin.messages.outbound
{
    using System;
    public class InitOpCode
    {
        public String Opcode { get; private set; } = "init";
        public String Version { get; set; }
        public String Mode { get; set; }
    }
}
