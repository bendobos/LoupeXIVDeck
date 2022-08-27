﻿namespace Loupedeck.LoupeXIVDeck
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Reactive.Linq;

    using Websocket.Client;
    using Newtonsoft.Json;
    using Loupedeck.LoupeXIVDeck.messages.inbound;
    using Loupedeck.LoupeXIVDeck.messages.outbound;
    using System.Reactive.Subjects;

    public class FFXIVPluginLink
    {
        public static Int32 port = 37984;
        public static String apiKey = "";
        public Boolean isApplicationReady = false;
        public Subject<Boolean> isApplicationReadySubject = new Subject<Boolean>();

        private WebsocketClient client;

        public static String GetBaseUrl()
        {
            return $"http://localhost:{port}";
        }

        public static String GetApiKey()
        {
            return apiKey;
        }

        async public void Connect()
        {
            var url = new Uri($"ws://localhost:{FFXIVPluginLink.port}/ws");

            this.client = new WebsocketClient(url);
            this.client.ReconnectTimeout = Constants.WEBSOCKET_RECONNECT_TIMEOUT;
            this.client.ErrorReconnectTimeout = Constants.WEBSOCKET_RECONNECT_TIMEOUT;

            this.InitSubscriptions();

            await this.client.Start();  
        }

        private void InitSubscriptions()
        {
            this.client.DisconnectionHappened.Subscribe(info => {
                System.Diagnostics.Debug.WriteLine($"## Disconnection happened, type {info.Type}");

                // Set not ready state if a disconnection happens that is not because no messages received.
                // NoMessageReceived will reconnect automatically.
                if (info.Type != DisconnectionType.NoMessageReceived)
                {
                    this.isApplicationReadySubject.OnNext(false);
                }
            });

            this.client.ReconnectionHappened.Subscribe(info => {
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
            // Sends a fake version so no errors come up
            var initMessage = JsonConvert.SerializeObject(new InitOpCode("0.2.13", "Plugin"));

            System.Diagnostics.Debug.WriteLine($"## Sending Message: {initMessage}");
            var result = Task.Run(() => client.Send(initMessage));
        }

        private void OnInitReceive(ResponseMessage msg) {
            System.Diagnostics.Debug.WriteLine($"## Received initReply: {msg}");
            var initReply = JsonConvert.DeserializeObject<InitReply>(msg.ToString());

            FFXIVPluginLink.apiKey = initReply.apiKey;
            System.Diagnostics.Debug.WriteLine($"## Got an API key: {FFXIVPluginLink.apiKey}");

            System.Diagnostics.Debug.WriteLine($"## Updating isReady state");
            this.isApplicationReadySubject.OnNext(true);
        }
    }
}
