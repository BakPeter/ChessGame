using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameLogic
{
    public class Square
    {
        private Piece _pieceOnASquare;
        private int _pieceOnASquareCounter;
        private Exception _squareValidationException;

        public Piece PieceOnASquare
        {
            get
            {
                return this._pieceOnASquare;
            }
            internal set
            {
                this._pieceOnASquare = value;
            }
        }

        internal int PieceOnASqaureCounter
        {
            get
            {
                return this._pieceOnASquareCounter;
            }

            private set
            {
                this._pieceOnASquareCounter = value;
            }
        }
        internal void PieceStaiedOnASquareForTurn()
        {
            if (PieceOnASquare != null)
                PieceOnASqaureCounter++;
        }
        internal void PieceMovedFromSquare()
        {
            PieceOnASqaureCounter = 0;
        }

        internal bool ValidSquare
        {
            get
            {
                return this._squareValidationException == null ? true : false;
            }
        }
        internal Exception SquareValidationException
        {
            get
            {
                return this._squareValidationException;
            }
            private set
            {
                this._squareValidationException = value;
            }
        }

        internal Square()
        {
            PieceOnASquare = null;
            PieceOnASqaureCounter = 0;
        }

        internal Square(Piece pieceOnASquare)
        {
            PieceOnASquare = pieceOnASquare;
            PieceOnASqaureCounter = 0;
        }

        internal Square(Square other)
        {
            if(other == null)
            {
                this._squareValidationException = new ArgumentNullException("Param obj for Copy C'tor, of Square class, is null");
            }
            else
            {
                if (other._pieceOnASquare != null)
                    this._pieceOnASquare = new Piece(other._pieceOnASquare);
                else
                    this._pieceOnASquare = null;

                this._pieceOnASquareCounter = other._pieceOnASquareCounter;

                if (other._squareValidationException == null)
                    this._squareValidationException = null;
                else
                    this._squareValidationException = new Exception("Param for Square Copy C'tor is in valid");
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Square other = obj as Square;

            if (other == null)
                return false;

            if (this._pieceOnASquareCounter != other._pieceOnASquareCounter)
                return false;

            if (!this._pieceOnASquare.Equals(other._pieceOnASquare))
                return false;

            if (!this._squareValidationException.Equals(other._squareValidationException))
                return false;


            return true;
        }

        public override string ToString()
        {
            return string.Format("<_pieceOnASquare = {0}>, _pieceOnASquareCounter = {1},  <_squareValidationException = {2}>",
                PieceOnASquare==null?"null": PieceOnASquare.ToString(), PieceOnASqaureCounter,  SquareValidationException==null?"null":_squareValidationException.Message);
        }
    }
}
