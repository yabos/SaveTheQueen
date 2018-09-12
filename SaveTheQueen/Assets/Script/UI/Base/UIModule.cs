namespace Lib.uGui
{
    public interface IUIAction
    {
        void MessageCallback(string key, params object[] args);
        void Clear();
    }

    public interface IUIModule
    {
        void OnEnterModule();

        void OnExitModule();

        void OnRefreshModule();

        void OnDestroyModule();
    }

}