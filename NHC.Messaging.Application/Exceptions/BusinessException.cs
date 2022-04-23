using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
