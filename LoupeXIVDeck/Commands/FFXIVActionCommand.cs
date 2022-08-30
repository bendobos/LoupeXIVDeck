namespace Loupedeck.LoupeXIVDeckPlugin.commands
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using static Loupedeck.LoupeXIVDeckPlugin.FFXIVGameTypes;

    public class FFXIVActionCommand : PluginDynamicCommand
    {
        private IFFXIVPluginLink _pluginLink;
        private IFFXIVApi _api;
        private IDisposable isApplicationReadySubscription;
        private Boolean isApplicationReady;

        public FFXIVActionCommand() : base("Execute Action", "Execute Action", "FFXIV Commands")
        {
            this.MakeProfileAction("tree");
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

        protected override PluginProfileActionData GetProfileActionData() {
            var actions = Task.Run(async () => await this._api.GetActions());
            var tree = new PluginProfileActionTree("Select Action:");

            var actionsObj = JsonHelpers.DeserializeAnyObject<Dictionary<String, List<FFXIVAction>>>(actions.Result);

            tree.AddLevel("Action Type");
            tree.AddLevel("Action");

            foreach (var actionType in actionsObj)
            {
                var node = tree.Root.AddNode(actionType.Key);

                foreach (var action in actionType.Value)
                {
                    node.AddItem($"{action.type}:{action.id}", action.name);
                }
            }

            return tree;
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            if (actionParameter != null && this.isApplicationReady)
            {
                var action = this.ActionParameterToFFXIVAction(actionParameter);

                var result = Task.Run(async () => await this._api.GetAction(action.type, action.id)).Result;

                var icon = Task.Run(async () => await this._api.GetIcon(result.iconId));

                return BitmapImage.FromArray(icon.Result);
            }

            return base.GetCommandImage(actionParameter, imageSize);
        }

        protected override async void RunCommand(String actionParameter)
        {
            if (this.isApplicationReady)
            {
                var action = this.ActionParameterToFFXIVAction(actionParameter);

                await this._api.ExecuteAction(action.type, action.id);
            }
        }

        /**
         * Converts an actionParameter formatted like `ActionType:ActionId` into a `FFXIVHotbarSlot`
         */
        private FFXIVAction ActionParameterToFFXIVAction(String actionParameter)
        {
            System.Diagnostics.Debug.WriteLine(actionParameter);

            var paramArray = actionParameter.Split(':');

            return new FFXIVAction(paramArray[0], Int32.Parse(paramArray[1]));
        }

        protected override Boolean OnUnload() {
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
