using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChessGameLogic;

namespace ChessGameFinalVersion
{
    public partial class Form1 : Form
    {
        private Label[,] _labels = new Label[8, 8];
        private Game _gameLogic;

        private string _fromSquare;
        private string _toSquare;
        private int _squareClickedCounter = 0;
        private MoveResult _moveResult;

        public const int NUMBER_OF_MINUTES_PER_GAME = 10;
        private const int NUMBER_OF_SECONDS_IN_A_MINUTE = 60;
        private int _whiteTimerCounter;
        private int _blackTimerCounter;

        List<string> _movesForDebug = new List<string> {
            "a2a4",
"a7a5",
"b2b4",
"b7b5",
"c2c4",
"c7c5",
"d2d4",
"d7d5",
"e2e4",
"e7e5",
"f2f4",
"f7f5",
"g2g4",
"g7g5",
"h2h4",
"h7h5",
"a5a5",
"a4b5",
"a5b4",
"c4d5",
"c5d4",
"e4f5",
"e5f4",
"g4h5",
"g5h4",
"d1d4",
"d8d5",
"d4b4",
"d5b5",
"b4f4",
"b5f5",
"f4h4",
"f5h5",
"h4a4",
"e8d8",
"h5h5",
"a4a8",
"h5h1",
"a8a6",
"h1h3",
"a1a3",
"b8a6",
"a3h3",
"h8h3",
"f1h3",
"h3c8",
"c8h3",
"g1h3",
"f8h6",
"c1h6",
"g8h6",
"b1c3",
"a6c5",
"c3e4",
"c5e4",
"h6g4",
"h3f4",
"h6g4",
"e4d2",
"f4g2",
"e4d2",
"e1d2",
"g4e4",
"g4e3",
"d2d2",
"d2e3",


};
        int _movesForDebugCounter = 0;
        int _stopAutuMoveForDebugInMoveNumber = 70;

        private List<string> _movesToFile = new List<string>();
        
        public Form1()
        {
            InitializeComponent();

            _labels[7, 0] = _labelBoard8a;
            _labels[7, 1] = _labelBoard8b;
            _labels[7, 2] = _labelBoard8c;
            _labels[7, 3] = _labelBoard8d;
            _labels[7, 4] = _labelBoard8e;
            _labels[7, 5] = _labelBoard8f;
            _labels[7, 6] = _labelBoard8g;
            _labels[7, 7] = _labelBoard8h;

            _labels[6, 0] = _labelBoard7a;
            _labels[6, 1] = _labelBoard7b;
            _labels[6, 2] = _labelBoard7c;
            _labels[6, 3] = _labelBoard7d;
            _labels[6, 4] = _labelBoard7e;
            _labels[6, 5] = _labelBoard7f;
            _labels[6, 6] = _labelBoard7g;
            _labels[6, 7] = _labelBoard7h;

            _labels[5, 0] = _labelBoard6a;
            _labels[5, 1] = _labelBoard6b;
            _labels[5, 2] = _labelBoard6c;
            _labels[5, 3] = _labelBoard6d;
            _labels[5, 4] = _labelBoard6e;
            _labels[5, 5] = _labelBoard6f;
            _labels[5, 6] = _labelBoard6g;
            _labels[5, 7] = _labelBoard6h;

            _labels[4, 0] = _labelBoard5a;
            _labels[4, 1] = _labelBoard5b;
            _labels[4, 2] = _labelBoard5c;
            _labels[4, 3] = _labelBoard5d;
            _labels[4, 4] = _labelBoard5e;
            _labels[4, 5] = _labelBoard5f;
            _labels[4, 6] = _labelBoard5g;
            _labels[4, 7] = _labelBoard5h;

            _labels[3, 0] = _labelBoard4a;
            _labels[3, 1] = _labelBoard4b;
            _labels[3, 2] = _labelBoard4c;
            _labels[3, 3] = _labelBoard4d;
            _labels[3, 4] = _labelBoard4e;
            _labels[3, 5] = _labelBoard4f;
            _labels[3, 6] = _labelBoard4g;
            _labels[3, 7] = _labelBoard4h;


            _labels[2, 0] = _labelBoard3a;
            _labels[2, 1] = _labelBoard3b;
            _labels[2, 2] = _labelBoard3c;
            _labels[2, 3] = _labelBoard3d;
            _labels[2, 4] = _labelBoard3e;
            _labels[2, 5] = _labelBoard3f;
            _labels[2, 6] = _labelBoard3g;
            _labels[2, 7] = _labelBoard3h;

            _labels[1, 0] = _labelBoard2a;
            _labels[1, 1] = _labelBoard2b;
            _labels[1, 2] = _labelBoard2c;
            _labels[1, 3] = _labelBoard2d;
            _labels[1, 4] = _labelBoard2e;
            _labels[1, 5] = _labelBoard2f;
            _labels[1, 6] = _labelBoard2g;
            _labels[1, 7] = _labelBoard2h;


            _labels[0, 0] = _labelBoard1a;
            _labels[0, 1] = _labelBoard1b;
            _labels[0, 2] = _labelBoard1c;
            _labels[0, 3] = _labelBoard1d;
            _labels[0, 4] = _labelBoard1e;
            _labels[0, 5] = _labelBoard1f;
            _labels[0, 6] = _labelBoard1g;
            _labels[0, 7] = _labelBoard1h;

            setSquaresLabelsEnabledProporty(false);
            setTimers();
            setComboBoxSpecialMove();

        }

