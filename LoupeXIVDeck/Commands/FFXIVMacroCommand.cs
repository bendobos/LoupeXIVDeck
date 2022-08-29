namespace Loupedeck.LoupeXIVDeckPlugin.commands
{
    using System;
    using System.Threading.Tasks;

    internal class FFXIVMacroCommand : PluginDynamicCommand
    {
        private IFFXIVPluginLink _pluginLink;
        private IFFXIVApi _api;
        private IDisposable isApplicationReadySubscription;
        private Boolean isApplicationReady;

        public FFXIVMacroCommand() : base("Execute Macro", "Execute Macro", "FFXIV Commands")
        {
            this.MakeProfileAction("text;Macro ID");
        }

        protected override Boolean OnLoad() { 
            this.GetInstances();

            this.isApplicationReadySubscription = this._pluginLink.GetIsApplicationReadySubject()
                .Subscribe(ready => {
                    this.isApplicationReady = ready;
                    this.ActionImageChanged();
                });

            return true;
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            if (actionParameter != null && this.isApplicationReady)
            {
                var result = Task.Run(async () => await this._api.GetAction("Macro", Int32.Parse(actionParameter))).Result;

                var icon = Task.Run(async () => await this._api.GetIcon(result.iconId));

                return BitmapImage.FromArray(icon.Result);
            }

            return base.GetCommandImage(actionParameter, imageSize);
        }

        protected override async void RunCommand(String actionParameter)
        {
            if (this.isApplicationReady)
            {
                await this._api.ExecuteAction("Macro", Int32.Parse(actionParameter));
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
