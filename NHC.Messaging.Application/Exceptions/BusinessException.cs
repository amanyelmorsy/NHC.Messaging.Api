using System.Net;

namespace NHC.Messaging.Application.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException(int? errorCode = (int)HttpStatusCode.BadRequest, string message = null) : base(message)
        {
            ErrorCode = errorCode;
        }

        public int? ErrorCode { get; set; }
    }
}
