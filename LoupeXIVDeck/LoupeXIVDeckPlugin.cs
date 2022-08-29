namespace Loupedeck.LoupeXIVDeckPlugin
{
    using System;

    using StructureMap;

    public class LoupeXIVDeckPlugin : Plugin
    {
        private IDisposable isApplicationReadySubscription;

        public override Boolean HasNoApplication => true;
        public override Boolean UsesApplicationApiOnly => true;
        public Container container;

        public LoupeXIVDeckPlugin() {
            this.SetupDiContainer();
        }

        public override void Load()
        {
            this.SetIcons();

            this.SetupFFXIVConnection();
        }

        private void SetupDiContainer()
        {
            this.container = new Container(new FFXIVRegistry());
        }

        private void SetupFFXIVConnection()
        {
            var pluginLink = this.container.GetInstance<IFFXIVPluginLink>();

            this.isApplicationReadySubscription = pluginLink.GetIsApplicationReadySubject().Subscribe(isReady =>
            {
                if (isReady)
                {
                    this.OnPluginStatusChanged(Loupedeck.PluginStatus.Normal, null, null, null);
                }
                else
                {
                    this.OnPluginStatusChanged(Loupedeck.PluginStatus.Error, 
                        Constants.NO_CONNECTION_ERROR_MESSAGE, 
                        Constants.SUPPORT_URL, 
                        Constants.SUPPORT_URL_TITLE);
                }
            });

            // Only connect after isApplicationReadySubject subscription is available
            pluginLink.Connect();
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
            this.isApplicationReadySubscription.Dispose();
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
