namespace WIndowsForms_HW4_Synchronization_events_critical_sections
{
    partial class Form_Bus_Stop
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
            this.buttonAddPassengers = new System.Windows.Forms.Button();
            this.listBoxLog = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // buttonAddPassengers
            // 
            this.buttonAddPassengers.Location = new System.Drawing.Point(9, 10);
            this.buttonAddPassengers.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonAddPassengers.Name = "buttonAddPassengers";
            this.buttonAddPassengers.Size = new System.Drawing.Size(90, 24);
            this.buttonAddPassengers.TabIndex = 2;
            this.buttonAddPassengers.Text = "Add Passengers";
            this.buttonAddPassengers.UseVisualStyleBackColor = true;
            this.buttonAddPassengers.Click += new System.EventHandler(this.buttonAddPassengers_Click);
            // 
            // listBoxLog
            // 
            this.listBoxLog.FormattingEnabled = true;
            this.listBoxLog.Location = new System.Drawing.Point(9, 39);
            this.listBoxLog.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listBoxLog.Name = "listBoxLog";
            this.listBoxLog.Size = new System.Drawing.Size(600, 300);
            this.listBoxLog.TabIndex = 1;
            // 
            // Form_Bus_Stop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.listBoxLog);
            this.Controls.Add(this.buttonAddPassengers);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form_Bus_Stop";
            this.Text = "Bus Stop Simulation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormBusStop_FormClosing);
            this.Load += new System.EventHandler(this.FormBusStop_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonAddPassengers;
        private System.Windows.Forms.ListBox listBoxLog;
    }
}