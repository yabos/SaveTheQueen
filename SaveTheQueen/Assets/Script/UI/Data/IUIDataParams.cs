using Lib.Event;

namespace Lib.uGui
{
    public interface IUIDataParams
    {
    }

    public class MessageDataParams : IUIDataParams
    {
        public IMessage Message;
    }
}