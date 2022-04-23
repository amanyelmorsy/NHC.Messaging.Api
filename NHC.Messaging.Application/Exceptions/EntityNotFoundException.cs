using System.Net;

namespace NHC.Messaging.Application.Exceptions
{
    public class EntityNotFoundException : BusinessException
    {
        public EntityNotFoundException(string message = null) : base((int)HttpStatusCode.NotFound, message)
        {
        }
    }
}
