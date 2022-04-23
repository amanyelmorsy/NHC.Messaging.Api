using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
