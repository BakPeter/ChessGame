namespace ChessGameFinalVersion
{
    partial class FormPownPromoted
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._labelChoiseRook = new System.Windows.Forms.Label();
            this._labelChoiseQueen = new System.Windows.Forms.Label();
            this._labelChoiseKnight = new System.Windows.Forms.Label();
            this.buttonMakeChoise = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _labelChoiseRook
            // 
            this._labelChoiseRook.BackColor = System.Drawing.Color.Silver;
            this._labelChoiseRook.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this._labelChoiseRook.Location = new System.Drawing.Point(68, 9);
            this._labelChoiseRook.Name = "_labelChoiseRook";
            this._labelChoiseRook.Size = new System.Drawing.Size(50, 50);
            this._labelChoiseRook.TabIndex = 186;
            this._labelChoiseRook.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._labelChoiseRook.Click += new System.EventHandler(this.labelChoise_Click);
            // 
            // _labelChoiseQueen
            // 
            this._labelChoiseQueen.BackColor = System.Drawing.Color.WhiteSmoke;
            this._labelChoiseQueen.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this._labelChoiseQueen.Location = new System.Drawing.Point(12, 9);
            this._labelChoiseQueen.Name = "_labelChoiseQueen";
            this._labelChoiseQueen.Size = new System.Drawing.Size(50, 50);
            this._labelChoiseQueen.TabIndex = 185;
            this._labelChoiseQueen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._labelChoiseQueen.Click += new System.EventHandler(this.labelChoise_Click);
            // 
            // _labelChoiseKnight
            // 
            this._labelChoiseKnight.BackColor = System.Drawing.Color.WhiteSmoke;
            this._labelChoiseKnight.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this._labelChoiseKnight.Location = new System.Drawing.Point(124, 9);
            this._labelChoiseKnight.Name = "_labelChoiseKnight";
            this._labelChoiseKnight.Size = new System.Drawing.Size(50, 50);
            this._labelChoiseKnight.TabIndex = 189;
            this._labelChoiseKnight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._labelChoiseKnight.Click += new System.EventHandler(this.labelChoise_Click);
            // 
            // buttonMakeChoise
            // 
            this.buttonMakeChoise.Location = new System.Drawing.Point(12, 83);
            this.buttonMakeChoise.Name = "buttonMakeChoise";
            this.buttonMakeChoise.Size = new System.Drawing.Size(162, 27);
            this.buttonMakeChoise.TabIndex = 190;
            this.buttonMakeChoise.Text = "Choose Pown Promotion";
            this.buttonMakeChoise.UseVisualStyleBackColor = true;
            this.buttonMakeChoise.Click += new System.EventHandler(this.ButtonMakeChoise_Click);
            // 
            // FormPownPromoted
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(190, 122);
            this.ControlBox = false;
            this.Controls.Add(this.buttonMakeChoise);
            this.Controls.Add(this._labelChoiseKnight);
            this.Controls.Add(this._labelChoiseRook);
            this.Controls.Add(this._labelChoiseQueen);
            this.Name = "FormPownPromoted";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PownPromoted";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label _labelChoiseRook;
        private System.Windows.Forms.Label _labelChoiseQueen;
        private System.Windows.Forms.Label _labelChoiseKnight;
        private System.Windows.Forms.Button buttonMakeChoise;
    }
}