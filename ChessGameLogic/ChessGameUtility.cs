using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameLogic
{
    public enum ChessGameEntityColor { WHITE=1, BLACK }

    public static class ChessGameUtility
    {
        public static ChessGameEntityColor GetRivalColor(ChessGameEntityColor playerColor)
        {
            return playerColor == ChessGameEntityColor.WHITE ? ChessGameEntityColor.BLACK : ChessGameEntityColor.WHITE;
        }

        public static bool validValueForColumn(char column)
        {
            bool retVal;
            switch (column)
            {
                case 'a':
                case 'b':
                case 'c':
                case 'd':
                case 'e':
                case 'f':
                case 'g':
                case 'h':
                    retVal = true;
                    break;
                default:
                    retVal = false;
                    break;
            }

            return retVal;
        }
        public static bool validValueForRow(char row)
        {
            bool retVal;
            switch (row)
            {
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                    retVal = true;
                    break;
                default:
                    retVal = false;
                    break;
            }

            return retVal;
        }

        public const int I_LEGAL_VALUE_FOR_ROW_COLUMN = -1;
        public static int GetRow(char row)
        {
            int retVal;

            switch (row)
            {
                case '1':
                    retVal = 1;
                    break;
                case '2':
                    retVal = 2;
                    break;
                case '3':
                    retVal = 3;
                    break;
                case '4':
                    retVal = 4;
                    break;
                case '5':
                    retVal = 5;
                    break;
                case '6':
                    retVal = 6;
                    break;
                case '7':
                    retVal = 7;
                    break;
                case '8':
                    retVal = 8;
                    break;
                default:
                    retVal = I_LEGAL_VALUE_FOR_ROW_COLUMN;
                    break;
            }

            return retVal;
        }
        public static int GetColumn(char column)
        {
            int retVal;

            switch (column)
            {
                case 'a':
                    retVal = 1;
                    break;
                case 'b':
                    retVal = 2;
                    break;
                case 'c':
                    retVal = 3;
                    break;
                case 'd':
                    retVal = 4;
                    break;
                case 'e':
                    retVal = 5;
                    break;
                case 'f':
                    retVal = 6;
                    break;
                case 'g':
                    retVal = 7;
                    break;
                case 'h':
                    retVal = 8;
                    break;
                default:
                    retVal = I_LEGAL_VALUE_FOR_ROW_COLUMN;
                    break;
            }

            return retVal;
        }

        internal static int GetRowIndex(char row)
        {
            return GetRow(row) - 1;
        }
        internal static int GetColumnIndex(char column)
        {
            return GetColumn(column) - 1;
        }

        public static char GetColumnFronInt(int column)
        {
            char retVal;
            switch(column)
            {
                case 1:
                    retVal = 'a';
                    break;
                case 2:
                    retVal = 'b';
                    break;
                case 3:
                    retVal = 'c';
                    break;
                case 4:
                    retVal = 'd';
                    break;
                case 5:
                    retVal = 'e';
                    break;
                case 6:
                    retVal = 'f';
                    break;
                case 7:
                    retVal = 'g';
                    break;
                case 8:
                    retVal = 'h';
                    break;
                default:
                    retVal = 'Z';
                        break;
            }

            return retVal;
        }
    }
}
