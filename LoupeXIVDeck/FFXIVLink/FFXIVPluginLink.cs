namespace Loupedeck.LoupeXIVDeckPlugin
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;

    using Websocket.Client;
    using Loupedeck.LoupeXIVDeckPlugin.messages.inbound;
    using Loupedeck.LoupeXIVDeckPlugin.messages.outbound;

    public class FFXIVPluginLink : IFFXIVPluginLink
    {
        private readonly Int32 port = 37984;
        private readonly Subject<Boolean> isApplicationReadySubject = new Subject<Boolean>();
        private String apiKey = "";
        private WebsocketClient client;

        public String GetBaseUrl()
        {
            return $"http://localhost:{this.port}";
        }

        public String GetApiKey()
        {
            return this.apiKey;
        }

        public Subject<Boolean> GetIsApplicationReadySubject()
        {
            return this.isApplicationReadySubject;
        }

        async public void Connect()
        {
            var url = new Uri($"ws://localhost:{this.port}/ws");

            this.client = new WebsocketClient(url);
            this.client.ReconnectTimeout = Constants.WEBSOCKET_RECONNECT_TIMEOUT;
            this.client.ErrorReconnectTimeout = Constants.WEBSOCKET_RECONNECT_TIMEOUT;

            this.InitSubscriptions();

            await this.client.Start();
        }

        private void InitSubscriptions()
        {
            this.client.DisconnectionHappened.Subscribe(info =>
            {
                System.Diagnostics.Debug.WriteLine($"## Disconnection happened, type {info.Type}");

                // Set not ready state if a disconnection happens that is not because no messages received.
                // NoMessageReceived will reconnect automatically.
                if (info.Type != DisconnectionType.NoMessageReceived)
                {
                    this.isApplicationReadySubject.OnNext(false);
                }
            });

            this.client.ReconnectionHappened.Subscribe(info =>
            {
                System.Diagnostics.Debug.WriteLine($"## Reconnection happened, type: {info.Type}");

                // Re-Initialize on a reconnection.
                // Also used for initial initialization by sending an init message.
                // NoMessageReceived occurs only when a connection has been established before,
                // so there is no need to re-initialize
                if (info.Type != ReconnectionType.NoMessageReceived)
                {
                    this.SendInit(this.client);
                }
            });

            this.client
                .MessageReceived
                .Where(msg => msg.Text != null)
                .Where(msg => !msg.Text.Contains("initReply"))
                .Subscribe(msg => System.Diagnostics.Debug.WriteLine($"## WS Message received: {msg}"));

            this.client
                .MessageReceived
                .Where(msg => msg.Text != null)
                .Where(msg => msg.Text.Contains("initReply"))
                .Subscribe(msg => this.OnInitReceive(msg));
        }

        private void SendInit(WebsocketClient client)
        {
            var opCode = new InitOpCode
            {
                Version = "0.2.13",
                Mode = "Plugin"
            };

            // Sends a fake version so no errors come up
            var initMessage = JsonHelpers.SerializeAnyObject(opCode);
            var initMessage2 = JsonHelpers.SerializeAnyObject(opCode);

            System.Diagnostics.Debug.WriteLine($"## Sending Message: {initMessage}");
            System.Diagnostics.Debug.WriteLine($"## Sending Message: {initMessage2}");

            var result = Task.Run(() => client.Send(initMessage));
        }

        private void OnInitReceive(ResponseMessage msg)
        {
            System.Diagnostics.Debug.WriteLine($"## Received initReply: {msg}");
            var initReply = JsonHelpers.DeserializeObject<InitReply>(msg.ToString());

            this.apiKey = initReply.apiKey;
            System.Diagnostics.Debug.WriteLine($"## Got an API key: {this.apiKey}");

            this.isApplicationReadySubject.OnNext(true);
        }
    }
}
