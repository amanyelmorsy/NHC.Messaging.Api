using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NHC.Messaging.Application.Exceptions
{
    public class EntityNotFoundException : BusinessException
    {
        public EntityNotFoundException(string message = null) : base((int)HttpStatusCode.NotFound, message)
        {
        }
    }
}
