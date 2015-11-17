namespace TestAutoit
{
    partial class Form1
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
            this.buttonCode = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonRetry = new System.Windows.Forms.Button();
            this.buttonGetXY = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonGuessXY = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonCode
            // 
            this.buttonCode.Location = new System.Drawing.Point(12, 60);
            this.buttonCode.Name = "buttonCode";
            this.buttonCode.Size = new System.Drawing.Size(75, 23);
            this.buttonCode.TabIndex = 1;
            this.buttonCode.Text = "Code";
            this.buttonCode.UseVisualStyleBackColor = true;
            this.buttonCode.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(13, 151);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 2;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonRetry
            // 
            this.buttonRetry.Location = new System.Drawing.Point(94, 151);
            this.buttonRetry.Name = "buttonRetry";
            this.buttonRetry.Size = new System.Drawing.Size(75, 23);
            this.buttonRetry.TabIndex = 3;
            this.buttonRetry.Text = "Retry";
            this.buttonRetry.UseVisualStyleBackColor = true;
            this.buttonRetry.Click += new System.EventHandler(this.buttonRetry_Click);
            // 
            // buttonGetXY
            // 
            this.buttonGetXY.Location = new System.Drawing.Point(13, 13);
            this.buttonGetXY.Name = "buttonGetXY";
            this.buttonGetXY.Size = new System.Drawing.Size(75, 23);
            this.buttonGetXY.TabIndex = 4;
            this.buttonGetXY.Text = "GetXY";
            this.buttonGetXY.UseVisualStyleBackColor = true;
            this.buttonGetXY.Click += new System.EventHandler(this.buttonGetXY_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(94, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 5;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(120, 60);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonGuessXY
            // 
            this.buttonGuessXY.Location = new System.Drawing.Point(13, 107);
            this.buttonGuessXY.Name = "buttonGuessXY";
            this.buttonGuessXY.Size = new System.Drawing.Size(75, 23);
            this.buttonGuessXY.TabIndex = 7;
            this.buttonGuessXY.Text = "Guess XY";
            this.buttonGuessXY.UseVisualStyleBackColor = true;
            this.buttonGuessXY.Click += new System.EventHandler(this.buttonGuessXY_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.buttonGuessXY);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonGetXY);
            this.Controls.Add(this.buttonRetry);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.buttonCode);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCode;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonRetry;
        private System.Windows.Forms.Button buttonGetXY;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonGuessXY;
    }
}

