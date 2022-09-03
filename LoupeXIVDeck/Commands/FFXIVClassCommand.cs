namespace Loupedeck.LoupeXIVDeckPlugin.commands
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using static Loupedeck.LoupeXIVDeckPlugin.FFXIVGameTypes;

    internal class FFXIVClassCommand : PluginDynamicCommand
    {
        private IFFXIVPluginLink _pluginLink;
        private IFFXIVApi _api;
        private IDisposable isApplicationReadySubscription;
        private Boolean isApplicationReady;

        public FFXIVClassCommand() : base("Trigger Class", "Trigger Class", "FFXIV Commands")
        {
            this.MakeProfileAction("tree");
        }

        protected override Boolean OnLoad()
        {
            this.GetInstances();

            this.isApplicationReadySubscription = this._pluginLink.GetIsApplicationReadySubject()
                .Subscribe(ready => {
                    this.isApplicationReady = ready;
                    this.ActionImageChanged();
                });

            return true;
        }

        protected override PluginProfileActionData GetProfileActionData() {
            var classes = Task.Run(async () => await this._api.GetClasses()).Result;
            var tree = new PluginProfileActionTree("Select Class:");

            tree.AddLevel("Category");
            tree.AddLevel("Name");

            var classesDict = new Dictionary<String, List<FFXIVClass>>();
            foreach (var clazz in classes)
            {
                if (!classesDict.ContainsKey(clazz.categoryName))
                {
                    classesDict.Add(clazz.categoryName, new List<FFXIVClass>());
                }
                    
                classesDict[clazz.categoryName].Add(clazz);
            }

            foreach (var classCategory in classesDict)
            {
                var node = tree.Root.AddNode(classCategory.Key);

                foreach (var clazz in classCategory.Value)
                {
                    node.AddItem($"{clazz.id}", $"{clazz.name}{(clazz.hasGearset ? " (Has Gearset)" : "")}");
                }
            }

            return tree;
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            if (actionParameter != null && Int32.Parse(actionParameter) <= 40 && this.isApplicationReady)
            {
                var result = Task.Run(async () => await this._api.GetClass(Int32.Parse(actionParameter))).Result;

                var icon = Task.Run(async () => await this._api.GetIcon(result.iconId));

                return BitmapImage.FromArray(icon.Result);
            }

            return base.GetCommandImage(actionParameter, imageSize);
        }

        protected override async void RunCommand(String actionParameter) 
        {
            if (this.isApplicationReady)
            {
                await this._api.GetClasses();
                await this._api.TriggerClass(Int32.Parse(actionParameter));
            }
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
