using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameLogic
{
    public class Game
    {
        private Board _board;
        private GameStatus _currGameStatus;
        private MoveAnalizer _moveAnalizer;
        private ChessGameEntityColor _currPlayerColor;

        public ChessGameEntityColor CurrPlayerColor
        {
            get
            {
                return this._currPlayerColor;
            }
        }

        public Board BoradOfTheGame
        {
            get
            {
                return this._board;
            }
        }

        public Game()
        {
            this._board = new Board();
            this._moveAnalizer = new MoveAnalizer();
            this._currGameStatus = GameStatus.CONTINUE;
            this._currPlayerColor = ChessGameEntityColor.WHITE;
        }

        public MoveResult MakeMove(Move move)
        {
            MoveResult moveResult;
            try
            {
                if (!gameOver())
                {
                    /////
                    moveResult = this._moveAnalizer.AnalizeMove(this._board, move, this._currPlayerColor, this._currGameStatus);
                    /////

                    if (moveResult.MoveSuccessFlag)
                    {
                        if ((move.TypeOfTheMove == MoveType.STANDART) || (move.TypeOfTheMove == MoveType.POWN_PROMOTION))
                        {
                            this._board.makeMove(move);
                        }

                        this._currGameStatus = moveResult.NewGameStatus;

                        if (!gameOver() ||
                            moveResult.NewGameStatus == GameStatus.RESIGNATION ||
                            moveResult.NewGameStatus == GameStatus.FORFEIT || 
                            moveResult.NewGameStatus == GameStatus.WIN_ON_TIME)
                            this._currPlayerColor = ChessGameUtility.GetRivalColor(this._currPlayerColor);
                    }
                }
                else
                {
                    moveResult = new MoveResult() { MoveAnalizerResultExeption = new ExceptionChessGameLogic("Game over."), NewGameStatus = this._currGameStatus };
                }
            }
            catch (Exception e)
            {
                moveResult = new MoveResult() { MoveAnalizerResultExeption = e, NewGameStatus = this._currGameStatus };
            }

            return moveResult;
        }
        bool gameOver()
        {
            bool retVal;

            switch (this._currGameStatus)
            {
                case GameStatus.CHECK_MATE:
                case GameStatus.DRAW_BY_AGREEMENT:
                case GameStatus.RESIGNATION:
                case GameStatus.FORFEIT:
                case GameStatus.DROW_STALEMATE:
                case GameStatus.DROW_DU_TO_INSUFFICIENT_MATERIAL:
                case GameStatus.WIN_ON_TIME:
                    retVal = true;
                    break;
                default:
                    retVal = false;
                    break;
            }

            return retVal;
        }

        //for debug
        public List<int> GetPieceValidMoves(int row, int column)
        {
            List<int> retValIndexes = this._moveAnalizer.GetValidMovesIndexes(this._board, this._currPlayerColor, this._currGameStatus, row, column);
            List<int> retVal = new List<int>();

            foreach (int item in retValIndexes.ToArray())
            {
                retVal.Add(item + 1);
            }

            return retVal;
        }
        public List<int> GetPieceThretendSqures(int row, int column)
        {
            List<int> retValIndexes = this._moveAnalizer.GetPieceThretendSquresIndexes(this._board, this._currPlayerColor, this._currGameStatus, row, column);
            List<int> retVal = new List<int>();

            foreach (int item in retValIndexes.ToArray())
            {
                retVal.Add(item + 1);
            }

            return retVal;
        }
        //end for debug code

        public PieceTypes GetPieceTypeOnAsquare(char row, char column)
        {
            int rowInd = ChessGameUtility.GetRowIndex(row);
            int columnInd = ChessGameUtility.GetColumnIndex(column);

            return this._board.Squares[rowInd, columnInd].PieceOnASquare.PieceType;
        }
    }

    public enum GameStatus
    {
        CONTINUE,
        CHECK, 
        CHECK_MATE, 
        DRAW_BY_AGREEMENT,
        DRAW_BY_AGREEMENT_WAITING_RESPONSE,
        RESIGNATION,
        FORFEIT,
        DROW_STALEMATE,
        DROW_BY_REPETITION,
        WIN_ON_TIME,
        DROW_DU_TO_INSUFFICIENT_MATERIAL
        //DROW_FIFTY_MOVE_RULE,

    }
}
