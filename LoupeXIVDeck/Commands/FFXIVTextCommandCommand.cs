namespace Loupedeck.LoupeXIVDeckPlugin.commands
{
    using System;

    public class FFXIVTextCommandCommand : PluginDynamicCommand
    {
        private readonly IDisposable isApplicationReadySubscription;
        private Boolean isApplicationReady;

        public FFXIVTextCommandCommand() : base("Execute Text Command", "Execute Text Command", "FFXIV Commands")
        {
            this.MakeProfileAction("text;Command to be executed (e.g. '/echo <se.1>)");

            this.isApplicationReadySubscription = LoupeXIVDeckPlugin.pluginLink.isApplicationReadySubject
                .Subscribe(ready => this.isApplicationReady = ready);
        }

        async protected override void RunCommand(String actionParameter) {
            if (this.isApplicationReady)
            {
                await LoupeXIVDeckPlugin.api.RunTextCommand(actionParameter);
            }
        }
        protected override Boolean OnUnload()
        {
            this.isApplicationReadySubscription.Dispose();

            return base.OnUnload();
        }
    }
}
