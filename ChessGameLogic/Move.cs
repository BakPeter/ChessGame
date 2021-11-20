using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameLogic
{
    public class Move
    {
        private MoveType _typeOfTheMove;
        private string _fromSquare;
        private string _toSquare;
        private Exception _moveValidationException;
        private PieceTypes _pownPromotionType;


        public PieceTypes PownPromotionType
        {
            get
            {
                return this._pownPromotionType;
            }
            private set
            {
                this._pownPromotionType = value;
            }

        }

        public MoveType TypeOfTheMove
        {
            get
            {
                return this._typeOfTheMove;
            }
            private set
            {
                this._typeOfTheMove = value;
            }
        }
        public string FromSquare
        {
            get
            {
                return this._fromSquare;
            }
            private set
            {
                this._fromSquare = value;
            }

        }
        public string ToSquare
        {
            get
            {
                return this._toSquare;
            }
            private set
            {
                this._toSquare = value;
            }

        }
        public Exception MoveValidationException
        {
            get
            {
                return this._moveValidationException;
            }
            set
            {
                this._moveValidationException = value;
            }

        }
        public bool MoveValid
        {
            get
            {
                return this._moveValidationException == null ? true : false;
            }
        }


        public Move(MoveType moveType, string fromSquare, string toSquare, PieceTypes pownPromotinType)
        {
            initMove(moveType, fromSquare, toSquare, pownPromotinType);
        }

        public Move(Move other)
        {
            if(other == null)
            {
                MoveValidationException = new ArgumentNullException("Param for Copy Chtor is null");
            }
            else
            {
                initMove(other._typeOfTheMove, other._fromSquare, other._toSquare, other._pownPromotionType);
            }
        }

        private void initMove(MoveType moveType, string fromSquare, string toSquare, PieceTypes pownPromotinType)
        {
            TypeOfTheMove = moveType;
            FromSquare = fromSquare;
            ToSquare = toSquare;
            PownPromotionType = pownPromotinType;

            checkAndUpdateMoveValidation();
        }
        private void checkAndUpdateMoveValidation()
        {
            string fromStr = "fromSquare";
            string toStr = "toSquare";
            if (this._typeOfTheMove == MoveType.STANDART)
            {
                if(this._fromSquare!=null && this._toSquare!=null)
                {
                    checkSquareLegalityAndUpdateMoveValidation(this._fromSquare, fromStr);
                    checkSquareLegalityAndUpdateMoveValidation(this._toSquare, toStr);

                    if(MoveValid)
                    {
                        if(this._fromSquare == this._toSquare)
                        {
                            setMoveToInValid(string.Format("Can't make move {0}.\n Two diffrent squares are needed.", this.ToString()));
                        }
                    }
                }
                else
                {
                    setMoveToInValid("Can't make move {0}.\nParam fromSquare or Param toSquare are null.");
                }
            }
            else//this._typeOfTheMove != MoveType.STANADART
            {
                setMoveToValid();
            }
        }

        private void checkSquareLegalityAndUpdateMoveValidation(string squareCoordinates, string square)
        {
            string validatonErrorMsg = null;
            if (squareCoordinates.Length < 2 || squareCoordinates.Length > 2)
            {
                validatonErrorMsg = string.Format("Param {0}, {1}, is in valid :", square, squareCoordinates);
                validatonErrorMsg += "\nParam not in format of <row><column>";
            }
            else if (!ChessGameUtility.validValueForRow(squareCoordinates[1]))
            {
                validatonErrorMsg += string.Format("\nRow legal values are 1...8, not {0}", squareCoordinates[1]);
            }
            else if (!ChessGameUtility.validValueForColumn(squareCoordinates[0]))
            {
                validatonErrorMsg += string.Format("\nColumn legal values are a...b, not {0}", squareCoordinates[0]);
            }

            if (validatonErrorMsg == null)
                setMoveToValid();
            else
                setMoveToInValid("Can't make move {0}.\n"+validatonErrorMsg);
        }

        public override string ToString()
        {
            return this._fromSquare + this._toSquare;
        }

        protected void setMoveToInValid(string msg)
        {
            MoveValidationException = new ArgumentException(msg);

        }
        private void setMoveToValid()
        {
            MoveValidationException = null;
        }
    }

    public enum MoveType
    {
        //move with 2 squares
        STANDART,
        POWN_PROMOTION,
        //moves without squares
        DRAW_BY_AGREEMENT,
        RESIGNATION,
        FORFEIT,
        DROW_BY_REPETITION,
        DROW_DU_TO_INSUFFICIENT_MATERIAL,
        WIN_ON_TIME,
    }
}
