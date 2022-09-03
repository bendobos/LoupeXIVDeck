namespace Loupedeck.LoupeXIVDeckPlugin
{
    using System;

    public class FFXIVGameTypes
    {
        public class FFXIVTextCommand
        {
            public String Command { get; set; }
        }

        public class FFXIVHotbarSlot
        {
            public Int32 hotbarId;
            public Int32 slotId;

            public Int32 iconId;
            public String iconData;

            public FFXIVHotbarSlot(Int32 hotbarId, Int32 slotId)
            {
                this.hotbarId = hotbarId;
                this.slotId = slotId;
            }
        }

        public class FFXIVAction
        {
            public String name;
            public Int32 id;
            public String type;
            public String category;
            public Int32 iconId;

            public FFXIVAction(String type, Int32 id)
            {
                this.type = type;
                this.id = id;
            }
        }

        public class FFXIVClass
        {
            public Int32? id;
            public Int32? number;
            public String name;

            public String categoryName;
            public Int32 sortOrder;

            public Int32 iconId;
            public String iconData;

            public Int32 parentClass;
            public Boolean hasGearset;
        }
    }
}
