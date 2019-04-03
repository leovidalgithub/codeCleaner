namespace codeCleanerForm.GUI
{
    partial class codeCleanerWinForm
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
            this.btnReadDatabase = new System.Windows.Forms.Button();
            this.DGV = new System.Windows.Forms.DataGridView();
            this.btnReadDirectory = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
            this.SuspendLayout();
            // 
            // btnReadDatabase
            // 
            this.btnReadDatabase.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnReadDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReadDatabase.Location = new System.Drawing.Point(363, 330);
            this.btnReadDatabase.Name = "btnReadDatabase";
            this.btnReadDatabase.Size = new System.Drawing.Size(199, 83);
            this.btnReadDatabase.TabIndex = 6;
            this.btnReadDatabase.Text = "READ DATABASE";
            this.btnReadDatabase.UseVisualStyleBackColor = true;
            // 
            // DGV
            // 
            this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV.Location = new System.Drawing.Point(12, 38);
            this.DGV.Name = "DGV";
            this.DGV.Size = new System.Drawing.Size(776, 276);
            this.DGV.TabIndex = 7;
            // 
            // btnReadDirectory
            // 
            this.btnReadDirectory.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnReadDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReadDirectory.Location = new System.Drawing.Point(158, 330);
            this.btnReadDirectory.Name = "btnReadDirectory";
            this.btnReadDirectory.Size = new System.Drawing.Size(199, 83);
            this.btnReadDirectory.TabIndex = 5;
            this.btnReadDirectory.Text = "READ DIRECTORY";
            this.btnReadDirectory.UseVisualStyleBackColor = true;
            this.btnReadDirectory.Click += new System.EventHandler(this.btnReadDirectory_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.SystemColors.Control;
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.FlatAppearance.BorderSize = 2;
            this.btnExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnExit.Location = new System.Drawing.Point(708, 357);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(80, 56);
            this.btnExit.TabIndex = 8;
            this.btnExit.Text = "&Exit <ESC>";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(158, 13);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(436, 20);
            this.textBox1.TabIndex = 9;
            // 
            // codeCleanerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnReadDatabase);
            this.Controls.Add(this.DGV);
            this.Controls.Add(this.btnReadDirectory);
            this.Controls.Add(this.btnExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "codeCleanerForm";
            this.Opacity = 0D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Code Cleaner v.0.014 - AP 2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.codeCleanerForm_FormClosing);
            this.Load += new System.EventHandler(this.codeCleanerForm_Load);
            this.Shown += new System.EventHandler(this.codeCleanerForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnReadDatabase;
        private System.Windows.Forms.DataGridView DGV;
        private System.Windows.Forms.Button btnReadDirectory;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox textBox1;
    }
}

