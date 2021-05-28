namespace ComharDesign.Command
{
    partial class ExcelImport_Form
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
            this.Cancel = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.xlstbox = new System.Windows.Forms.TextBox();
            this.browserbtn = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(428, 89);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 5;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(347, 89);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 4;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // xlstbox
            // 
            this.xlstbox.Location = new System.Drawing.Point(88, 37);
            this.xlstbox.Name = "xlstbox";
            this.xlstbox.Size = new System.Drawing.Size(415, 20);
            this.xlstbox.TabIndex = 6;
            // 
            // browserbtn
            // 
            this.browserbtn.Location = new System.Drawing.Point(7, 35);
            this.browserbtn.Name = "browserbtn";
            this.browserbtn.Size = new System.Drawing.Size(75, 23);
            this.browserbtn.TabIndex = 7;
            this.browserbtn.Text = "Browse";
            this.browserbtn.UseVisualStyleBackColor = true;
            this.browserbtn.Click += new System.EventHandler(this.browserbtn_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // ExcelImport_Form
            // 
            this.AcceptButton = this.OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(515, 124);
            this.Controls.Add(this.browserbtn);
            this.Controls.Add(this.xlstbox);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Name = "ExcelImport_Form";
            this.Text = "Choose the Excel file";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.TextBox xlstbox;
        private System.Windows.Forms.Button browserbtn;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}