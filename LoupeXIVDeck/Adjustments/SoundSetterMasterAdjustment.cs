namespace Loupedeck.LoupeXIVDeckPlugin.Adjustments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class SoundSetterMasterAdjustment : PluginDynamicAdjustment
    {
        private IFFXIVPluginLink _pluginLink;
        private IFFXIVApi _api;
        private IDisposable isApplicationReadySubscription;
        private Boolean isApplicationReady;

        public SoundSetterMasterAdjustment() : base("Adjust Volume Channel", "Adjust Volume Channel", "Sound Setter Adjustments", false)
        {
            foreach (var channel in Constants.SOUNDSETTER_DICT)
            {
                this.AddParameter(channel.Value, channel.Value, "Channel Adjustments", this.GroupName);
            }

            this.MakeProfileAction("list");
        }

        protected override Boolean OnLoad()
        {
            this.GetInstances();

            this.isApplicationReadySubscription = this._pluginLink.GetIsApplicationReadySubject()
                .Subscribe(ready => this.isApplicationReady = ready);

            return true;
        }

        protected override void ApplyAdjustment(String actionParameter, Int32 diff)
        {
            if (this.isApplicationReady)
            {
                var command = this.GetSoundSetterCommand(actionParameter);

                var result = Task.Run(async () => await this._api.RunTextCommand($"/{command} {(diff > 0 ? "+" : "")}{diff}"));
            }
        }

        protected override Boolean OnUnload()
        {
            this.isApplicationReadySubscription.Dispose();

            return base.OnUnload();
        }

        private String GetSoundSetterCommand(String actionParameter)
        {
            foreach (var channel in Constants.SOUNDSETTER_DICT)
            {
                if (channel.Value == actionParameter)
                {
                    return channel.Key;
                }
            }

            return actionParameter.Replace(" ", "");
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
