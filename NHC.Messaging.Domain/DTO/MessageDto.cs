using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHC.Messaging.Domain.DTO
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string MessageContent { get; set; }
        public virtual IList<CustomerDto> Customers { get; set; } = new List<CustomerDto>();
        public virtual IList<CustomerMessageDto> CustomerMessages { get; set; } = new List<CustomerMessageDto>();

    }
}
