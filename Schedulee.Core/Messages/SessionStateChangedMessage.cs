using Schedulee.Core.Managers;

namespace Schedulee.Core.Messages
{
    public class SessionStateChangedMessage
    {
        public SessionStateChangedMessage(SessionState state)
        {
            State = state;
        }

        public SessionState State { get; set; }
    }
}