        private void setComboBoxSpecialMove()
        {

            string[] moveTypesNames = Enum.GetNames(typeof(ChessGameLogic.MoveType));

            foreach(string moveName in moveTypesNames)
            {
                if (moveName != MoveType.POWN_PROMOTION.ToString() &&
                    moveName != MoveType.WIN_ON_TIME.ToString())
                    this._comboBoxSpecialMove.Items.Add(moveName);

            }

            //this._comboBoxSpecialMove.Items.AddRange(moveTypesNames);
            this._comboBoxSpecialMove.Text = MoveType.STANDART.ToString();
        }

        private void makeMove(object sender, EventArgs e)
        {
            try
            {
                Button button = sender as Button;
                bool moveFinishedFlag = false;
                if (button != null)
                {
                    moveFinishedFlag = specialMoveButtonCliked();
                }
                else//sender is Label
                {
                    //////
                    /////
                    moveFinishedFlag = squareClicked((Label)sender);
                    /////
                    /////
                }

                if (moveFinishedFlag)
                {
                    if (this._moveResult.MoveSuccessFlag)
                    {
                        if (this._moveResult.NewGameStatus == GameStatus.CONTINUE)
                        {
                            updateUI();
                        }
                        else if (this._moveResult.NewGameStatus == GameStatus.CHECK)
                        {
                            newGameStatusCheck();
                        }
                        else if (this._moveResult.NewGameStatus == GameStatus.CHECK_MATE)
                        {
                            newGameStatusCheckMate();
                        }
                        else if (this._moveResult.NewGameStatus == GameStatus.DRAW_BY_AGREEMENT_WAITING_RESPONSE)
                        {
                            newGameStatusDrowByEgreementWiatingResponse();
                        }
                        else if (this._moveResult.NewGameStatus == GameStatus.DRAW_BY_AGREEMENT)
                        {
                            newGameStatusDrowByEgreement();
                        }
                        else if (this._moveResult.NewGameStatus == GameStatus.DROW_DU_TO_INSUFFICIENT_MATERIAL)
                        {
                            newGameStatusDrowDuTInSufficientMaterial();
                        }
                        else if (this._moveResult.NewGameStatus == GameStatus.RESIGNATION)
                        {
                            newGameStatusResignation();
                        }
                        else if (this._moveResult.NewGameStatus == GameStatus.FORFEIT)
                        {
                            newGameStatusFrofiet();
                        }
                        else if (this._moveResult.NewGameStatus == GameStatus.DROW_STALEMATE)
                        {
                            newGameStatusStalemate();
                        }
                        else if (this._moveResult.NewGameStatus == GameStatus.WIN_ON_TIME)
                        {
                            newGameStatusWinOnTime();
                        }
                    }
                    else
                    {
                        throw this._moveResult.MoveAnalizerResultExeption;
                    }
                }
            }
            catch (ExceptionChessGameLogic ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                initFromToSquare();
            }
        }

