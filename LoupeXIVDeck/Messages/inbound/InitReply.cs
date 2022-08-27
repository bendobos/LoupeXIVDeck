namespace Loupedeck.LoupeXIVDeckPlugin.messages.inbound
{
    using System;

    internal class InitReply
    {
        public String messageType = "initReply";
        public String version;
        public String apiKey;

        public InitReply(String version, String apiKey)
        {
            this.version = version;
            this.apiKey = apiKey;
        }
    }
}
