namespace DatingCode.Business.Core
{
    public interface ILocalizer
    {
        string ForObject(int localeId, int objectId);
        string ForString(int localeId, string templateCode);
    }
}
