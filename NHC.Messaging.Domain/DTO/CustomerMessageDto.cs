

namespace NHC.Messaging.Domain.DTO
{
    public class CustomerMessageDto
    {
        public int CustomerId { get; set; }
        public string CustomerPhone { get; set; }
        public int MessageId { get; set; }
        public CustomerDto CustomerDto { get; set; }  

    }
}
