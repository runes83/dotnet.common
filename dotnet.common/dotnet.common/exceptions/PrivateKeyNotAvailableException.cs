using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotnet.common.exceptions
{
    public class PrivateKeyNotAvailableException:Exception
    {
        public PrivateKeyNotAvailableException(string message):base(message)
        {
            
        }
    }
}
