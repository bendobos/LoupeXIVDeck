namespace Loupedeck.LoupeXIVDeckPlugin
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IFFXIVApi
    {
        Task<System.Boolean> ExecuteAction(System.String type, System.Int32 id);
        Task<FFXIVGameTypes.FFXIVAction> GetAction(System.String type, System.Int32 id);
        Task<System.Collections.Generic.Dictionary<System.String, System.Collections.Generic.List<FFXIVGameTypes.FFXIVAction>>> GetActions();
        Task<System.Collections.Generic.List<FFXIVGameTypes.FFXIVClass>> GetClasses(System.Boolean unlocked = false);
        Task<FFXIVGameTypes.FFXIVClass> GetClass(System.Int32 classId);
        Task<HttpResponseMessage> GetHotbarSlot(System.Int32 hotbarId, System.Int32 slotId);
        Task<System.Byte[]> GetIcon(System.Int32 iconId, System.Boolean hq = false);
        Task<System.Boolean> RunTextCommand(System.String command);
        Task<System.Boolean> TriggerClass(System.Int32 classId);
        Task<System.Boolean> TriggerHotbarSlot(System.Int32 hotbarId, System.Int32 slotId);
    }
}