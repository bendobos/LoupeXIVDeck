namespace Loupedeck.LoupeXIVDeckPlugin
{
    using System.Reactive.Subjects;

    public interface IFFXIVPluginLink
    {
        void Connect();
        System.String GetApiKey();
        System.String GetBaseUrl();
        Subject<System.Boolean> GetIsApplicationReadySubject();
    }
}