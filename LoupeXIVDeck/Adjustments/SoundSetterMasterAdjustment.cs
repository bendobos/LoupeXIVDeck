namespace Loupedeck.LoupeXIVDeckPlugin.Adjustments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class SoundSetterMasterAdjustment : PluginDynamicAdjustment
    {
        private readonly Dictionary<String, String> channels = new Dictionary<String, String>() {
            { "ssmv", "Master Volume" },
            { "ssbgm", "BGM Volume" },
            { "sssfx", "SFX Volume" },
            { "ssv", "Voice Volume" },
            { "sssys", "System Volume" },
            { "ssas", "Ambient Sound Volume" },
            { "ssp", "Performance Volume" }
        };
        private readonly IDisposable isApplicationReadySubscription;
        private Boolean isApplicationReady;

        public SoundSetterMasterAdjustment() : base("Adjust Volume Channel", "Adjust Volume Channel", "Sound Setter Adjustments", false)
        {
            this.isApplicationReadySubscription = LoupeXIVDeckPlugin.pluginLink.isApplicationReadySubject
                .Subscribe(ready => this.isApplicationReady = ready);

            foreach (var channel in this.channels)
            {
                this.AddParameter(channel.Value, channel.Value, "Channel Adjustments", this.GroupName);
            }

            this.MakeProfileAction("list");
        }

        protected override void ApplyAdjustment(String actionParameter, Int32 diff)
        {
            if (this.isApplicationReady)
            {
                var command = this.GetSoundSetterCommand(actionParameter);

                var result = Task.Run(async () => await LoupeXIVDeckPlugin.api.RunTextCommand($"/{command} {(diff > 0 ? "+" : "")}{diff}"));
            }
        }

        protected override Boolean OnUnload()
        {
            this.isApplicationReadySubscription.Dispose();

            return base.OnUnload();
        }

        private String GetSoundSetterCommand(String actionParameter)
        {
            foreach (var channel in this.channels)
            {
                if (channel.Value == actionParameter)
                {
                    return channel.Key;
                }
            }

            return actionParameter.Replace(" ", "");
        }
    }
}
