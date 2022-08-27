namespace Loupedeck.LoupeXIVDeckPlugin.commands
{
    using System;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using static Loupedeck.LoupeXIVDeckPlugin.FFXIVGameTypes;

    public class FFXIVHotbarButtonCommand : PluginDynamicCommand
    {
        private readonly IDisposable isApplicationReadySubscription;
        private Boolean isApplicationReady;

        public FFXIVHotbarButtonCommand() : base("Press Hotbar Button", "Press Hotbar Button", "FFXIV Commands")
        {
            this.MakeProfileAction("text;<HotbarID>:<SlotID> (e.g. 0:4)");

            this.isApplicationReadySubscription = LoupeXIVDeckPlugin.pluginLink.isApplicationReadySubject
                .Subscribe(ready => {
                    this.isApplicationReady = ready;
                    this.ActionImageChanged();
                });
        }

        async protected override void RunCommand(String actionParameter)
        {
            if (this.isApplicationReady)
            {
                var hotbarSlot = this.ActionParameterToHotbarSlot(actionParameter);

                await LoupeXIVDeckPlugin.api.TriggerHotbarSlot(hotbarSlot.hotbarId, hotbarSlot.slotId);
            }
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize) 
        {
            if (actionParameter != null && this.isApplicationReady)
            {
                var hotbarSlot = this.ActionParameterToHotbarSlot(actionParameter);

                // The following is a bit ugly but we need to work with async data in a sync function

                var task = Task.Run(async () => await LoupeXIVDeckPlugin.api.GetHotbarSlot(hotbarSlot.hotbarId, hotbarSlot.slotId));

                var resultContent = Task.Run(async () => await task.Result.Content.ReadAsStringAsync());

                hotbarSlot = JsonConvert.DeserializeObject<FFXIVHotbarSlot>(resultContent.Result);

                // The iconData need to be split because having `data:image/png;base64,` in it is not allowed
                return BitmapImage.FromArray(Convert.FromBase64String(hotbarSlot.iconData.Split(',')[1]));
            }

            return base.GetCommandImage(actionParameter, imageSize);   
        }

        /**
         * Converts an actionParameter formatted like `hotbarId:slotId` into a `FFXIVHotbarSlot`
         */
        private FFXIVHotbarSlot ActionParameterToHotbarSlot(String actionParameter)
        {
            var paramArray = actionParameter.Split(':');

            return new FFXIVHotbarSlot(Int32.Parse(paramArray[0]), Int32.Parse(paramArray[1]));
        }

        protected override Boolean OnUnload() {
            this.isApplicationReadySubscription.Dispose();

            return base.OnUnload();
        }
    }

}
