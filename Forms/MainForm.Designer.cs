using MiniPomidoroWin.Controls;

namespace MiniPomidoroWin
{
	partial class MainForm
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			notifyIcon1 = new NotifyIcon(components);
			minutesNum = new NumericUpDown();
			secondsNum = new NumericUpDown();
			lblMinutes = new Label();
			lblSeconds = new Label();
			circularTimer = new CircularTimer();
			btnSettings = new Button();
			btnPlayPause = new ModernButton();
			btnTimerStop = new ModernButton();
			btnInfo = new Button();
			((System.ComponentModel.ISupportInitialize)minutesNum).BeginInit();
			((System.ComponentModel.ISupportInitialize)secondsNum).BeginInit();
			SuspendLayout();
			// 
			// notifyIcon1
			// 
			notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
			notifyIcon1.Text = "MiniMiniPomidoro";
			notifyIcon1.Visible = true;
			// 
			// minutesNum
			// 
			minutesNum.Location = new Point(185, 117);
			minutesNum.Name = "minutesNum";
			minutesNum.Size = new Size(112, 39);
			minutesNum.TabIndex = 1;
			// 
			// secondsNum
			// 
			secondsNum.Location = new Point(397, 117);
			secondsNum.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
			secondsNum.Name = "secondsNum";
			secondsNum.Size = new Size(112, 39);
			secondsNum.TabIndex = 2;
			secondsNum.Value = new decimal(new int[] { 5, 0, 0, 0 });
			// 
			// lblMinutes
			// 
			lblMinutes.AutoSize = true;
			lblMinutes.Location = new Point(185, 72);
			lblMinutes.Name = "lblMinutes";
			lblMinutes.Size = new Size(103, 32);
			lblMinutes.TabIndex = 6;
			lblMinutes.Text = "Минуты";
			// 
			// lblSeconds
			// 
			lblSeconds.AutoSize = true;
			lblSeconds.Location = new Point(397, 72);
			lblSeconds.Name = "lblSeconds";
			lblSeconds.Size = new Size(110, 32);
			lblSeconds.TabIndex = 7;
			lblSeconds.Text = "Секунды";
			// 
			// circularTimer
			// 
			circularTimer.Location = new Point(197, 306);
			circularTimer.Name = "circularTimer";
			circularTimer.Size = new Size(300, 300);
			circularTimer.TabIndex = 8;
			circularTimer.Text = "circularTimer1";
			// 
			// btnSettings
			// 
			btnSettings.BackColor = SystemColors.ActiveCaption;
			btnSettings.FlatStyle = FlatStyle.Popup;
			btnSettings.Location = new Point(12, 12);
			btnSettings.Name = "btnSettings";
			btnSettings.Size = new Size(50, 50);
			btnSettings.TabIndex = 11;
			btnSettings.Text = "⛭";
			btnSettings.UseVisualStyleBackColor = false;
			btnSettings.Click += btnSettings_Click;
			// 
			// btnPlayPause
			// 
			btnPlayPause.BackColor = Color.FromArgb(153, 102, 204);
			btnPlayPause.BorderRadius = 50;
			btnPlayPause.FlatStyle = FlatStyle.Flat;
			btnPlayPause.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
			btnPlayPause.ForeColor = Color.White;
			btnPlayPause.Location = new Point(197, 189);
			btnPlayPause.Name = "btnPlayPause";
			btnPlayPause.Size = new Size(100, 100);
			btnPlayPause.TabIndex = 12;
			btnPlayPause.Text = "start";
			btnPlayPause.UseVisualStyleBackColor = false;
			btnPlayPause.Click += btnPlayPause_Click;
			// 
			// btnTimerStop
			// 
			btnTimerStop.BackColor = Color.FromArgb(153, 102, 204);
			btnTimerStop.FlatStyle = FlatStyle.Flat;
			btnTimerStop.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
			btnTimerStop.ForeColor = Color.White;
			btnTimerStop.Location = new Point(397, 189);
			btnTimerStop.Name = "btnTimerStop";
			btnTimerStop.Size = new Size(100, 100);
			btnTimerStop.TabIndex = 13;
			btnTimerStop.Text = "stop";
			btnTimerStop.UseVisualStyleBackColor = false;
			btnTimerStop.Click += btnTimerStop_Click;
			// 
			// btnInfo
			// 
			btnInfo.BackColor = SystemColors.ActiveCaption;
			btnInfo.FlatStyle = FlatStyle.Popup;
			btnInfo.Location = new Point(612, 12);
			btnInfo.Name = "btnInfo";
			btnInfo.Size = new Size(50, 50);
			btnInfo.TabIndex = 14;
			btnInfo.Text = "!";
			btnInfo.UseVisualStyleBackColor = false;
			btnInfo.Click += btnInfo_Click;
			// 
			// MainForm
			// 
			AutoScaleDimensions = new SizeF(13F, 32F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = SystemColors.ControlDark;
			ClientSize = new Size(674, 629);
			Controls.Add(btnInfo);
			Controls.Add(btnTimerStop);
			Controls.Add(btnPlayPause);
			Controls.Add(btnSettings);
			Controls.Add(circularTimer);
			Controls.Add(lblSeconds);
			Controls.Add(lblMinutes);
			Controls.Add(secondsNum);
			Controls.Add(minutesNum);
			Icon = (Icon)resources.GetObject("$this.Icon");
			Name = "MainForm";
			Text = "MiniPomidoro";
			((System.ComponentModel.ISupportInitialize)minutesNum).EndInit();
			((System.ComponentModel.ISupportInitialize)secondsNum).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private NotifyIcon notifyIcon1;
		private NumericUpDown minutesNum;
		private NumericUpDown secondsNum;
		private Label lblMinutes;
		private Label lblSeconds;
		private CircularTimer circularTimer;
		private Button btnSettings;
		private ModernButton btnPlayPause;
		private ModernButton btnTimerStop;
		private Button btnInfo;
	}
}
