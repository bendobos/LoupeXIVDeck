namespace Loupedeck.LoupeXIVDeck.messages.outbound
{
    using System;
    public class InitOpCode
    {
        public String opcode = "init";
        public String version;
        public String mode;

        public InitOpCode(String version, String mode)
        {
            this.version = version;
            this.mode = mode;
        }
    }
}
