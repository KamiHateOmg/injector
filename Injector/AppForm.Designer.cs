namespace Injector
{
    partial class AppForm
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
            this.injectButton1 = new System.Windows.Forms.Button();
            this.selectButton1 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.status1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // injectButton1
            // 
            this.injectButton1.Location = new System.Drawing.Point(347, 346);
            this.injectButton1.Name = "injectButton1";
            this.injectButton1.Size = new System.Drawing.Size(75, 23);
            this.injectButton1.TabIndex = 0;
            this.injectButton1.Text = "inject";
            this.injectButton1.UseVisualStyleBackColor = true;
            this.injectButton1.Click += new System.EventHandler(this.injectButton1_Click);
            // 
            // selectButton1
            // 
            this.selectButton1.Location = new System.Drawing.Point(347, 317);
            this.selectButton1.Name = "selectButton1";
            this.selectButton1.Size = new System.Drawing.Size(75, 23);
            this.selectButton1.TabIndex = 1;
            this.selectButton1.Text = "select";
            this.selectButton1.UseVisualStyleBackColor = true;
            this.selectButton1.Click += new System.EventHandler(this.selectButton1_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(174, 375);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(421, 14);
            this.progressBar1.TabIndex = 2;
            // 
            // status1
            // 
            this.status1.AutoSize = true;
            this.status1.Location = new System.Drawing.Point(363, 283);
            this.status1.Name = "status1";
            this.status1.Size = new System.Drawing.Size(41, 13);
            this.status1.TabIndex = 3;
            this.status1.Text = "status1";
            // 
            // AppForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.status1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.selectButton1);
            this.Controls.Add(this.injectButton1);
            this.Name = "AppForm";
            this.Text = "AppForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button injectButton1;
        private System.Windows.Forms.Button selectButton1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label status1;
    }
}