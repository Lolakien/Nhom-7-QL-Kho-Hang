namespace QL_KhoHang.UserControls
{
    partial class Section
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Section));
            this.lbTenSP = new System.Windows.Forms.Label();
            this.percentBar = new Bunifu.UI.WinForms.BunifuProgressBar();
            this.SuspendLayout();
            // 
            // lbTenSP
            // 
            this.lbTenSP.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTenSP.Location = new System.Drawing.Point(12, 18);
            this.lbTenSP.Name = "lbTenSP";
            this.lbTenSP.Size = new System.Drawing.Size(175, 23);
            this.lbTenSP.TabIndex = 0;
            this.lbTenSP.Text = "label1";
            // 
            // percentBar
            // 
            this.percentBar.AllowAnimations = false;
            this.percentBar.Animation = 0;
            this.percentBar.AnimationSpeed = 220;
            this.percentBar.AnimationStep = 10;
            this.percentBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.percentBar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("percentBar.BackgroundImage")));
            this.percentBar.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.percentBar.BorderRadius = 9;
            this.percentBar.BorderThickness = 1;
            this.percentBar.Location = new System.Drawing.Point(193, 28);
            this.percentBar.Maximum = 100;
            this.percentBar.MaximumValue = 100;
            this.percentBar.Minimum = 0;
            this.percentBar.MinimumValue = 0;
            this.percentBar.Name = "percentBar";
            this.percentBar.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.percentBar.ProgressBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.percentBar.ProgressColorLeft = System.Drawing.Color.DodgerBlue;
            this.percentBar.ProgressColorRight = System.Drawing.Color.DodgerBlue;
            this.percentBar.Size = new System.Drawing.Size(118, 13);
            this.percentBar.TabIndex = 1;
            this.percentBar.Value = 75;
            this.percentBar.ValueByTransition = 75;
            // 
            // Section
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.percentBar);
            this.Controls.Add(this.lbTenSP);
            this.Name = "Section";
            this.Size = new System.Drawing.Size(388, 61);
            this.Leave += new System.EventHandler(this.Section_Leave);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbTenSP;
        private Bunifu.UI.WinForms.BunifuProgressBar percentBar;
    }
}
