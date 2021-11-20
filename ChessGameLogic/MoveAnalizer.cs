using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameLogic
{
    public class MoveAnalizer
    {
        private Board _borad;
        private Move _move;
        private ChessGameEntityColor _currPlayerColor;
        private GameStatus _currGameStatus;
        private List<Board> _whitePlayerLastThreeMoves;
        private List<Board> _blackPlayerLastThreeMoves;

        private const int LAST_ROW_COLUMN_INDEX = 7;
        private const int FIRST_ROW_COLUMN_INDEX = 0;

        public MoveAnalizer()
        {
            this._whitePlayerLastThreeMoves = new List<Board>();
            this._blackPlayerLastThreeMoves = new List<Board>();
        }

        public MoveResult AnalizeMove(Board board, Move move, ChessGameEntityColor currPlayerColor, GameStatus currGameStatus)
        {
            initMoveAnalizer(board, move, currPlayerColor, currGameStatus);

            MoveResult moveResult = new MoveResult();

            if (this._move.MoveValid)
            {
                if (move.TypeOfTheMove == MoveType.DROW_DU_TO_INSUFFICIENT_MATERIAL)
                {
                    return drowDuToInsufficientMaterial();
                }

                if (move.TypeOfTheMove == MoveType.WIN_ON_TIME)
                {
                    return winOnTime();
                }

                if (move.TypeOfTheMove == MoveType.DRAW_BY_AGREEMENT)
                {
                    return drowByAgrementMove();
                }
                if (move.TypeOfTheMove == MoveType.RESIGNATION)
                {
                    return resignation();
                }
                if (move.TypeOfTheMove == MoveType.FORFEIT)
                {
                    return forfeit();
                }
                if (move.TypeOfTheMove == MoveType.DROW_BY_REPETITION)
                {
                    return repition();
                }

                string fromSquare = move.FromSquare;
                int rowFromInd = ChessGameUtility.GetRowIndex(fromSquare[1]);
                int columnFromInd = ChessGameUtility.GetColumnIndex(fromSquare[0]);
                Piece pieceOnTheFromSquare = this._borad.getPieceBySquareCoordinateIndexes(rowFromInd, columnFromInd);

                if (pieceOnTheFromSquare != null)
                {
                    if (pieceOnTheFromSquare.PieceColor == this._currPlayerColor)
                    {
                        List<int> moves = getValidMovesOfPieceOnASquareBySquareCoordinatesIndexes(rowFromInd, columnFromInd);
                        string toSquare = move.ToSquare;
                        int rowToInd = ChessGameUtility.GetRowIndex(toSquare[1]);
                        int columnToInd = ChessGameUtility.GetColumnIndex(toSquare[0]);

                        if (movesContains(moves, rowToInd, columnToInd))
                        {
                            this._borad.makeMove(this._move);

                            if (playerInCheck(this._currPlayerColor))
                            {
                                if (this._currGameStatus == GameStatus.CHECK)
                                {
                                    return updateMoveResult(ref moveResult, this._currGameStatus, new ExceptionChessGameLogic("Curr player still check"));
                                }
                                else
                                {
                                    return updateMoveResult(ref moveResult, this._currGameStatus, new ExceptionChessGameLogic(string.Format("Curr player in check if move {0} is made", this._move.ToString())));
                                }
                            }
                            else
                            {
                                if (stalmaitPlayerNotInCheck())
                                {
                                    return updateMoveResult(ref moveResult, GameStatus.DROW_STALEMATE, null);
                                }
                            }

                            ChessGameEntityColor rivelColor = ChessGameUtility.GetRivalColor(this._currPlayerColor);
                            if (!playerInCheckMate(rivelColor))
                            {
                                if (playerInCheck(rivelColor))
                                {
                                    return updateMoveResult(ref moveResult, GameStatus.CHECK, null);
                                }
                                else
                                {
                                    addMoveToLastThreeMoves();

                                    return updateMoveResult(ref moveResult, GameStatus.CONTINUE, null);
                                }
                            }
                            else
                            {
                                return updateMoveResult(ref moveResult, GameStatus.CHECK_MATE, null);
                            }
                        }
                        else
                        {
                            moveResult = updateMoveResult(ref moveResult,
                                                this._currGameStatus,
                                                new ExceptionChessGameLogic(string.Format("In valid move, {0}", this._move.ToString())));
                        }
                    }
                    else
                    {
                        moveResult = updateMoveResult(ref moveResult,
                                                 this._currGameStatus,
                                                 new ExceptionChessGameLogic(string.Format("Move {0} is not valid.\nCurr player {1} player.", this._move.ToString(), this._currPlayerColor)));
                    }
                }
                else
                {
                    moveResult = updateMoveResult(ref moveResult,
                                                 this._currGameStatus,
                                                 new ExceptionChessGameLogic(string.Format("Move {0} is not valid.\nNo piece on the first square.", this._move.ToString())));
                }
            }
            else
            {
                moveResult = updateMoveResult(ref moveResult, this._currGameStatus, move.MoveValidationException);
            }

            return moveResult;
        }

        private MoveResult drowDuToInsufficientMaterial()
        {
            bool drowflag = false;
            MoveResult moveResult = new MoveResult();
            List<int> currPlayerPiecesSquareCoordinates = this._borad.getSquareCoordinatesOfAllPieceOfAColor(this._currPlayerColor);
            List<int> rivalPlayerPiecesSquareCoordinates = this._borad.getSquareCoordinatesOfAllPieceOfAColor(ChessGameUtility.GetRivalColor(this._currPlayerColor));
          
            int rivalPlayerMovesCount = rivalPlayerPiecesSquareCoordinates.Count / 2;
            int currPlayerMovesCount = currPlayerPiecesSquareCoordinates.Count / 2;

            if (currPlayerMovesCount <= 2 && rivalPlayerMovesCount <= 2)
            {

                int currPlayerBishopRowInd = 0, currPlayerBishopColInd = 0;
                int rivalPlayerBishopRowInd = 0, rivalPlayerBishopColInd = 0;

                getIndexesOfABishopOrAKnight(currPlayerPiecesSquareCoordinates, ref currPlayerBishopRowInd, ref currPlayerBishopColInd);
                getIndexesOfABishopOrAKnight(rivalPlayerPiecesSquareCoordinates, ref rivalPlayerBishopRowInd, ref rivalPlayerBishopColInd);

                if (currPlayerMovesCount == 2 && rivalPlayerMovesCount == 1)
                {
                    if (currPlayerBishopRowInd != -1 && currPlayerBishopColInd != -1)
                    {
                        drowflag = true;
                    }
                }

                if (currPlayerMovesCount == 1 && rivalPlayerMovesCount == 2)
                {

                    if (rivalPlayerBishopRowInd != -1 && rivalPlayerBishopColInd != -1)
                    {
                        drowflag = true;
                    }
                }


                if (currPlayerMovesCount == 2 && rivalPlayerMovesCount == 2)
                {
                    if(this._borad.squaresAreOfTheSameColor(currPlayerBishopRowInd, currPlayerBishopColInd, rivalPlayerBishopRowInd, rivalPlayerBishopColInd))
                        drowflag = true;
                }
            }

            if (drowflag)
            {
                return updateMoveResult(ref moveResult, GameStatus.DROW_DU_TO_INSUFFICIENT_MATERIAL, null);
            }
            else
            {
                return updateMoveResult(ref moveResult, this._currGameStatus, new ExceptionChessGameLogic(string.Format("{0} is not valid", MoveType.DROW_DU_TO_INSUFFICIENT_MATERIAL)));
            }
        }
        private void getIndexesOfABishopOrAKnight(List<int> piecesIndexes, ref int row, ref int column)
        {

            for (int i = 0; i < piecesIndexes.Count; i += 2)
            {
                if (this._borad.isPieceOnAsquareBishopOrAKnigth(piecesIndexes[i], piecesIndexes[i+1]))
                {
                    row = piecesIndexes[i];
                    column = piecesIndexes[i + 1];

                    return;
                }
            }


            row = -1;
            column = -1;
        }

        private void addMoveToLastThreeMoves()
        {
            if (this._currPlayerColor == ChessGameEntityColor.WHITE)
            {
                if (this._whitePlayerLastThreeMoves.Count == 3)
                {
                    this._whitePlayerLastThreeMoves[0] = this._borad;
                }
                else
                {
                    this._whitePlayerLastThreeMoves.Add(this._borad);
                }
            }
            else
            {
                if (this._blackPlayerLastThreeMoves.Count == 3)
                {
                    this._blackPlayerLastThreeMoves[0] = this._borad;
                }
                else
                {
                    this._blackPlayerLastThreeMoves.Add(this._borad);
                }
            }
        }

        private MoveResult winOnTime()
        {
            MoveResult moveResult = new MoveResult();
            return updateMoveResult(ref moveResult, GameStatus.WIN_ON_TIME, null);
        }

        private MoveResult repition()
        {
            if (this._whitePlayerLastThreeMoves.Count == 3)
            {
                if (this._whitePlayerLastThreeMoves[0].Equals(this._whitePlayerLastThreeMoves[1]) &&
                    this._whitePlayerLastThreeMoves[1].Equals(this._whitePlayerLastThreeMoves[2]))
                    return new MoveResult() { MoveAnalizerResultExeption = null, NewGameStatus = GameStatus.DROW_BY_REPETITION };
            }
            else if (this._blackPlayerLastThreeMoves.Count == 3)
            {
                if (this._blackPlayerLastThreeMoves[0].Equals(this._blackPlayerLastThreeMoves[1]) &&
                    this._blackPlayerLastThreeMoves[1].Equals(this._blackPlayerLastThreeMoves[2]))
                    return new MoveResult() { MoveAnalizerResultExeption = null, NewGameStatus = GameStatus.DROW_BY_REPETITION };
            }

            return new MoveResult() { MoveAnalizerResultExeption = new ExceptionChessGameLogic("Drow to Repition is in valid") };
        }

        private bool stalmaitPlayerNotInCheck()
        {
            List<int> moves = getPlayerAllValidMoves(this._currPlayerColor);
            if (moves.Count == 0)
                return true;
            else
                return false;
        }

        private MoveResult forfeit()
        {
            MoveResult retVal = new MoveResult();
            return updateMoveResult(ref retVal, GameStatus.FORFEIT, null);
        }

        private MoveResult resignation()
        {
            MoveResult retVal = new MoveResult();
            return updateMoveResult(ref retVal, GameStatus.RESIGNATION, null);
        }

        private MoveResult drowByAgrementMove()
        {
            MoveResult result = new MoveResult();
            if (this._currGameStatus == GameStatus.DRAW_BY_AGREEMENT_WAITING_RESPONSE)
            {
                return updateMoveResult(ref result, GameStatus.DRAW_BY_AGREEMENT, null);
            }
            else
            {
                return updateMoveResult(ref result, GameStatus.DRAW_BY_AGREEMENT_WAITING_RESPONSE, null);
            }
        }

        private bool playerInCheckMate(ChessGameEntityColor playerColor)
        {
            int playerKingRow = -1, playerKingColumn = -1;
            this._borad.getKingSquareIndex(playerColor, ref playerKingRow, ref playerKingColumn);
            ChessGameEntityColor rivalColor = ChessGameUtility.GetRivalColor(playerColor);

            List<int> allRivalPiecesThreteningTheKing = getAllRivalPiecesThreteningTheKing(playerKingRow, playerKingColumn, rivalColor);
            if (allRivalPiecesThreteningTheKing.Count == 0)
                return false;

            removeAllThreteningPieceCanBeTaken(ref allRivalPiecesThreteningTheKing, playerKingRow, playerKingColumn, playerColor);
            if (allRivalPiecesThreteningTheKing.Count == 0)
                return false;

            removeAllThreteningPieceCanBeBlocked(ref allRivalPiecesThreteningTheKing, playerKingRow, playerKingColumn, playerColor);
            if (allRivalPiecesThreteningTheKing.Count == 0)
                return false;

            List<int> kingValidMoves = getKingValidMoves(playerKingRow, playerKingColumn, playerColor);
            List<int> rivalThretenedSquares = getPlayerAllThretendSquares(rivalColor);

            int initialNumberOfMoves = kingValidMoves.Count/2;
            int count = 0;
            for (int i = 0; count < initialNumberOfMoves; count++)
            {
                if (movesContains(rivalThretenedSquares, kingValidMoves[i], kingValidMoves[i + 1]))
                {
                    kingValidMoves.RemoveAt(i + 1);
                    kingValidMoves.RemoveAt(i);
                }
            }

            if (kingValidMoves.Count == 0)
                return true;

            return false;
        }

        private void removeAllThreteningPieceCanBeBlocked(ref List<int> allRivalPiecesThreteningTheKing, int playerKingRow, int playerKingColumn, ChessGameEntityColor playerColor)
        {
            List<int> playerValidMoves = getPlayerAllValidMovesButTheKing(playerColor);
            int count = 0;
            for (int i = 0; count < allRivalPiecesThreteningTheKing.Count / 2; count++)
            {
                if (pieceCanBeBlocked(playerValidMoves, allRivalPiecesThreteningTheKing[i], allRivalPiecesThreteningTheKing[i + 1], playerKingRow, playerKingColumn))
                {
                    allRivalPiecesThreteningTheKing.RemoveAt(i + 1);
                    allRivalPiecesThreteningTheKing.RemoveAt(i);
                }
            }
        }

        private bool pieceCanBeBlocked(List<int> playerValidMoves, int pieceRow, int pieceColumn, int playerKingRow, int playerKingColumn)
        {
            int rowDirection = pieceRow - playerKingRow;
            if (rowDirection < -1)
                rowDirection = -1;
            else if (rowDirection > 1)
                rowDirection = 1;

            int columnDirection = pieceColumn - playerKingColumn;
            if (columnDirection < -1)
                columnDirection = -1;
            else if (columnDirection > 1)
                columnDirection = 1;

            int i, j;
            for (i = playerKingRow + rowDirection, j = playerKingColumn + columnDirection;
                !(i == pieceRow && j == pieceColumn);
                i += rowDirection, j += columnDirection)
            {
                if (movesContains(playerValidMoves, i, j))
                    return true;
            }

            return false;
        }

        private void removeAllThreteningPieceCanBeTaken(ref List<int> allRivalPiecesThreteningTheKing, int playerKingRow, int playerKingColumn, ChessGameEntityColor playerColor)
        {
            List<int> allSquareThretedByPlayer = getPlayerAllThretendSquaresButTheKing(playerColor);

            int count = 0;
            for (int i = 0; count < allRivalPiecesThreteningTheKing.Count / 2; count++)
            {
                if (movesContains(allSquareThretedByPlayer, allRivalPiecesThreteningTheKing[i], allRivalPiecesThreteningTheKing[i + 1]))
                {
                    allRivalPiecesThreteningTheKing.RemoveAt(i + 1);
                    allRivalPiecesThreteningTheKing.RemoveAt(i);
                }
            }
        }

        private List<int> getAllRivalPiecesThreteningTheKing(int playerKingRow, int playerKingColumn, ChessGameEntityColor rivalColor)
        {
            List<int> retVal = new List<int>();
            List<int> currMoves;
            Square[,] squares = this._borad.Squares;
            Piece piece;

            for (int i = 0; i < squares.GetLength(0); i++)
            {
                for (int j = 0; j < squares.GetLength(1); j++)
                {
                    piece = squares[i, j].PieceOnASquare;
                    if (piece != null)
                    {
                        if (piece.PieceColor == rivalColor)
                        {
                            currMoves = getThretendSuaresOfPieceOnASquareBySquareCoordinatesIbdexes(i, j);
                            if(movesContains(currMoves, playerKingRow, playerKingColumn))
                            {
                                retVal.Add(i);
                                retVal.Add(j);
                            }
                        }
                    }
                }
            }

            return retVal;
        }

        private bool playerInCheck(ChessGameEntityColor playerColor)
        {
            int playerKingRow = -1, playerKingColumn = -1;
            this._borad.getKingSquareIndex(playerColor, ref playerKingRow, ref playerKingColumn);

            List<int> rivalThretendSquares = getPlayerAllThretendSquares(ChessGameUtility.GetRivalColor(playerColor));

            if (movesContains(rivalThretendSquares, playerKingRow, playerKingColumn))
                return true;
            else
                return false;
        }


        private List<int> getPlayerAllThretendSquaresButTheKing(ChessGameEntityColor playerColor)
        {
            List<int> moves = new List<int>();
            List<int> currPieceMoves;
            Square[,] squares = this._borad.Squares;
            Piece piece;

            for (int i = 0; i < squares.GetLength(0); i++)
            {
                for (int j = 0; j < squares.GetLength(1); j++)
                {
                    piece = squares[i, j].PieceOnASquare;
                    if (piece != null)
                    {
                        if (piece.PieceColor == playerColor)
                        {
                            if (piece.PieceType != PieceTypes.WHITE_KING && piece.PieceType != PieceTypes.BLACK_KING)
                            {
                                currPieceMoves = getThretendSuaresOfPieceOnASquareBySquareCoordinatesIbdexes(i, j);
                                moves.AddRange(currPieceMoves);
                            }
                        }
                    }
                }
            }

            return moves;
        }

        private List<int> getPlayerAllThretendSquares(ChessGameEntityColor playerColor)
        {
            List<int> moves = new List<int>();
            List<int> currPieceMoves;
            Square[,] squares = this._borad.Squares;
            Piece piece;

            for(int i=0; i<squares.GetLength(0); i++)
            {
                for(int j=0; j<squares.GetLength(1); j++)
                {
                    piece = squares[i, j].PieceOnASquare;
                    if (piece != null)
                    {
                        if (piece.PieceColor == playerColor)
                        {
                            currPieceMoves = getThretendSuaresOfPieceOnASquareBySquareCoordinatesIbdexes(i, j);
                            moves.AddRange(currPieceMoves);
                        }
                    }
                }
            }

            return moves;
        }

        private List<int> getPlayerAllValidMoves(ChessGameEntityColor playerColor)
        {
            List<int> moves = new List<int>();
            List<int> currPieceMoves;
            Square[,] squares = this._borad.Squares;
            Piece piece;

            for (int i = 0; i < squares.GetLength(0); i++)
            {
                for (int j = 0; j < squares.GetLength(1); j++)
                {
                    piece = squares[i, j].PieceOnASquare;
                    if (piece != null)
                    {
                        if (piece.PieceColor == playerColor)
                        {
                            currPieceMoves = getValidMovesOfPieceOnASquareBySquareCoordinatesIndexes(i, j);
                            moves.AddRange(currPieceMoves);
                        }
                    }
                }
            }

            return moves;
        }

        private List<int> getPlayerAllValidMovesButTheKing(ChessGameEntityColor playerColor)
        {
            List<int> moves = new List<int>();
            List<int> currPieceMoves;
            Square[,] squares = this._borad.Squares;
            Piece piece;

            for (int i = 0; i < squares.GetLength(0); i++)
            {
                for (int j = 0; j < squares.GetLength(1); j++)
                {
                    piece = squares[i, j].PieceOnASquare;
                    if (piece != null)
                    {
                        if (piece.PieceType != PieceTypes.BLACK_KING && piece.PieceType != PieceTypes.WHITE_KING)
                        {
                            if (piece.PieceColor == playerColor)
                            {
                                currPieceMoves = getValidMovesOfPieceOnASquareBySquareCoordinatesIndexes(i, j);
                                moves.AddRange(currPieceMoves);
                            }
                        }
                    }
                }
            }

            return moves;
        }

        private bool movesContains(List<int> moves, int rowInd, int columnInd)
        {
            for(int i=0; i<moves.Count; i+=2)
            {
                if (moves[i] == rowInd && moves[i + 1] == columnInd)
                    return true;
            }

            return false;
        }


        //for debug
        internal List<int> GetValidMovesIndexes(Board board, ChessGameEntityColor currPlayerColor, GameStatus currGameStatus, int row, int column)
        {
            initMoveAnalizer(board, null, currPlayerColor, currGameStatus);
            return getValidMovesOfPieceOnASquareBySquareCoordinatesIndexes(row - 1, column - 1);
        }
        internal List<int> GetPieceThretendSquresIndexes(Board board, ChessGameEntityColor currPlayerColor, GameStatus currGameStatus, int row, int column)
        {
            initMoveAnalizer(board, null, currPlayerColor, currGameStatus);
            return getThretendSuaresOfPieceOnASquareBySquareCoordinatesIbdexes(row - 1, column - 1);
        }
        //end for debug code
         

        private List<int> getValidMovesOfPieceOnASquareBySquareCoordinatesIndexes(int rowIndex, int columnIndex)
        {
            List<int> validMoves = new List<int>();

            Piece piece = this._borad.getPieceBySquareCoordinateIndexes(rowIndex, columnIndex);

            if(piece != null)
            {
                switch(piece.PieceType)
                {
                    case PieceTypes.BLACK_POWN:
                    case PieceTypes.WHITE_POWN:
                        validMoves = getPownValidMoves(rowIndex, columnIndex, piece.PieceColor);
                        break;
                    case PieceTypes.BLACK_ROOK:
                    case PieceTypes.WHITE_ROOK:
                        validMoves = getRookValidMoves(rowIndex, columnIndex, piece.PieceColor);
                        break;
                    case PieceTypes.BLACK_BISHOP:
                    case PieceTypes.WHITE_BISHOP:
                        validMoves = getBishpValidMoves(rowIndex, columnIndex, piece.PieceColor);
                        break;
                    case PieceTypes.BLACK_QUEEN:
                    case PieceTypes.WHITE_QUEEN:
                        validMoves = getQueenValidMoves(rowIndex, columnIndex, piece.PieceColor);
                        break;
                    case PieceTypes.BLACK_KNIGHT:
                    case PieceTypes.WHITE_KNIGHT:
                        validMoves = getKnightValidMoves(rowIndex, columnIndex, piece.PieceColor);
                        break;
                    case PieceTypes.BLACK_KING:
                    case PieceTypes.WHITE_KING:
                        validMoves = getKingValidMoves(rowIndex, columnIndex, piece.PieceColor);
                        break;
                }
            }

            return validMoves;
        }
        private List<int>  getThretendSuaresOfPieceOnASquareBySquareCoordinatesIbdexes(int rowIndex, int columnIndex)
        {
            List<int> thretendSquares = new List<int>();

            Piece piece = this._borad.getPieceBySquareCoordinateIndexes(rowIndex, columnIndex);

            if (piece != null)
            {
                switch (piece.PieceType)
                {
                    case PieceTypes.BLACK_POWN:
                    case PieceTypes.WHITE_POWN:
                        thretendSquares = getPownThretendSquares(rowIndex, columnIndex, piece.PieceColor);
                        break;
                    case PieceTypes.BLACK_ROOK:
                    case PieceTypes.WHITE_ROOK:
                        thretendSquares = getRookThretenedSqures(rowIndex, columnIndex, piece.PieceColor);
                        break;
                    case PieceTypes.BLACK_BISHOP:
                    case PieceTypes.WHITE_BISHOP:
                        thretendSquares = getBishpThretenedSqures(rowIndex, columnIndex, piece.PieceColor);
                        break;
                    case PieceTypes.BLACK_QUEEN:
                    case PieceTypes.WHITE_QUEEN:
                        thretendSquares = getQueenThretendSquares(rowIndex, columnIndex, piece.PieceColor);
                        break;
                    case PieceTypes.BLACK_KNIGHT:
                    case PieceTypes.WHITE_KNIGHT:
                        thretendSquares = getKnightThretenedSqures(rowIndex, columnIndex, piece.PieceColor);
                        break;
                    case PieceTypes.BLACK_KING:
                    case PieceTypes.WHITE_KING:
                        thretendSquares = getKingThretendSquares(rowIndex, columnIndex, piece.PieceColor);
                        break;
                }
            }

            return thretendSquares;
        }

        private List<int> getPownValidMoves(int pownRowCoordinateIndex, int pownColumnCoordinateIndex, ChessGameEntityColor pownColor)
        {
            List<int> moves = getPownThretendSquares(pownRowCoordinateIndex, pownColumnCoordinateIndex, pownColor);

            int rowDirection = pownColor == ChessGameEntityColor.WHITE ? 1 : -1;
            Piece piece = this._borad.getPieceBySquareCoordinateIndexes(pownRowCoordinateIndex + rowDirection, pownColumnCoordinateIndex);
            if (piece == null)
            {
                moves.Add(pownRowCoordinateIndex + rowDirection);
                moves.Add(pownColumnCoordinateIndex);

                if (this._borad.getPieceBySquareCoordinateIndexes(pownRowCoordinateIndex, pownColumnCoordinateIndex).IndicatorFirstMove == FirstMoveIndicator.NOT_MOVED)
                {
                    piece = this._borad.getPieceBySquareCoordinateIndexes(pownRowCoordinateIndex + rowDirection + rowDirection, pownColumnCoordinateIndex);
                    if (piece == null)
                    {
                        moves.Add(pownRowCoordinateIndex + rowDirection + rowDirection);
                        moves.Add(pownColumnCoordinateIndex);
                    }
                }
            }

            return moves;
        }
        private List<int> getPownThretendSquares(int pownRowCoordinateIndex, int pownColumnCoordinateIndex, ChessGameEntityColor pownColor)
        {
            List<int> moves = new List<int>();

            int rowDirection = pownColor == ChessGameEntityColor.WHITE ? 1 : -1;
            Square square;
            Piece piece;

            if (validSquareCoordinatesIndexes(pownRowCoordinateIndex + rowDirection, pownColumnCoordinateIndex + 1))
            {
                piece = this._borad.getPieceBySquareCoordinateIndexes(pownRowCoordinateIndex + rowDirection, pownColumnCoordinateIndex + 1);
                if(piece!=null)
                {
                    if(piece.PieceColor != pownColor)
                    {
                        moves.Add(pownRowCoordinateIndex + rowDirection);
                        moves.Add(pownColumnCoordinateIndex + 1);
                    }
                }
            }
            if (validSquareCoordinatesIndexes(pownRowCoordinateIndex + rowDirection, pownColumnCoordinateIndex - 1))
            {
                piece = this._borad.getPieceBySquareCoordinateIndexes(pownRowCoordinateIndex + rowDirection, pownColumnCoordinateIndex - 1);
                if (piece != null)
                {
                    if (piece.PieceColor != pownColor)
                    {
                        moves.Add(pownRowCoordinateIndex + rowDirection);
                        moves.Add(pownColumnCoordinateIndex - 1);
                    }
                }
            }

            //en passent
            bool flag;
            if (pownColor == ChessGameEntityColor.WHITE)
            {
                if (pownRowCoordinateIndex == 4)
                    flag = true;
                else
                    flag = false;
            }
            else//pownColor == ChessGameEntityColor.BALCK
            {
                if (pownRowCoordinateIndex == 3)
                    flag = true;
                else
                    flag = false;
            }

            if (flag)
            {

                if (validSquareCoordinatesIndexes(pownRowCoordinateIndex, pownColumnCoordinateIndex + 1))
                {

                    square = this._borad.getSquareBySquareCoordinateIndexes(pownRowCoordinateIndex, pownColumnCoordinateIndex + 1);
                    if (square.PieceOnASquare != null)
                    {
                        if (square.PieceOnASquare.PieceColor != pownColor)
                        {

                            if (square.PieceOnASqaureCounter == 0)
                            {
                                moves.Add(pownRowCoordinateIndex + rowDirection);
                                moves.Add(pownColumnCoordinateIndex + 1);
                            }
                        }
                    }
                }

                //en passent
                if (validSquareCoordinatesIndexes(pownRowCoordinateIndex, pownColumnCoordinateIndex - 1))
                {
                    square = this._borad.getSquareBySquareCoordinateIndexes(pownRowCoordinateIndex, pownColumnCoordinateIndex - 1);
                    if (square.PieceOnASquare != null)
                    {
                        if (square.PieceOnASquare.PieceColor != pownColor)
                        {
                            if (square.PieceOnASqaureCounter == 0)
                            {
                                moves.Add(pownRowCoordinateIndex + rowDirection);
                                moves.Add(pownColumnCoordinateIndex - 1);
                            }
                        }
                    }
                }
            }


            return moves;
        }

        private List<int> getKingValidMoves(int kingRowCoordinateIndex, int kingColumnCoordinateIndex, ChessGameEntityColor kingColor)
        {
            List<int> moves = new List<int>();

            addValidMoveIfValidIndexes(ref moves, kingRowCoordinateIndex + 1, kingColumnCoordinateIndex - 1, kingColor);
            addValidMoveIfValidIndexes(ref moves, kingRowCoordinateIndex + 1, kingColumnCoordinateIndex, kingColor);
            addValidMoveIfValidIndexes(ref moves, kingRowCoordinateIndex + 1, kingColumnCoordinateIndex + 1, kingColor);
            addValidMoveIfValidIndexes(ref moves, kingRowCoordinateIndex, kingColumnCoordinateIndex - 1, kingColor);
            addValidMoveIfValidIndexes(ref moves, kingRowCoordinateIndex, kingColumnCoordinateIndex + 1, kingColor);
            addValidMoveIfValidIndexes(ref moves, kingRowCoordinateIndex - 1, kingColumnCoordinateIndex - 1, kingColor);
            addValidMoveIfValidIndexes(ref moves, kingRowCoordinateIndex - 1, kingColumnCoordinateIndex, kingColor);
            addValidMoveIfValidIndexes(ref moves, kingRowCoordinateIndex - 1, kingColumnCoordinateIndex + 1, kingColor);

            //casteling
            Piece king = this._borad.getPieceBySquareCoordinateIndexes(kingRowCoordinateIndex, kingColumnCoordinateIndex);
            Piece piece;
            if(kingColor == ChessGameEntityColor.WHITE && kingRowCoordinateIndex == 0 && kingColumnCoordinateIndex==4)
            {
                if(king.IndicatorFirstMove == FirstMoveIndicator.NOT_MOVED)
                {
                    if (this._borad.getPieceBySquareCoordinateIndexes(0, 5) == null &&
                       this._borad.getPieceBySquareCoordinateIndexes(0, 6) == null)
                    {
                        piece = this._borad.getPieceBySquareCoordinateIndexes(0, 7);
                        if (piece != null)
                        {
                            if (piece.PieceType == PieceTypes.WHITE_ROOK)
                            {
                                if (piece.IndicatorFirstMove == FirstMoveIndicator.NOT_MOVED)
                                {
                                    moves.Add(0);
                                    moves.Add(6);
                                }
                            }
                        }
                    }

                    if (this._borad.getPieceBySquareCoordinateIndexes(0, 3) == null&&
                        this._borad.getPieceBySquareCoordinateIndexes(0, 2) == null&&
                        this._borad.getPieceBySquareCoordinateIndexes(0,1) == null)
                    {
                        piece = this._borad.getPieceBySquareCoordinateIndexes(0, 0);
                        if (piece != null)
                        {
                            if (piece != null)
                            {
                                if (piece.PieceType == PieceTypes.WHITE_ROOK)
                                {
                                    if (piece.IndicatorFirstMove == FirstMoveIndicator.NOT_MOVED)
                                    {
                                        moves.Add(0);
                                        moves.Add(2);
                                    }
                                }
                            }
                        }
                    }        
                }
            }

            if (kingColor == ChessGameEntityColor.BLACK && kingRowCoordinateIndex == 7 && kingColumnCoordinateIndex == 4)
            {
                if (king.IndicatorFirstMove == FirstMoveIndicator.NOT_MOVED)
                {
                    if (this._borad.getPieceBySquareCoordinateIndexes(7, 5) == null &&
                      this._borad.getPieceBySquareCoordinateIndexes(7, 6) == null)
                    {
                        piece = this._borad.getPieceBySquareCoordinateIndexes(7, 7);
                        if(piece!=null)
                        { 
                        if (piece.PieceType == PieceTypes.BLACK_ROOK)
                        {
                                if (piece.IndicatorFirstMove == FirstMoveIndicator.NOT_MOVED)
                                {
                                    moves.Add(7);
                                    moves.Add(6);
                                }
                            }
                        }
                    }

                    if (this._borad.getPieceBySquareCoordinateIndexes(7, 3) == null &&
                        this._borad.getPieceBySquareCoordinateIndexes(7, 2) == null &&
                        this._borad.getPieceBySquareCoordinateIndexes(7, 1) == null)
                    {
                        piece = this._borad.getPieceBySquareCoordinateIndexes(7, 0);
                        if (piece != null)
                        {
                            if (piece.PieceType == PieceTypes.BLACK_ROOK)
                            {
                                if (piece.IndicatorFirstMove == FirstMoveIndicator.NOT_MOVED)
                                {
                                    moves.Add(7);
                                    moves.Add(2);
                                }
                            }
                        }
                    }
                }

            }

            return moves;
        }
        private List<int> getKingThretendSquares(int kingRowCoordinateIndex, int kingColumnCoordinateIndex, ChessGameEntityColor kingColor)
        {
            List<int> moves = new List<int>();

            addThretendeSquareIfValidIndexes(ref moves, kingRowCoordinateIndex + 1, kingColumnCoordinateIndex - 1, kingColor);
            addThretendeSquareIfValidIndexes(ref moves, kingRowCoordinateIndex + 1, kingColumnCoordinateIndex, kingColor);
            addThretendeSquareIfValidIndexes(ref moves, kingRowCoordinateIndex + 1, kingColumnCoordinateIndex + 1, kingColor);
            addThretendeSquareIfValidIndexes(ref moves, kingRowCoordinateIndex, kingColumnCoordinateIndex - 1, kingColor);
            addThretendeSquareIfValidIndexes(ref moves, kingRowCoordinateIndex, kingColumnCoordinateIndex + 1, kingColor);
            addThretendeSquareIfValidIndexes(ref moves, kingRowCoordinateIndex - 1, kingColumnCoordinateIndex - 1, kingColor);
            addThretendeSquareIfValidIndexes(ref moves, kingRowCoordinateIndex - 1, kingColumnCoordinateIndex, kingColor);
            addThretendeSquareIfValidIndexes(ref moves, kingRowCoordinateIndex - 1, kingColumnCoordinateIndex + 1, kingColor);

            return moves;
        }

        private List<int> getKnightValidMoves(int knightRowCoordinateIndex, int knigthColumnCoordinateIndex, ChessGameEntityColor knigthColor)
        {
            List<int> moves = new List<int>();

            addValidMoveIfValidIndexes(ref moves, knightRowCoordinateIndex + 1, knigthColumnCoordinateIndex - 2, knigthColor);
            addValidMoveIfValidIndexes(ref moves, knightRowCoordinateIndex + 1, knigthColumnCoordinateIndex + 2, knigthColor);
            addValidMoveIfValidIndexes(ref moves, knightRowCoordinateIndex + 2, knigthColumnCoordinateIndex - 1, knigthColor);
            addValidMoveIfValidIndexes(ref moves, knightRowCoordinateIndex + 2, knigthColumnCoordinateIndex + 1, knigthColor);
            addValidMoveIfValidIndexes(ref moves, knightRowCoordinateIndex - 1, knigthColumnCoordinateIndex - 2, knigthColor);
            addValidMoveIfValidIndexes(ref moves, knightRowCoordinateIndex - 1, knigthColumnCoordinateIndex + 2, knigthColor);
            addValidMoveIfValidIndexes(ref moves, knightRowCoordinateIndex - 2, knigthColumnCoordinateIndex - 1, knigthColor);
            addValidMoveIfValidIndexes(ref moves, knightRowCoordinateIndex - 2, knigthColumnCoordinateIndex + 1, knigthColor);

            return  moves;
        }
        private List<int> getKnightThretenedSqures(int knightRowCoordinateIndex, int knighColumnCoordinateIndex, ChessGameEntityColor knigthColor)
        {
            List<int> moves = new List<int>();

            addThretendeSquareIfValidIndexes(ref moves, knightRowCoordinateIndex + 1, knighColumnCoordinateIndex - 2, knigthColor);
            addThretendeSquareIfValidIndexes(ref moves, knightRowCoordinateIndex + 1, knighColumnCoordinateIndex + 2, knigthColor);
            addThretendeSquareIfValidIndexes(ref moves, knightRowCoordinateIndex + 2, knighColumnCoordinateIndex - 1, knigthColor);
            addThretendeSquareIfValidIndexes(ref moves, knightRowCoordinateIndex + 2, knighColumnCoordinateIndex + 1, knigthColor);
            addThretendeSquareIfValidIndexes(ref moves, knightRowCoordinateIndex - 1, knighColumnCoordinateIndex - 2, knigthColor);
            addThretendeSquareIfValidIndexes(ref moves, knightRowCoordinateIndex - 1, knighColumnCoordinateIndex + 2, knigthColor);
            addThretendeSquareIfValidIndexes(ref moves, knightRowCoordinateIndex - 2, knighColumnCoordinateIndex - 1, knigthColor);
            addThretendeSquareIfValidIndexes(ref moves, knightRowCoordinateIndex - 2, knighColumnCoordinateIndex + 1, knigthColor);

            return moves;
        }

        private void addValidMoveIfValidIndexes(ref List<int> moves, int rowIndex, int columnIndex, ChessGameEntityColor knigthColor)
        {
            if (validSquareCoordinatesIndexes(rowIndex, columnIndex))
            {
                Piece piece = this._borad.getPieceBySquareCoordinateIndexes(rowIndex, columnIndex);
                if (piece != null)
                {
                    if (piece.PieceColor != knigthColor)
                    {
                        moves.Add(rowIndex);
                        moves.Add(columnIndex);
                    }
                }
                else
                {
                    moves.Add(rowIndex);
                    moves.Add(columnIndex);
                }
            }
        }
        private void addThretendeSquareIfValidIndexes(ref List<int> moves, int rowIndex, int columnIndex, ChessGameEntityColor knigthColor)
        {
            if (validSquareCoordinatesIndexes(rowIndex, columnIndex))
            {
                moves.Add(rowIndex);
                moves.Add(columnIndex);
            }
        }
        private bool validSquareCoordinatesIndexes(int rowIndex, int columnIndex)
        {
            if(validIndex(rowIndex))
            {
                if(validIndex(columnIndex))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private bool validIndex(int index)
        {
            if (index < FIRST_ROW_COLUMN_INDEX)
                return false;
            if (index > LAST_ROW_COLUMN_INDEX)
                return false;

            return true;
        }
        

        private List<int> getBishpValidMoves(int bishopRowCoordinateIndex, int bishopColumnCoordinateIndex, ChessGameEntityColor bishopColor)
        {
            List<int> moves = new List<int>();
            Piece currPiece;

            int rowInd, columnInd;

            bool continueLoop = true;
            for (rowInd = bishopRowCoordinateIndex + 1, columnInd = bishopColumnCoordinateIndex + 1;
                continueLoop && rowInd <= LAST_ROW_COLUMN_INDEX && columnInd <= LAST_ROW_COLUMN_INDEX;
                rowInd++, columnInd++)
            {
                currPiece = this._borad.getPieceBySquareCoordinateIndexes(rowInd, columnInd);
                if (currPiece != null)
                {
                    if (currPiece.PieceColor != bishopColor)
                    {
                        moves.Add(rowInd);
                        moves.Add(columnInd);
                    }

                    continueLoop = false;
                }
                else
                {
                    moves.Add(rowInd);
                    moves.Add(columnInd);
                }
            }

            continueLoop = true;
            for (rowInd = bishopRowCoordinateIndex - 1, columnInd = bishopColumnCoordinateIndex + 1;
                continueLoop && rowInd >= FIRST_ROW_COLUMN_INDEX && columnInd <= LAST_ROW_COLUMN_INDEX;
                rowInd--, columnInd++)
            {
                currPiece = this._borad.getPieceBySquareCoordinateIndexes(rowInd, columnInd);
                if (currPiece != null)
                {
                    if (currPiece.PieceColor != bishopColor)
                    {
                        moves.Add(rowInd);
                        moves.Add(columnInd);
                    }

                    continueLoop = false;
                }
                else
                {
                    moves.Add(rowInd);
                    moves.Add(columnInd);
                }
            }

            continueLoop = true;
            for (rowInd = bishopRowCoordinateIndex - 1, columnInd = bishopColumnCoordinateIndex - 1;
                continueLoop && rowInd >= FIRST_ROW_COLUMN_INDEX && columnInd >= FIRST_ROW_COLUMN_INDEX;
                rowInd--, columnInd--)
            {
                currPiece = this._borad.getPieceBySquareCoordinateIndexes(rowInd, columnInd);
                if (currPiece != null)
                {
                    if (currPiece.PieceColor != bishopColor)
                    {
                        moves.Add(rowInd);
                        moves.Add(columnInd);
                    }

                    continueLoop = false;
                }
                else
                {
                    moves.Add(rowInd);
                    moves.Add(columnInd);
                }
            }

            continueLoop = true;
            for (rowInd = bishopRowCoordinateIndex + 1, columnInd = bishopColumnCoordinateIndex - 1;
                continueLoop && rowInd <= LAST_ROW_COLUMN_INDEX && columnInd >= FIRST_ROW_COLUMN_INDEX;
                rowInd++, columnInd--)
            {
                currPiece = this._borad.getPieceBySquareCoordinateIndexes(rowInd, columnInd);
                if (currPiece != null)
                {
                    if (currPiece.PieceColor != bishopColor)
                    {
                        moves.Add(rowInd);
                        moves.Add(columnInd);
                    }

                    continueLoop = false;
                }
                else
                {
                    moves.Add(rowInd);
                    moves.Add(columnInd);
                }
            }
            return moves;
        }
        private List<int> getBishpThretenedSqures(int bishopRowCoordinateIndex, int bishopColumnCoordinateIndex, ChessGameEntityColor bishopColor)
        {
            List<int> moves = new List<int>();
            Piece currPiece;

            int rowInd, columnInd;

            bool continueLoop = true;
            for (rowInd = bishopRowCoordinateIndex + 1, columnInd = bishopColumnCoordinateIndex + 1;
                continueLoop && rowInd <= LAST_ROW_COLUMN_INDEX && columnInd <= LAST_ROW_COLUMN_INDEX;
                rowInd++, columnInd++)
            {
                currPiece = this._borad.getPieceBySquareCoordinateIndexes(rowInd, columnInd);
                if (currPiece != null)
                {
                    continueLoop = false;
                }

                moves.Add(rowInd);
                moves.Add(columnInd);
            }

            continueLoop = true;
            for (rowInd = bishopRowCoordinateIndex - 1, columnInd = bishopColumnCoordinateIndex + 1;
                continueLoop && rowInd >= FIRST_ROW_COLUMN_INDEX && columnInd <= LAST_ROW_COLUMN_INDEX;
                rowInd--, columnInd++)
            {
                currPiece = this._borad.getPieceBySquareCoordinateIndexes(rowInd, columnInd);
                if (currPiece != null)
                {
                    continueLoop = false;
                }

                moves.Add(rowInd);
                moves.Add(columnInd);
            }

            continueLoop = true;
            for (rowInd = bishopRowCoordinateIndex - 1, columnInd = bishopColumnCoordinateIndex - 1;
                continueLoop && rowInd >= FIRST_ROW_COLUMN_INDEX && columnInd >= FIRST_ROW_COLUMN_INDEX;
                rowInd--, columnInd--)
            {
                currPiece = this._borad.getPieceBySquareCoordinateIndexes(rowInd, columnInd);
                if (currPiece != null)
                {
                    continueLoop = false;
                }

                moves.Add(rowInd);
                moves.Add(columnInd);
            }

            continueLoop = true;
            for (rowInd = bishopRowCoordinateIndex + 1, columnInd = bishopColumnCoordinateIndex - 1;
                continueLoop && rowInd <= LAST_ROW_COLUMN_INDEX && columnInd >= FIRST_ROW_COLUMN_INDEX;
                rowInd++, columnInd--)
            {
                currPiece = this._borad.getPieceBySquareCoordinateIndexes(rowInd, columnInd);
                if (currPiece != null)
                {
                    continueLoop = false;
                }

                moves.Add(rowInd);
                moves.Add(columnInd);
            }

            return moves;
        }

        private List<int> getRookValidMoves(int rookRowCoordinateIndex, int rookColumnCoordinateIndex, ChessGameEntityColor rookColor)
        {
            List<int> moves = new List<int>();

            Piece currPiece;

            bool continueLoop = true;
            int rowInd = rookRowCoordinateIndex;
            int columnInd;
            for (columnInd = rookColumnCoordinateIndex + 1; continueLoop && columnInd <= LAST_ROW_COLUMN_INDEX; columnInd++)
            {
                currPiece = this._borad.getPieceBySquareCoordinateIndexes(rowInd, columnInd);
                if (currPiece != null)
                {
                    if (currPiece.PieceColor != rookColor)
                    {
                        moves.Add(rowInd);
                        moves.Add(columnInd);
                    }

                    continueLoop = false;
                }
                else
                {
                    moves.Add(rowInd);
                    moves.Add(columnInd);
                }
            }
            continueLoop = true;
            for (columnInd = rookColumnCoordinateIndex -1 ; continueLoop && columnInd >= FIRST_ROW_COLUMN_INDEX; columnInd--)
            {
                currPiece = this._borad.getPieceBySquareCoordinateIndexes(rowInd, columnInd);
                if (currPiece != null)
                {
                    if (currPiece.PieceColor != rookColor)
                    {
                        moves.Add(rowInd);
                        moves.Add(columnInd);
                    }
                    continueLoop = false;
                }
                else
                {
                    moves.Add(rowInd);
                    moves.Add(columnInd);
                }
            }

            continueLoop = true;
            columnInd = rookColumnCoordinateIndex;
            for (rowInd = rookRowCoordinateIndex + 1; continueLoop && rowInd <= LAST_ROW_COLUMN_INDEX; rowInd++)
            {
                currPiece = this._borad.getPieceBySquareCoordinateIndexes(rowInd, columnInd);
                if (currPiece != null)
                {
                    if (currPiece.PieceColor != rookColor)
                    {
                        moves.Add(rowInd);
                        moves.Add(columnInd);
                        //rookValidMoves[rowInd] = columnInd;
                    }
                    continueLoop = false;
                }
                else
                {
                    moves.Add(rowInd);
                    moves.Add(columnInd);
                    //rookValidMoves[rowInd] = columnInd;
                }
            }
            continueLoop = true;
            for (rowInd = rookRowCoordinateIndex - 1; continueLoop && rowInd >= FIRST_ROW_COLUMN_INDEX; rowInd--)
            {
                currPiece = this._borad.getPieceBySquareCoordinateIndexes(rowInd, columnInd);
                if (currPiece != null)
                {
                    if (currPiece.PieceColor != rookColor)
                    {
                        moves.Add(rowInd);
                        moves.Add(columnInd);
                        //rookValidMoves[rowInd] = columnInd;
                    }
                        continueLoop = false;
                }
                else
                {
                    moves.Add(rowInd);
                    moves.Add(columnInd);
                    //rookValidMoves[rowInd] = columnInd;
                }
            }

            return moves;
        }
        private List<int> getRookThretenedSqures(int rookRowCoordinateIndex, int rookColumnCoordinateIndex, ChessGameEntityColor rookColor)
        {
            List<int> thretenedSquares = new List<int>();
            Piece currPiece;

            bool continueLoop = true;
            int rowInd = rookRowCoordinateIndex;
            int columnInd;
            for (columnInd = rookColumnCoordinateIndex + 1; continueLoop && columnInd <= LAST_ROW_COLUMN_INDEX; columnInd++)
            {
                currPiece = this._borad.getPieceBySquareCoordinateIndexes(rowInd, columnInd);
                if (currPiece != null)
                {
                    continueLoop = false;
                }

                thretenedSquares.Add(rowInd);
                thretenedSquares.Add(columnInd);
            }
            continueLoop = true;
            for (columnInd = rookColumnCoordinateIndex - 1; continueLoop && columnInd >= FIRST_ROW_COLUMN_INDEX; columnInd--)
            {
                currPiece = this._borad.getPieceBySquareCoordinateIndexes(rowInd, columnInd);
                if (currPiece != null)
                {
                    continueLoop = false;
                }

                thretenedSquares.Add(rowInd);
                thretenedSquares.Add(columnInd);
            }

            continueLoop = true;
            columnInd = rookColumnCoordinateIndex;
            for (rowInd = rookRowCoordinateIndex + 1; continueLoop && rowInd <= LAST_ROW_COLUMN_INDEX; rowInd++)
            {
                currPiece = this._borad.getPieceBySquareCoordinateIndexes(rowInd, columnInd);
                if (currPiece != null)
                {
                    continueLoop = false;
                }

                thretenedSquares.Add(rowInd);
                thretenedSquares.Add(columnInd);
            }
            continueLoop = true;
            for (rowInd = rookRowCoordinateIndex - 1; continueLoop && rowInd >= FIRST_ROW_COLUMN_INDEX; rowInd--)
            {
                currPiece = this._borad.getPieceBySquareCoordinateIndexes(rowInd, columnInd);
                if (currPiece != null)
                {
                    continueLoop = false;
                }

                thretenedSquares.Add(rowInd);
                thretenedSquares.Add(columnInd);
            }
            return thretenedSquares;
        }

        private List<int> getQueenValidMoves(int queenRowCoordinateIndex, int queenColumnCoordinateIndex, ChessGameEntityColor queenColor)
        {
            //horizontal and vertical moves
            List<int> moves = getRookValidMoves(queenRowCoordinateIndex, queenColumnCoordinateIndex, queenColor);
            //diagonal moves
            List<int> diagonalMoves = getBishpValidMoves(queenRowCoordinateIndex, queenColumnCoordinateIndex, queenColor);

            foreach (int move in diagonalMoves.ToArray())
            {
                moves.Add(move);
            }

            return moves;
        }
        private List<int> getQueenThretendSquares(int queenRowCoordinateIndex, int queenColumnCoordinateIndex, ChessGameEntityColor queenColor)
        {
            //horizontal and vertical moves
            List<int> moves = getRookThretenedSqures(queenRowCoordinateIndex, queenColumnCoordinateIndex, queenColor);
            //diagonal moves
            List<int> diagonalMoves = getBishpThretenedSqures(queenRowCoordinateIndex, queenColumnCoordinateIndex, queenColor);

            foreach (int move in diagonalMoves.ToArray())
            {
                moves.Add(move);
            }

            return moves;
        }




        private void initMoveAnalizer(Board board, Move move, ChessGameEntityColor currPlayerColor, GameStatus currGameStatus)
        {
            this._borad = new Board(board);
            this._move = new Move(move);
            this._currPlayerColor = currPlayerColor;
            this._currGameStatus = currGameStatus;
        }

        MoveResult updateMoveResult(ref MoveResult moveResult, GameStatus newGameStatus, Exception moveAnalizerResultExeption)
        {
            moveResult.NewGameStatus = newGameStatus;
            moveResult.MoveAnalizerResultExeption = moveAnalizerResultExeption;

            return moveResult;
        }
    }

    public struct MoveResult
    {
        private GameStatus _newGameStatus;
        private Exception _moveAnalizerResultExeption;

        public bool MoveSuccessFlag
        {
            get
            {
                return this._moveAnalizerResultExeption == null ? true : false;
            }
        }
        public GameStatus NewGameStatus
        {
            get
            {
                return this._newGameStatus;
            }
            set
            {
                this._newGameStatus = value;
            }
        }
        public Exception MoveAnalizerResultExeption
        {
            get
            {
                return this._moveAnalizerResultExeption;
            }
            set
            {
                this._moveAnalizerResultExeption = value;
            }
        }
    }
}
