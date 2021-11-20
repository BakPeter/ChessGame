using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameLogic
{
    public class ExceptionChessGameLogic : Exception
    {
        public ExceptionChessGameLogic(string msg) : base(msg)
        {

        }
    }
}
