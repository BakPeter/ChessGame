using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameLogic
{
    public class Piece
    {
        private PieceTypes _pieceType;
        private ChessGameEntityColor _pieceColor;
        private FirstMoveIndicator _IndicatorFirstMovefirstMove;
        private Exception _inValidPieceException;



        public PieceTypes PieceType
        {
            get
            {
                return this._pieceType;
            }
            internal set
            {
                this._pieceType = value;
            }
        }

        public ChessGameEntityColor PieceColor
        {
            get
            {
                return this._pieceColor;
            }
            private set
            {
                this._pieceColor = value;
            }
        }
        private void setPieceColor()
        {
            if (this._pieceType == PieceTypes.WHITE_KING ||
                   this._pieceType == PieceTypes.WHITE_QUEEN ||
                   this._pieceType == PieceTypes.WHITE_ROOK ||
                   this._pieceType == PieceTypes.WHITE_BISHOP ||
                   this._pieceType == PieceTypes.WHITE_KNIGHT ||
                   this._pieceType == PieceTypes.WHITE_POWN)
                PieceColor = ChessGameEntityColor.WHITE;
            else
                PieceColor = ChessGameEntityColor.BLACK;
        }

        internal FirstMoveIndicator IndicatorFirstMove
        {
            get
            {
                return this._IndicatorFirstMovefirstMove;
            }
            private set
            {
                this._IndicatorFirstMovefirstMove = value;
            }
        }
        private void setFirstMoveMadeFlag()
        {
            if (pieceHasSpecialFirstMoveIndicator())
                IndicatorFirstMove = FirstMoveIndicator.NOT_MOVED;
            else
                IndicatorFirstMove = FirstMoveIndicator.NO_FIRST_MOVE_FOR_THIS_TYPE_OF_PIECE;
        }
        internal bool pieceHasSpecialFirstMoveIndicator()
        {
            PieceTypes pieceType = PieceType;
            if (pieceType == PieceTypes.WHITE_KING ||
                     pieceType == PieceTypes.WHITE_ROOK ||
                     pieceType == PieceTypes.WHITE_POWN ||
                     pieceType == PieceTypes.BLACK_KING ||
                     pieceType == PieceTypes.BLACK_ROOK ||
                     pieceType == PieceTypes.BLACK_POWN)
                return true;
            else
                return false;
        }

        internal Exception InvaildPieceException
        {
            get
            {
                return this._inValidPieceException;
            }
            private set
            {
                this._inValidPieceException = value;
            }
        }
        internal bool PieceValid
        {
            get
            {
                return this._inValidPieceException == null ? true : false;
            }
        }

        public Piece(PieceTypes pieceType)
        {
            PieceType = pieceType;
            setPieceColor();
            setFirstMoveMadeFlag();
        }

        public Piece(Piece obj)
        {
            if (obj != null)
            {
                this._pieceType = obj.PieceType;
                this._pieceColor = obj.PieceColor;
                this._IndicatorFirstMovefirstMove = obj._IndicatorFirstMovefirstMove;
            }
            else
            {
                this._inValidPieceException = new ArgumentNullException("Param obj for Copy C'tor, of Piece calss, is null");
            }
        }

        public override string ToString()
        {
            return string.Format("_pieceType = {0}, _pieceColor = {1}, _firstMoveMadeFlag = {2}, PieceValid property = {3} _inValidPieceException = {4} ",
                                        PieceType, PieceColor, IndicatorFirstMove, PieceValid, InvaildPieceException == null ? "null" : _inValidPieceException.Message);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Piece other = obj as Piece;
            if (other == null)
                return false;

            return this.PieceType == other.PieceType &&
                   this.PieceColor == other.PieceColor &&
                   this.IndicatorFirstMove == other.IndicatorFirstMove &&
                   this._inValidPieceException.Equals(other._inValidPieceException);
        }

        public override int GetHashCode()
        {
            return this._pieceType.GetHashCode() ^ this._pieceColor.GetHashCode() ^ this._IndicatorFirstMovefirstMove.GetHashCode();
        }

        public void PieceMovedForTheFirstTime()
        {
            if (pieceHasSpecialFirstMoveIndicator())
                IndicatorFirstMove = FirstMoveIndicator.MOVED;
        }
    }

    internal enum FirstMoveIndicator
    {
        NO_FIRST_MOVE_FOR_THIS_TYPE_OF_PIECE,
        NOT_MOVED,
        MOVED
    }

    public enum PieceTypes
    {
        NOUN,

        WHITE_KING,
        WHITE_QUEEN,
        WHITE_ROOK,
        WHITE_BISHOP,
        WHITE_KNIGHT,
        WHITE_POWN,

        BLACK_KING,
        BLACK_QUEEN,
        BLACK_ROOK,
        BLACK_BISHOP,
        BLACK_KNIGHT,
        BLACK_POWN
    }
}
