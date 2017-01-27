using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSnake.Util
{
    public class TooLongMessageException : Exception
    {
        public TooLongMessageException(string message)
            : base(string.Format("メッセージが長すぎます: \"{0}\"", message))
        {
        }
    }
}
