namespace Loupedeck.LoupeXIVDeck
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class Constants
    {
        private static readonly Int32 websocketReconnectTimeoutInSeconds = 15;

        public static readonly TimeSpan WEBSOCKET_RECONNECT_TIMEOUT = 
            TimeSpan.FromSeconds(websocketReconnectTimeoutInSeconds);
    }
}
