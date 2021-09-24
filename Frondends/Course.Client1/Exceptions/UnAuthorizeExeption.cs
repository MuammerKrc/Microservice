using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Course.Client1.Exceptions
{
    public class UnAuthorizeExeption : Exception
    {
        public UnAuthorizeExeption():base()
        {

        }

        public UnAuthorizeExeption(string message) : base(message)
        {

        }

        public UnAuthorizeExeption(string message, Exception innerException) : base(message, innerException)
        {

        }


    }
}
