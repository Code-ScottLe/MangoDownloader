using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangoEngine.Exceptions
{
    public class MangoException : Exception
    {
        /*Represent the exception of the engine */

        #region Constructor
        /*Constructors*/
        public MangoException()
            : base()
        {

        }

        public MangoException(string message)
            : base(message)
        {

        }

        public MangoException(string message, Exception inner)
            : base(message, inner)
        {

        }
        #endregion
    }
}
