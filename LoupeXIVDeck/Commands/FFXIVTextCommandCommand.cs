namespace Loupedeck.LoupeXIVDeckPlugin.commands
{
    using System;

    public class FFXIVTextCommandCommand : PluginDynamicCommand
    {
        private IFFXIVPluginLink _pluginLink;
        private IFFXIVApi _api;
        private IDisposable isApplicationReadySubscription;
        private Boolean isApplicationReady;

        public FFXIVTextCommandCommand() : base("Execute Text Command", "Execute Text Command", "FFXIV Commands")
        {
            this.MakeProfileAction("text;Command to be executed (e.g. '/echo <se.1>)");
        }

        protected override Boolean OnLoad()
        {
            this.GetInstances();

            this.isApplicationReadySubscription = this._pluginLink.GetIsApplicationReadySubject()
                .Subscribe(ready => this.isApplicationReady = ready);

            return true;
        }

        async protected override void RunCommand(String actionParameter) {
            if (this.isApplicationReady)
            {
                await this._api.RunTextCommand(actionParameter);
            }
        }

        protected override Boolean OnUnload()
        {
            this.isApplicationReadySubscription.Dispose();

            return base.OnUnload();
        }

        private void GetInstances()
        {
            var plugin = (LoupeXIVDeckPlugin)base.Plugin;
            var container = plugin.container;

            this._pluginLink = container.GetInstance<IFFXIVPluginLink>();
            this._api = container.GetInstance<IFFXIVApi>();
        }
    }
}
