using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHC.Messaging.Domain
{
    public class CustomerMessage
    {
        public int CustomerId { get; set; }
        public int MessageId { get; set; }
        public bool Status { get; set; }
    }
}
