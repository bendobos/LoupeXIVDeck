namespace Loupedeck.LoupeXIVDeckPlugin.commands
{
    using System;
    using System.Threading.Tasks;

    using static Loupedeck.LoupeXIVDeckPlugin.FFXIVGameTypes;

    public class FFXIVActionCommand : PluginDynamicCommand
    {
        private readonly IDisposable isApplicationReadySubscription;
        private Boolean isApplicationReady;

        public FFXIVActionCommand() : base("Execute Action", "Execute Action", "FFXIV Commands")
        {
            this.MakeProfileAction("text;<ActionType>:<ActionId> (e.g. Macro:2)");

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
                var action = this.ActionParameterToFFXIVAction(actionParameter);

                var result = Task.Run(async () => await LoupeXIVDeckPlugin.api.GetAction(action.type, action.id)).Result;

                var icon = Task.Run(async () => await LoupeXIVDeckPlugin.api.GetIcon(result.iconId));

                return BitmapImage.FromArray(icon.Result);
            }

            return base.GetCommandImage(actionParameter, imageSize);
        }

        protected override async void RunCommand(String actionParameter)
        {
            if (this.isApplicationReady)
            {
                var action = this.ActionParameterToFFXIVAction(actionParameter);

                await LoupeXIVDeckPlugin.api.ExecuteAction(action.type, action.id);
            }
        }

        /**
         * Converts an actionParameter formatted like `ActionType:ActionId` into a `FFXIVHotbarSlot`
         */
        private FFXIVAction ActionParameterToFFXIVAction(String actionParameter)
        {
            var paramArray = actionParameter.Split(':');

            return new FFXIVAction(paramArray[0], Int32.Parse(paramArray[1]));
        }

        protected override Boolean OnUnload() {
            this.isApplicationReadySubscription.Dispose();

            return base.OnUnload();
        }
    }
}
