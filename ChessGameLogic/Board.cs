using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameLogic
{
    public class Board
    {
        private Square[,] _squares;

        public Square[,] Squares
        {
            get
            {
                return this._squares;
            }
            private set
            {
                this._squares = value;
            }
        }

        public Board()
        {
            initBoard();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Board other = obj as Board;

            if (other == null)
                return false;
           
            for (int row = 0; row < this._squares.GetLength(0); row++)
            {
                for (int col = 0; col < this._squares.GetLength(1); col++)
                {
                    if (!this._squares[row, col].Equals(other._squares[row, col]))
                        return false;
                }
            }

            return true;
        }

        internal void makeMove(Move move)
        {
            int fromRowInd = ChessGameUtility.GetRowIndex(move.FromSquare[1]);
            int fromColumnInd = ChessGameUtility.GetColumnIndex(move.FromSquare[0]);
            int toRowInd = ChessGameUtility.GetRowIndex(move.ToSquare[1]);
            int toColumnInd = ChessGameUtility.GetColumnIndex(move.ToSquare[0]);

            if (!makeMoveIfCasteling(fromRowInd, fromColumnInd, toRowInd, toColumnInd))
            {
                if (!makeMoveIfEnPassant(fromRowInd, fromColumnInd, toRowInd, toColumnInd))
                {
                    this._squares[toRowInd, toColumnInd].PieceOnASquare = this._squares[fromRowInd, fromColumnInd].PieceOnASquare;
                    this._squares[fromRowInd, fromColumnInd].PieceOnASquare = null;

                    if (move.TypeOfTheMove == MoveType.POWN_PROMOTION)
                    {
                        this._squares[toRowInd, toColumnInd].PieceOnASquare.PieceType = move.PownPromotionType;
                    }

                }
            }
           
            updatePieceOnAsquareCounter();
            this._squares[toRowInd, toColumnInd].PieceMovedFromSquare();
            this._squares[fromRowInd, fromColumnInd].PieceMovedFromSquare();

            this._squares[toRowInd, toColumnInd].PieceOnASquare.PieceMovedForTheFirstTime();

        }

    

        private bool makeMoveIfEnPassant(int fromRowInd, int fromColumnInd, int toRowInd, int toColumnInd)
        {
            bool retVal = false;
            Piece piece = this._squares[fromRowInd, fromColumnInd].PieceOnASquare;
            ChessGameEntityColor currPalyerColor = piece.PieceColor;

            if ((fromRowInd == 4 && toRowInd == 5) || (fromRowInd == 3 && toRowInd == 2))
            {
                if (piece != null)
                {
                    if (piece.PieceType == PieceTypes.WHITE_POWN)
                    {
                        if (fromRowInd + 1 == toRowInd)
                        {
                            if (fromColumnInd == (toColumnInd + 1))
                            {
                                retVal = true;
                            }
                            else if (fromColumnInd == (toColumnInd - 1))
                            {
                                retVal = true;
                            }
                        }
                    }
                    else if (piece.PieceType == PieceTypes.BLACK_POWN)
                    {
                        if (fromRowInd - 1 == toRowInd)
                        {
                            if (fromColumnInd == (toColumnInd + 1))
                            {
                                retVal = true;
                            }
                            else if (fromColumnInd == (toColumnInd - 1))
                            {
                                retVal = true;
                            }
                        }
                    }
                }
            }
            else
            {
                retVal = false;
            }


            if (retVal)
            {
                piece = this._squares[fromRowInd, toColumnInd].PieceOnASquare;
                if (piece != null)
                {
                    if (piece.PieceColor == currPalyerColor)
                    {
                        retVal = false;
                    }
                    else
                    {
                        this._squares[fromRowInd, toColumnInd].PieceOnASquare = null;
                        this._squares[toRowInd, toColumnInd].PieceOnASquare = this._squares[fromRowInd, fromColumnInd].PieceOnASquare;
                        this._squares[fromRowInd, fromColumnInd].PieceOnASquare = null;
                    }
                }
                else
                {
                    retVal = false;
                }
            }

            return retVal;
        }

        internal void getKingSquareIndex(ChessGameEntityColor playerColor, ref int playerKingRow, ref int playerKingColumn)
        {
            Piece piece;
            for (int i=0;i<this._squares.GetLength(0); i++)
            {
                for(int j=0; j<this._squares.GetLength(1); j++)
                {
                    piece = this._squares[i, j].PieceOnASquare;
                    if (piece!=null)
                    {
                        switch(playerColor)
                        {
                            case ChessGameEntityColor.WHITE:
                                if(piece.PieceType == PieceTypes.WHITE_KING)
                                {
                                    playerKingRow = i;
                                    playerKingColumn = j;
                                }
                                break;
                            case ChessGameEntityColor.BLACK:
                                if (piece.PieceType == PieceTypes.BLACK_KING)
                                {
                                    playerKingRow = i;
                                    playerKingColumn = j;
                                }
                                break;
                        }
                    }
                }
            }
        }

        private bool makeMoveIfCasteling(int fromRowInd, int fromColumnInd, int toRowInd, int toColumnInd)
        {
            bool retVal = false;

            //white 
            if (fromRowInd == 0 && fromColumnInd == 4)
            {
                if (toRowInd == 0 && toColumnInd == 6)
                {//e1 to g1
                    //f1
                    this._squares[0, 5].PieceOnASquare = this._squares[0, 7].PieceOnASquare;
                    this._squares[0, 7].PieceOnASquare = null;
                    retVal = true;
                }
                else if (toRowInd == 0 && toColumnInd == 2)
                {//e1 to c1
                    //d1
                    this._squares[0, 3].PieceOnASquare = this._squares[0, 0].PieceOnASquare;
                    this._squares[0, 0].PieceOnASquare = null;
                    retVal = true;
                }
            }

            //black
            if (fromRowInd == 7 && fromColumnInd == 4)
            {
                if (toRowInd == 7 && toColumnInd == 6)
                {//e8 to g8
                    //f1
                    this._squares[7, 5].PieceOnASquare = this._squares[7, 7].PieceOnASquare;
                    this._squares[7, 7].PieceOnASquare = null;
                    retVal = true;
                }
                else if (toRowInd == 7 && toColumnInd == 2)
                {//e8 to c8
                    //d1
                    this._squares[7, 3].PieceOnASquare = this._squares[7, 0].PieceOnASquare;
                    this._squares[7, 0].PieceOnASquare = null;
                    retVal = true;
                }
            }

            if (retVal)
            {
                this._squares[toRowInd, toColumnInd].PieceOnASquare = this._squares[fromRowInd, fromColumnInd].PieceOnASquare;
                this._squares[fromRowInd, fromColumnInd].PieceOnASquare = null;
            }
           

            return retVal;
        }

        internal List<int> getSquareCoordinatesOfAllPieceOfAColor(ChessGameEntityColor playerColor)
        {
            List<int> retVal = new List<int>();

            for(int i=0; i<this._squares.GetLength(0); i++)
            {
                for(int j=0; j<this._squares.GetLength(1); j++)
                {
                    if(this._squares[i,j].PieceOnASquare != null)
                    {
                        if(this._squares[i, j].PieceOnASquare.PieceColor ==playerColor)
                        {
                            retVal.Add(i);
                            retVal.Add(j);
                        }
                    }
                }
            }

            return retVal;
        }

        internal bool isPieceOnAsquareBishopOrAKnigth(int row, int column)
        {
            Piece piece = this._squares[row, column].PieceOnASquare;

            if (piece == null)
                return false;

            if (piece.PieceType == PieceTypes.BLACK_BISHOP || piece.PieceType == PieceTypes.WHITE_BISHOP ||
                piece.PieceType == PieceTypes.BLACK_KNIGHT || piece.PieceType == PieceTypes.WHITE_KNIGHT)
                return true;
            else
                return false;
        }

        internal bool squaresAreOfTheSameColor(int rowInd1, int colInd1, int rowInd2, int colInd2)
        {
            ChessGameEntityColor firstColor = rowInd1 % 2 != colInd1 % 2 ? ChessGameEntityColor.WHITE : ChessGameEntityColor.BLACK;
            ChessGameEntityColor secondColor = rowInd2 % 2 != colInd2 % 2 ? ChessGameEntityColor.WHITE : ChessGameEntityColor.BLACK;

            return firstColor == secondColor ? true : false;
        }

        private void updatePieceOnAsquareCounter()
        {
            for(int i=0; i<this._squares.GetLength(0); i++)
            {
                for(int j=0; j<this._squares.GetLength(1); j++)
                {
                    this._squares[i, j].PieceStaiedOnASquareForTurn();
                }
            }
        }

        public Board(Board other)
        {
            if (other == null)
                Squares = null;
            else
            {
                Squares = new Square[8, 8];

                for (int i = 0; i < this._squares.GetLength(0); i++)
                {
                    for (int j = 0; j < this._squares.GetLength(1); j++)
                    {
                        this._squares[i, j] = new Square(other._squares[i, j]);
                    }
                }
            }


            
        }

        private void initBoard()
        {
            this._squares = new Square[8, 8];

            for(int i=0; i<8; i++)
            {
                for( int j=0; j<8; j++)
                {
                    this._squares[i, j] = new Square();

                    if (i == 7)
                    {
                        if (j == 0 || j == 7)
                            this._squares[i, j].PieceOnASquare = new Piece(PieceTypes.BLACK_ROOK);

                        if (j == 1 || j == 6)
                            this._squares[i, j].PieceOnASquare = new Piece(PieceTypes.BLACK_KNIGHT);

                        if (j == 2 || j == 5)
                            this._squares[i, j].PieceOnASquare = new Piece(PieceTypes.BLACK_BISHOP);

                        if (j == 3)
                            this._squares[i, j].PieceOnASquare = new Piece(PieceTypes.BLACK_QUEEN);

                        if (j == 4)
                            this._squares[i, j].PieceOnASquare = new Piece(PieceTypes.BLACK_KING);
                    }

                    if (i == 1)
                        this._squares[i, j].PieceOnASquare = new Piece(PieceTypes.WHITE_POWN);

                    if (i == 6)
                        this._squares[i, j].PieceOnASquare = new Piece(PieceTypes.BLACK_POWN);

                    if (i == 0)
                    {
                        if (j == 0 || j == 7)
                            this._squares[i, j].PieceOnASquare = new Piece(PieceTypes.WHITE_ROOK);

                        if (j == 1 || j == 6)
                            this._squares[i, j].PieceOnASquare = new Piece(PieceTypes.WHITE_KNIGHT);

                        if (j == 2 || j == 5)
                            this._squares[i, j].PieceOnASquare = new Piece(PieceTypes.WHITE_BISHOP);

                        if (j == 3)
                            this._squares[i, j].PieceOnASquare = new Piece(PieceTypes.WHITE_QUEEN);

                        if (j == 4)
                            this._squares[i, j].PieceOnASquare = new Piece(PieceTypes.WHITE_KING);
                    }
                }
            }
        }

        internal Piece getPieceBySquareCoordinate(int row, int column)
        {
            return getPieceBySquareCoordinateIndexes(row - 1, column - 1);
        }

        internal Piece getPieceBySquareCoordinateIndexes(int rowIndex, int columnIndex)
        {
            return this._squares[rowIndex, columnIndex].PieceOnASquare;
        }
        
        internal Square getSquareBySquareCoordinateIndexes(int rowIndex, int columnIndex)
        {
            return this._squares[rowIndex, columnIndex];
        }
    }
}
