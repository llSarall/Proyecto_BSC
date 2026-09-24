using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Exceptions
{
    public class ReglaNegocioException : Exception
    {
        public ReglaNegocioException(string message) : base(message)
        {
        }
    }
}