        private void newGameStatusDrowDuTInSufficientMaterial()
        {
            gameOver();
            this._labelCurrPlayerNameColor.Text = string.Format("{0}", GameStatus.DROW_DU_TO_INSUFFICIENT_MATERIAL);
            MessageBox.Show(string.Format("{0}", GameStatus.DROW_DU_TO_INSUFFICIENT_MATERIAL.ToString()), "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void newGameStatusWinOnTime()
        {

            updateUI();
            this._labelCurrPlayerNameColor.Text = string.Format("{0} player victorios", this._gameLogic.CurrPlayerColor);
            gameOver();
            MessageBox.Show(string.Format("{0} player {1}", this._gameLogic.CurrPlayerColor, GameStatus.WIN_ON_TIME.ToString()), "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void promotePown(object sender, EventArgs e)
        {
            FormPownPromoted form = new FormPownPromoted(this._gameLogic.CurrPlayerColor);
            form.ShowDialog();

            PieceTypes type = form.ChoosenType;

            MessageBox.Show(type.ToString(), "Chosen type");
        }


        private void newGameStatusStalemate()
        {
            this._labelCurrPlayerNameColor.Text = string.Format("{0}", this._moveResult.NewGameStatus);
            gameOver();
            MessageBox.Show(string.Format("{0}", this._moveResult.NewGameStatus), "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void newGameStatusFrofiet()
        {
            this._labelCurrPlayerNameColor.Text = string.Format("{0} player won by {1}", this._gameLogic.CurrPlayerColor, this._moveResult.NewGameStatus);
            gameOver();
            MessageBox.Show(string.Format("{0}", this._moveResult.NewGameStatus), "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void newGameStatusResignation()
        {
            this._labelCurrPlayerNameColor.Text = string.Format("{0} player won by {1}", this._gameLogic.CurrPlayerColor, this._moveResult.NewGameStatus);
            gameOver();
            MessageBox.Show(string.Format("{0}", this._moveResult.NewGameStatus), "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void newGameStatusDrowByEgreement()
        {
            this._labelCurrPlayerNameColor.Text = string.Format("{0}", this._moveResult.NewGameStatus);
            gameOver();
            MessageBox.Show(string.Format("{0}", this._moveResult.NewGameStatus), "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void gameOver()
        {
            this._timerBlackPlayer.Enabled = false;
            this._timerWhitePlayer.Enabled = false;
        }

        private void newGameStatusDrowByEgreementWiatingResponse()
        {
            updateUI();
            this._labelCurrPlayerNameColor.Text += string.Format("\n{0}", this._moveResult.NewGameStatus);

        }

        private void newGameStatusCheckMate()
        {
            updateUI();
            this._labelCurrPlayerNameColor.Text = string.Format("{0} player victorios", this._gameLogic.CurrPlayerColor);
            gameOver();
            MessageBox.Show(string.Format("{0} player in CheckMate", ChessGameUtility.GetRivalColor(this._gameLogic.CurrPlayerColor)), "CheckMate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void newGameStatusCheck()
        {
            updateUI();
            this._labelCurrPlayerNameColor.Text = "Check!\n" + this._labelCurrPlayerNameColor.Text;
            MessageBox.Show(string.Format("{0} player in check", this._gameLogic.CurrPlayerColor), "Check", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }

        private bool squareClicked(Label squareLabel)
        {
            bool moveFinisedFlag;

            if (this._squareClickedCounter == 0)
            {
                this._fromSquare += "" + squareLabel.Name[squareLabel.Name.Length - 1] + squareLabel.Name[squareLabel.Name.Length - 2];

                //this._moves = this._gameLogic.GetPieceValidMoves(ChessGameUtility.GetRow(this._fromSquare[1]), ChessGameUtility.GetColumn(this._fromSquare[0]));
                //this._moves = this._gameLogic.GetPieceThretendSqures(ChessGameUtility.GetRow(this._fromSquare[1]), ChessGameUtility.GetColumn(this._fromSquare[0]));
                //for (int i = 0; i < this._moves.Count; i += 2)
                //{
                //    MarkDeMarkSquare(this._moves[i] - 1, this._moves[i + 1] - 1, true);
                //}
                MarkDeMarkSquare(this._fromSquare, true);

                this._squareClickedCounter++;
                moveFinisedFlag = false;
            }
            else//this._squareClickedCounter == 1
            {
                this._toSquare += "" + squareLabel.Name[squareLabel.Name.Length - 1] + squareLabel.Name[squareLabel.Name.Length - 2];

                Move move = getMove();
                //this._moveResult = this._gameLogic.MakeMove(new Move(MoveType.STANDART, this._fromSquare, this._toSquare));

                /////
                ////
                this._moveResult = this._gameLogic.MakeMove(move);
                this._movesToFile.Add("\"" + move.ToString() + "\",");
                /////
                ////

                //for (int i = 0; i < this._moves.Count; i += 2)
                //{
                //    MarkDeMarkSquare(this._moves[i] - 1, this._moves[i + 1] - 1, false);
                //}
                MarkDeMarkSquare(this._fromSquare, false);

                this._squareClickedCounter = 0;

                initFromToSquare();

                moveFinisedFlag = true;
            }

            return moveFinisedFlag;
        }

        private Move getMove()
        {
            Move move;
            PieceTypes pieceType;
            FormPownPromoted form = null;

            int row = ChessGameUtility.GetRow(this._toSquare[1]);
            if (row == 8)
            {
                pieceType = this._gameLogic.GetPieceTypeOnAsquare(this._fromSquare[1], this._fromSquare[0]);
                if (pieceType == PieceTypes.WHITE_POWN)
                {
                    form = new FormPownPromoted(ChessGameEntityColor.WHITE);
                }
            }
            else if (row == 1)
            {
                pieceType = this._gameLogic.GetPieceTypeOnAsquare(this._fromSquare[1], this._fromSquare[0]);
                if (pieceType == PieceTypes.BLACK_POWN)
                {
                    form = new FormPownPromoted(ChessGameEntityColor.BLACK);
                }
            }

            if (form != null)
            {
                form.ShowDialog();
                pieceType = form.ChoosenType;
                move = new Move(MoveType.POWN_PROMOTION, this._fromSquare, this._toSquare, pieceType);
            }
            else
            {
                move = new Move(MoveType.STANDART, this._fromSquare, this._toSquare, PieceTypes.NOUN);
            }

            return move;
        }

        private bool specialMoveButtonCliked()
        {
            MoveType moveType = (MoveType)Enum.Parse(typeof(MoveType), this._comboBoxSpecialMove.Text);
            if (moveType != MoveType.STANDART)
            {
                this._moveResult = this._gameLogic.MakeMove(new Move(moveType, null, null, PieceTypes.NOUN));
            }
            else
            {
                MessageBox.Show(string.Format("Special move con't be {0}", moveType.ToString()), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this._comboBoxSpecialMove.Text = MoveType.STANDART.ToString();
            return true;
        }

        private void MarkDeMarkSquare(string square, bool mark)
        {
            int rowInd = ChessGameUtility.GetRow(square[1]) - 1;
            int columnInd = ChessGameUtility.GetColumn(square[0]) - 1;

            MarkDeMarkSquare(rowInd, columnInd, mark);
        }
        private void MarkDeMarkSquare(int rowInd, int columnInd, bool mark)
        {
            if (mark)
                this._labels[rowInd, columnInd].BorderStyle = BorderStyle.FixedSingle;
            else
                this._labels[rowInd, columnInd].BorderStyle = BorderStyle.None;
        }

        private void buttonStartNewGame_Click(object sender, EventArgs e)
        {
            this._gameLogic = new Game();
            this._buttonSaveToFile.Enabled = true;
            this._textBoxGameId.Enabled = true;
            this._movesToFile.Clear();
            setTimers();
            this._timerWhitePlayer.Enabled = true; ;
            this._timerBlackPlayer.Enabled = false; ;


            this._buttonSpecialMove.Enabled = true;
            this._buttonMakeNextMoveDebug.Enabled = true;
            this._comboBoxSpecialMove.Enabled = true;

            this._labelWhitePlayerTimerShower.Text = "White PlayerTimer : " + this._whiteTimerCounter.ToString();
            this._labelBlackPlayerTimerShower.Text = "Black PlayerTimer : " + this._blackTimerCounter.ToString();

            this._timerWhitePlayer.Enabled = true;

            this._buttonMakeNextMoveDebug.Enabled = true;
            this._movesForDebugCounter = 0;
            this._labelAautoTurnNumberMsg.Text = "Move Number:";
            this._labelAutoTurnNumberShower.Text = "0";


            setSquaresLabelsEnabledProporty(true);

            printBoard();
            updateCurrPlayerLabel();
        }

        private void buttonEndGame_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Shure?", "End Game", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();
        }

        private void setSquaresLabelsEnabledProporty(bool valueToSet)
        {
            for (int i = 0; i < this._labels.GetLength(0); i++)
            {
                for (int j = 0; j < this._labels.GetLength(1); j++)
                {
                    this._labels[i, j].Enabled = valueToSet;
                }
            }
        }

        private void printBoard()
        {
            Square[,] squares = this._gameLogic.BoradOfTheGame.Squares;
            Piece piece;
            for (int i = 0; i < squares.GetLength(0); i++)
            {
                for (int j = 0; j < squares.GetLength(1); j++)
                {
                    piece = squares[i, j].PieceOnASquare;
                    if (piece != null)
                        this._labels[i, j].Text = getUtfChessPieceRepresantationAsAString(piece.PieceType);
                    else
                        this._labels[i, j].Text = "";
                }
            }
        }

        private string getUtfChessPieceRepresantationAsAString(PieceTypes pieceType)
        {
            string retVal = "";

            switch (pieceType)
            {
                case PieceTypes.WHITE_KING:
                    retVal = "♔";
                    break;
                case PieceTypes.WHITE_QUEEN:
                    retVal = "♕"; ;
                    break;
                case PieceTypes.WHITE_ROOK:
                    retVal = "♖"; ;
                    break;
                case PieceTypes.WHITE_BISHOP:
                    retVal = "♗";
                    break;
                case PieceTypes.WHITE_KNIGHT:
                    retVal = "♘";
                    break;
                case PieceTypes.WHITE_POWN:
                    retVal = "♙";
                    break;
                case PieceTypes.BLACK_KING:
                    retVal = "♚";
                    break;
                case PieceTypes.BLACK_QUEEN:
                    retVal = "♛"; ;
                    break;
                case PieceTypes.BLACK_ROOK:
                    retVal = "♜"; ;
                    break;
                case PieceTypes.BLACK_BISHOP:
                    retVal = "♝";
                    break;
                case PieceTypes.BLACK_KNIGHT:
                    retVal = "♞";
                    break;
                case PieceTypes.BLACK_POWN:
                    retVal = "♟";
                    break;
            }

            return retVal;
        }

        private void updateUI()
        {
            printBoard();
            updateCurrPlayerLabel();
            updateCurrPlayerTimer();
        }

        private void updateCurrPlayerLabel()
        {
            this._labelCurrPlayerNameColor.Text = string.Format("Curr Player : {0}", this._gameLogic.CurrPlayerColor);
        }

        private void initFromToSquare()
        {
            this._toSquare = this._fromSquare = "";
        }

        private void setTimers()
        {
            this._blackTimerCounter = NUMBER_OF_SECONDS_IN_A_MINUTE * NUMBER_OF_MINUTES_PER_GAME;
            this._whiteTimerCounter = NUMBER_OF_SECONDS_IN_A_MINUTE * NUMBER_OF_MINUTES_PER_GAME;
        }
        private void timerWhitePlayer_Tick(object sender, EventArgs e)
        {
            this._whiteTimerCounter--;
            this._labelWhitePlayerTimerShower.Text = "White PlayerTimer : " + this._whiteTimerCounter.ToString();
            if (this._whiteTimerCounter == 0)
                gameOverWinOnTome();
        }
        private void _timerBlackPlayer_Tick(object sender, EventArgs e)
        {
            this._blackTimerCounter--;
            this._labelBlackPlayerTimerShower.Text = "Black PlayerTimer : " + this._blackTimerCounter.ToString();
            if(this._blackTimerCounter == 0)
                gameOverWinOnTome();
        }

        private void gameOverWinOnTome()
        {
            this._comboBoxSpecialMove.Text = MoveType.WIN_ON_TIME.ToString();
            makeMove(this._buttonSpecialMove, null);
        }

        private void updateCurrPlayerTimer()
        {
            if (this._timerWhitePlayer.Enabled)//curr timer is white
            {
                this._timerWhitePlayer.Enabled = false;
                this._timerBlackPlayer.Enabled = true;
            }
            else//curr timer is black
            {
                this._timerWhitePlayer.Enabled = true;
                this._timerBlackPlayer.Enabled = false;
            }
        }

        private void buttonMakeNextMoveDebug_Click(object sender, EventArgs e)
        {
            if (this._movesForDebugCounter == this._stopAutuMoveForDebugInMoveNumber - 1)
            {
                int x = 19;
            }
            try
            {
                string currMove = this._movesForDebug[this._movesForDebugCounter];
                string nextMove = this._movesForDebugCounter < this._movesForDebug.Count - 1 ? this._movesForDebug[this._movesForDebugCounter + 1] : " NO MORE AUTO MOVES";
                this._labelAautoTurnNumberMsg.Text = "Move : " + currMove + "\nNext Move : " + nextMove;
                this._movesForDebugCounter++;
                this._labelAutoTurnNumberShower.Text = this._movesForDebugCounter.ToString();

                int row = ChessGameUtility.GetRow(currMove[1]) - 1;
                int column = ChessGameUtility.GetColumn(currMove[0]) - 1;
                Label senderLabel = this._labels[row, column];
                makeMove(senderLabel, null);

                row = ChessGameUtility.GetRow(currMove[3]) - 1;
                column = ChessGameUtility.GetColumn(currMove[2]) - 1;
                senderLabel = this._labels[row, column];
                /////
                ////
                makeMove(senderLabel, null);
                //////
                /////

                //  MessageBox.Show("auto move number " + this._movesForFebugCounter + " move " + currMove, "Move for debug");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Move for debug Error");

            }
        }

        private void buttonSaveToFile(object sender, EventArgs e)
        {
            string fileName = this._textBoxGameId.Text;
            if(fileName == "")
            {
                MessageBox.Show("File Name Is Not Entered.\nFile Not Saved", "File Save Error");
                return;
            }
            string path = @"C:\Users\Peter\Desktop\ChessGameFinalVersion\Games\";

            string pathAndFileName = string.Format(@"{0}{1}.txt", path, fileName);
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(pathAndFileName))
            {
                foreach (string line in this._movesToFile.ToArray())
                {
                    file.WriteLine(line);
                }
            }

            MessageBox.Show("File Saved.");
        }
    }
}
