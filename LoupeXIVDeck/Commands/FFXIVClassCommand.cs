namespace Loupedeck.LoupeXIVDeckPlugin.commands
{
    using System;
    using System.Threading.Tasks;

    internal class FFXIVClassCommand : PluginDynamicCommand
    {
        private readonly IDisposable isApplicationReadySubscription;
        public Boolean isApplicationReady;

        public FFXIVClassCommand() : base("Trigger Class", "Trigger Class", "FFXIV Commands")
        {
            this.MakeProfileAction("text;Class ID");

            this.isApplicationReadySubscription = LoupeXIVDeckPlugin.pluginLink.isApplicationReadySubject
                .Subscribe(ready => {
                    this.isApplicationReady = ready;
                    this.ActionImageChanged();
                });
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            if (actionParameter != null && Int32.Parse(actionParameter) <= 40 && this.isApplicationReady)
            {
                var result = Task.Run(async () => await LoupeXIVDeckPlugin.api.GetClass(Int32.Parse(actionParameter))).Result;

                var icon = Task.Run(async () => await LoupeXIVDeckPlugin.api.GetIcon(result.iconId));

                return BitmapImage.FromArray(icon.Result);
            }

            return base.GetCommandImage(actionParameter, imageSize);
        }

        protected override async void RunCommand(String actionParameter) 
        {
            if (this.isApplicationReady)
            {
                await LoupeXIVDeckPlugin.api.TriggerClass(Int32.Parse(actionParameter));
            }
        }

        protected override Boolean OnUnload() {
            this.isApplicationReadySubscription.Dispose();

            return base.OnUnload();
        }
    }
}
