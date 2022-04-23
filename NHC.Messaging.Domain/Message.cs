using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHC.Messaging.Domain
{
    public class Message:Entity
    {
        public string Header { get; set; }
        public string MessageContent { get; set; }
        public virtual IList<CustomerMessage> CustomerMessages { get; set; } = new List<CustomerMessage>();

    }
}
