

namespace NHC.Messaging.Domain
{
    public class Message:Entity
    {
        public string Header { get; set; }
        public string MessageContent { get; set; }
        public virtual IList<CustomerMessage> CustomerMessages { get; set; } = new List<CustomerMessage>();

    }
}
