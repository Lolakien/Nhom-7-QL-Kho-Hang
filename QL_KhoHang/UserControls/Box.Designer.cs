namespace QL_KhoHang
{
    partial class Box
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnRight = new System.Windows.Forms.Panel();
            this.pnTop = new System.Windows.Forms.Panel();
            this.pnBottom = new System.Windows.Forms.Panel();
            this.pnLeft = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnRight
            // 
            this.pnRight.BackColor = System.Drawing.SystemColors.Control;
            this.pnRight.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnRight.Location = new System.Drawing.Point(0, 0);
            this.pnRight.Name = "pnRight";
            this.pnRight.Size = new System.Drawing.Size(5, 150);
            this.pnRight.TabIndex = 0;
            // 
            // pnTop
            // 
            this.pnTop.BackColor = System.Drawing.SystemColors.Control;
            this.pnTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnTop.Location = new System.Drawing.Point(5, 0);
            this.pnTop.Name = "pnTop";
            this.pnTop.Size = new System.Drawing.Size(145, 5);
            this.pnTop.TabIndex = 1;
            // 
            // pnBottom
            // 
            this.pnBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnBottom.Location = new System.Drawing.Point(5, 145);
            this.pnBottom.Name = "pnBottom";
            this.pnBottom.Size = new System.Drawing.Size(145, 5);
            this.pnBottom.TabIndex = 2;
            // 
            // pnLeft
            // 
            this.pnLeft.BackColor = System.Drawing.SystemColors.Control;
            this.pnLeft.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnLeft.Location = new System.Drawing.Point(145, 5);
            this.pnLeft.Name = "pnLeft";
            this.pnLeft.Size = new System.Drawing.Size(5, 140);
            this.pnLeft.TabIndex = 3;
            // 
            // Box
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnLeft);
            this.Controls.Add(this.pnBottom);
            this.Controls.Add(this.pnTop);
            this.Controls.Add(this.pnRight);
            this.Name = "Box";
            this.Load += new System.EventHandler(this.Box_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnRight;
        private System.Windows.Forms.Panel pnTop;
        private System.Windows.Forms.Panel pnBottom;
        private System.Windows.Forms.Panel pnLeft;




    }
}
