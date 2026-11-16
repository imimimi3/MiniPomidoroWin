namespace MiniPomidoroWin.Forms
{
	partial class MiniTimerForm
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
			circularTimer = new MiniPomidoroWin.Controls.CircularTimer();
			btnPause = new MiniPomidoroWin.Controls.ModernButton();
			btnStop = new MiniPomidoroWin.Controls.ModernButton();
			SuspendLayout();
			// 
			// circularTimer
			// 
			circularTimer.Location = new Point(10, 10);
			circularTimer.Name = "circularTimer";
			circularTimer.Size = new Size(190, 190);
			circularTimer.TabIndex = 0;
			circularTimer.Text = "circularTimer1";
			circularTimer.Thickness = 10;
			// 
			// btnPause
			// 
			btnPause.BackColor = Color.FromArgb(153, 102, 204);
			btnPause.BorderRadius = 24;
			btnPause.FlatStyle = FlatStyle.Flat;
			btnPause.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
			btnPause.ForeColor = Color.White;
			btnPause.Location = new Point(216, 41);
			btnPause.Name = "btnPause";
			btnPause.Size = new Size(48, 48);
			btnPause.TabIndex = 1;
			btnPause.Text = "⏸";
			btnPause.UseVisualStyleBackColor = false;
			btnPause.Click += btnPause_Click;
			// 
			// btnStop
			// 
			btnStop.BackColor = Color.FromArgb(153, 102, 204);
			btnStop.FlatStyle = FlatStyle.Flat;
			btnStop.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
			btnStop.ForeColor = Color.White;
			btnStop.Location = new Point(210, 109);
			btnStop.Name = "btnStop";
			btnStop.Size = new Size(60, 60);
			btnStop.TabIndex = 2;
			btnStop.Text = "⏹";
			btnStop.UseVisualStyleBackColor = false;
			btnStop.Click += btnStop_Click;
			// 
			// MiniTimerForm
			// 
			AutoScaleDimensions = new SizeF(13F, 32F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = SystemColors.ControlDark;
			ClientSize = new Size(280, 210);
			Controls.Add(btnStop);
			Controls.Add(btnPause);
			Controls.Add(circularTimer);
			FormBorderStyle = FormBorderStyle.None;
			Name = "MiniTimerForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.Manual;
			Text = "MiniPomidoro";
			TopMost = true;
			ResumeLayout(false);
		}

		#endregion

		private Controls.CircularTimer circularTimer;
		private Controls.ModernButton btnPause;
		private Controls.ModernButton btnStop;
	}
}