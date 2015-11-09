namespace Net4WinFormsDemo
{
    partial class MainForm
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
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCharge = new System.Windows.Forms.Button();
            this.messageTextBlock = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtAmount
            // 
            this.txtAmount.Location = new System.Drawing.Point(148, 87);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(187, 20);
            this.txtAmount.TabIndex = 0;
            this.txtAmount.Text = "1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(218, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Amount";
            // 
            // btnCharge
            // 
            this.btnCharge.Location = new System.Drawing.Point(209, 124);
            this.btnCharge.Name = "btnCharge";
            this.btnCharge.Size = new System.Drawing.Size(75, 23);
            this.btnCharge.TabIndex = 2;
            this.btnCharge.Text = "Charge";
            this.btnCharge.UseVisualStyleBackColor = true;
            this.btnCharge.Click += new System.EventHandler(this.btnCharge_Click);
            // 
            // messageTextBlock
            // 
            this.messageTextBlock.AutoSize = true;
            this.messageTextBlock.Location = new System.Drawing.Point(116, 172);
            this.messageTextBlock.Name = "messageTextBlock";
            this.messageTextBlock.Size = new System.Drawing.Size(62, 13);
            this.messageTextBlock.TabIndex = 3;
            this.messageTextBlock.Text = "SDK Status";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 261);
            this.Controls.Add(this.messageTextBlock);
            this.Controls.Add(this.btnCharge);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtAmount);
            this.Name = "MainForm";
            this.Text = "Win Forms Net4 RetailSDK";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCharge;
        private System.Windows.Forms.Label messageTextBlock;
    }
}

