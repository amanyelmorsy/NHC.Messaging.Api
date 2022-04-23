using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHC.Messaging.Domain
{
    public class Customer: Entity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public virtual IList<CustomerMessage> CustomerMessages { get; set; } = new List<CustomerMessage>();
    }
}
