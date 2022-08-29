namespace Loupedeck.LoupeXIVDeckPlugin
{
    using System;
    using System.Collections.Generic;

    public static class Constants
    {
        private static readonly Int32 websocketReconnectTimeoutInSeconds = 15;

        public static readonly TimeSpan WEBSOCKET_RECONNECT_TIMEOUT = 
            TimeSpan.FromSeconds(websocketReconnectTimeoutInSeconds);

        public static readonly String NO_CONNECTION_ERROR_MESSAGE =
            "No connection to Game Plugin Websocket. " +
            "Make sure you have the XIVDeck Game Plugin installed and the port set to default (37984). " +
            "Retrying connection every 30 seconds...";

        public static readonly String SUPPORT_URL = "https://github.com/KazWolfe/XIVDeck";

        public static readonly String SUPPORT_URL_TITLE = "See XIVDeck Github for instructions";

        public static readonly Dictionary<String, String> SOUNDSETTER_DICT = new Dictionary<String, String>() {
            { "ssmv", "Master Volume" },
            { "ssbgm", "BGM Volume" },
            { "sssfx", "SFX Volume" },
            { "ssv", "Voice Volume" },
            { "sssys", "System Volume" },
            { "ssas", "Ambient Sound Volume" },
            { "ssp", "Performance Volume" }
        };
    }
}
