using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChessGameLogic;

namespace ChessGameFinalVersion
{
    public partial class FormPownPromoted : Form
    {
        private ChessGameEntityColor _playerColor;
        private PieceTypes _chosenType;
        private Label _labelCchosen = null;

        public PieceTypes ChoosenType
        {
            get
            {
                return this._chosenType;
            }
        }
        public FormPownPromoted(ChessGameEntityColor playerColor)
        {
            InitializeComponent();

            this._playerColor = playerColor;

            setLabelsChoise();
        }

        private void setLabelsChoise()
        {
            if (this._playerColor == ChessGameEntityColor.WHITE)
            {
                this._labelChoiseQueen.Text = getUtfChessPieceRepresantationAsAString(PieceTypes.WHITE_QUEEN);
                this._labelChoiseRook.Text = getUtfChessPieceRepresantationAsAString(PieceTypes.WHITE_ROOK);
                this._labelChoiseKnight.Text = getUtfChessPieceRepresantationAsAString(PieceTypes.WHITE_KNIGHT);
            }
            else
            {
                this._labelChoiseQueen.Text = getUtfChessPieceRepresantationAsAString(PieceTypes.BLACK_QUEEN);
                this._labelChoiseRook.Text = getUtfChessPieceRepresantationAsAString(PieceTypes.BLACK_ROOK);
                this._labelChoiseKnight.Text = getUtfChessPieceRepresantationAsAString(PieceTypes.BLACK_KNIGHT);
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

        private void ButtonMakeChoise_Click(object sender, EventArgs e)
        {
            if(this._labelCchosen != null)
            {
                //MessageBox.Show(this._chosenType.ToString(), "Type Chossen");
                this.Close();
            }
            else
            {
                MessageBox.Show("Choose Piece", "No Choise Made");
            }
        }

        private void labelChoise_Click(object sender, EventArgs e)
        {
            if (this._labelCchosen != null)
                this._labelCchosen.BorderStyle = BorderStyle.None;
            this._labelCchosen = (Label)sender;
            this._labelCchosen.BorderStyle = BorderStyle.FixedSingle; 
            if(this._playerColor == ChessGameEntityColor.WHITE)
            {
                if (this._labelCchosen.Name == "_labelChoiseQueen")
                    this._chosenType = PieceTypes.WHITE_QUEEN;
                else if(this._labelCchosen.Name == "_labelChoiseRook")
                    this._chosenType = PieceTypes.WHITE_ROOK;
                else
                    this._chosenType = PieceTypes.WHITE_KNIGHT;
            }
            else
            {
                if (this._labelCchosen.Name == "_labelChoiseQueen")
                    this._chosenType = PieceTypes.BLACK_QUEEN;
                else if (this._labelCchosen.Name == "_labelChoiseRook")
                    this._chosenType = PieceTypes.BLACK_ROOK;
                else
                    this._chosenType = PieceTypes.BLACK_KNIGHT;
            }
        }
    }
}
