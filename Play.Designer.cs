namespace Igrica
{
    partial class Play
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
            this.components = new System.ComponentModel.Container();
            this.nosTimer = new System.Windows.Forms.Timer(this.components);
            this.comboTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // nosTimer
            // 
            this.nosTimer.Enabled = true;
            this.nosTimer.Interval = 1000;
            this.nosTimer.Tick += new System.EventHandler(this.nosTimer_Tick);
            // 
            // comboTimer
            // 
            this.comboTimer.Interval = 300;
            this.comboTimer.Tick += new System.EventHandler(this.comboTimer_Tick);
            // 
            // Play
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 498);
            this.Name = "Play";
            this.Text = "Play";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Play_FormClosing);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Play_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Play_MouseClick);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Play_MouseDoubleClick);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer nosTimer;
        private System.Windows.Forms.Timer comboTimer;
    }
}