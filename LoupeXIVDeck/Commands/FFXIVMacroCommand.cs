namespace Loupedeck.LoupeXIVDeck.commands
{
    using System;
    using System.Threading.Tasks;

    internal class FFXIVMacroCommand : PluginDynamicCommand
    {
        private readonly IDisposable isApplicationReadySubscription;
        private Boolean isApplicationReady;

        public FFXIVMacroCommand() : base("Execute Macro", "Execute Macro", "FFXIV Commands")
        {
            this.MakeProfileAction("text;Macro ID");

            this.isApplicationReadySubscription = LoupeXIVDeckPlugin.pluginLink.isApplicationReadySubject
                .Subscribe(ready => { 
                    this.isApplicationReady = ready; 
                    this.ActionImageChanged(); 
                });
        }
        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            if (actionParameter != null && this.isApplicationReady)
            {
                var result = Task.Run(async () => await LoupeXIVDeckPlugin.api.GetAction("Macro", Int32.Parse(actionParameter))).Result;

                var icon = Task.Run(async () => await LoupeXIVDeckPlugin.api.GetIcon(result.iconId));

                return BitmapImage.FromArray(icon.Result);
            }

            return base.GetCommandImage(actionParameter, imageSize);
        }

        protected override async void RunCommand(String actionParameter)
        {
            if (this.isApplicationReady)
            {
                await LoupeXIVDeckPlugin.api.ExecuteAction("Macro", Int32.Parse(actionParameter));
            }
        }

        protected override Boolean OnUnload()
        {
            this.isApplicationReadySubscription.Dispose();

            return base.OnUnload();
        }
    }
}
