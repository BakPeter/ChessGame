using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameLogic
{
    public class ExceptionSquareCoordinate : Exception
    {
        public ExceptionSquareCoordinate(string msg):base(msg)
        {

        }
    }
}
