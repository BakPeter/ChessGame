using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameLogic
{
    internal class SquareCoordinate:IComparable<SquareCoordinate>
    {
        public const int ROW_COLUMN_MIN_VALUE = 1;
        public const int ROW_COLUMN_NAX_VALUE = 8;

        private int _row;
        private int _column;
        private ExceptionSquareCoordinate _inValidSquareCoordinateException = null;

        public SquareCoordinate(int row, int column)
        {
            Row = row;
            Column = column;
        }
        public SquareCoordinate(int row, char column)
        {
            Row = row;
            ColumnAsChar = column;
        }
        public SquareCoordinate(SquareCoordinate other)
        {
            if(other == null)
            {
                InValidSquareCoordinateException = new ExceptionSquareCoordinate("Param for copy C'tor is null");
            }
            else
            {
                Row = other.Row;
                Column = other.Column;
                InValidSquareCoordinateException = other.InValidSquareCoordinateException;
            }
        }

        public bool ValidSquareCoordinates
        {
            get
            {
                return this._inValidSquareCoordinateException == null ? true : false;
            }
        }
        public ExceptionSquareCoordinate InValidSquareCoordinateException
        {
            get
            {
                return this._inValidSquareCoordinateException;
            }
            private set
            {
                this._inValidSquareCoordinateException = value;
            }
        }
        public int Column
        {
            get
            {
                return this._column;
            }
            private set
            {
                this._column = value;

                if (value < 1)
                    if (this._inValidSquareCoordinateException == null)
                        InValidSquareCoordinateException = new ExceptionSquareCoordinate(string.Format("Param for Column Property is {0}, min value for Column is {1}", value, ROW_COLUMN_MIN_VALUE));
                    else
                        InValidSquareCoordinateException = new ExceptionSquareCoordinate(this._inValidSquareCoordinateException.Message + "\n" + string.Format("Param for Column Property is {0}, min value for Column is {1}", value, ROW_COLUMN_MIN_VALUE));
                else if (value > 8)
                    if (this._inValidSquareCoordinateException == null)
                        InValidSquareCoordinateException = new ExceptionSquareCoordinate(string.Format("Param for Column Property is {0}, mmax value for Column is {1}", value, ROW_COLUMN_NAX_VALUE));
                    else
                        InValidSquareCoordinateException = new ExceptionSquareCoordinate(this._inValidSquareCoordinateException.Message + "\n" + string.Format("Param for Column Property is {0}, mmax value for Column is {1}", value, ROW_COLUMN_NAX_VALUE));
            }
        }
        public char ColumnAsChar
        {
            get
            {
                return (char)('a' + this._column - 1);
            }
            private set
            {
                int columnAsInt = getColumnFromChar(value);
                Column = columnAsInt;
            }
        }
        private int getColumnFromChar(char column)
        {
            return column - 'a' + 1;
        }
        public int Row
        {
            get
            {
                return this._row;
            }
            private set
            {
                this._row = value;

                if (value < 1)
                    if (this._inValidSquareCoordinateException == null)
                        InValidSquareCoordinateException = new ExceptionSquareCoordinate(string.Format("Param for Row Property is {0}, min value for row is {1}", value, ROW_COLUMN_MIN_VALUE));
                    else
                        InValidSquareCoordinateException = new ExceptionSquareCoordinate(this._inValidSquareCoordinateException.Message + "\n" + string.Format("Param for Row Property is {0}, min value for row is {1}", value, ROW_COLUMN_MIN_VALUE));
                else if (value > 8)
                    if (this._inValidSquareCoordinateException == null)
                        InValidSquareCoordinateException = new ExceptionSquareCoordinate(string.Format("Param for Row Property is {0}, max value for row is {1}", value, ROW_COLUMN_NAX_VALUE));
                    else
                        InValidSquareCoordinateException = new ExceptionSquareCoordinate(this._inValidSquareCoordinateException.Message + "\n" + string.Format("Param for Row Property is {0}, max value for row is {1}", value, ROW_COLUMN_NAX_VALUE));
            }
        }

        public override int GetHashCode()
        {
            return this._row ^ this._column;
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("Param for Equale is null");

            SquareCoordinate other = obj as SquareCoordinate;
            if (other == null)
                throw new ArgumentException("Param for Equales is not SquareCoordinate object");

           return  (this._row == other._row) && (this._column == other._column);
        }
        public override string ToString()
        {
            return string.Format("{0}{1}, Exception : {2}", Row, ColumnAsChar, InValidSquareCoordinateException==null?"null":this._inValidSquareCoordinateException.Message);
        }

        public int CompareTo(SquareCoordinate other)
        {
            if (other == null)
                return 1;

            int retVal = this._row - other.Row;
            if(retVal == 0)
            {
                retVal = this._column - other._column;
            }

            return retVal;
        }
    }
}
