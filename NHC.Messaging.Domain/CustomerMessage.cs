

namespace NHC.Messaging.Domain
{
    public class CustomerMessage
    {
        public int CustomerId { get; set; }
        public int MessageId { get; set; }
        public bool Status { get; set; }
    }
}
