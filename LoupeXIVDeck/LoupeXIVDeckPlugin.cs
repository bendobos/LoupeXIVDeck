namespace Loupedeck.LoupeXIVDeck
{
    using System;

    public class LoupeXIVDeckPlugin : Plugin
    {
        // Make application dynamic and not bound to a specific process
        public override Boolean HasNoApplication => true;
        public override Boolean UsesApplicationApiOnly => true;

        public static FFXIVPluginLink pluginLink = new FFXIVPluginLink();
        public static FFXIVApi api;

        public override void Load()
        {
            this.SetIcons();

            LoupeXIVDeckPlugin.pluginLink.isApplicationReadySubject.Subscribe(isReady =>
            {
                System.Diagnostics.Debug.WriteLine($"## Received isReady state: {isReady}");

                if (isReady)
                {
                    LoupeXIVDeckPlugin.api = new FFXIVApi();

                    this.OnPluginStatusChanged(Loupedeck.PluginStatus.Normal, null, null, null);
                } else
                {
                    this.OnPluginStatusChanged(Loupedeck.PluginStatus.Error,
                        "No connection to Game Plugin Websocket. " +
                        "Make sure you have the XIVDeck Game Plugin installed and the port set to default (37984). " +
                        "Retrying connection every 30 seconds...",
                        "https://github.com/KazWolfe/XIVDeck",
                        "See XIVDeck Github for instructions"
                    );
                }
            });

            // Only connect after isApplicationReadySubject subscription is available,
            // otherwise it won't be triggered (i.e. if connecting in the constructor if FFXIVPluginLink.
            LoupeXIVDeckPlugin.pluginLink.Connect();
        }

        private void SetIcons()
        {
            this.Info.Icon16x16 = EmbeddedResources.ReadImage(EmbeddedResources.FindFile("icon_16.png"));
            this.Info.Icon32x32 = EmbeddedResources.ReadImage(EmbeddedResources.FindFile("icon_32.png"));
            this.Info.Icon48x48 = EmbeddedResources.ReadImage(EmbeddedResources.FindFile("icon_48.png"));
            this.Info.Icon256x256 = EmbeddedResources.ReadImage(EmbeddedResources.FindFile("icon_256.png"));
        }

        public override void Unload()
        {
        }

        private void OnApplicationStarted(Object sender, EventArgs e)
        {
        }

        private void OnApplicationStopped(Object sender, EventArgs e)
        {
        }

        public override void RunCommand(String commandName, String parameter)
        {
        }

        public override void ApplyAdjustment(String adjustmentName, String parameter, Int32 diff)
        {
        }
    }
}
