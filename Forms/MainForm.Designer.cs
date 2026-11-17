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
			minutesField = new TimeField();
			secondsField = new TimeField();
			btnStart = new IconButton();
			btnSettings = new ModernButton();
			btnInfo = new ModernButton();
			SuspendLayout();
			// 
			// notifyIcon1
			// 
			notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
			notifyIcon1.Text = "MiniPomidoro";
			notifyIcon1.Visible = true;
			// 
			// minutesField
			// 
			minutesField.Caption = "мин";
			minutesField.Location = new Point(98, 100);
			minutesField.Maximum = 100;
			minutesField.Name = "minutesField";
			minutesField.Size = new Size(160, 110);
			minutesField.TabIndex = 0;
			minutesField.Value = 25;
			// 
			// secondsField
			// 
			secondsField.Caption = "сек";
			secondsField.Location = new Point(282, 100);
			secondsField.Maximum = 59;
			secondsField.Name = "secondsField";
			secondsField.Size = new Size(160, 110);
			secondsField.TabIndex = 1;
			// 
			// btnStart
			// 
			btnStart.BackColor = Color.FromArgb(153, 102, 204);
			btnStart.BorderRadius = 68;
			btnStart.FlatStyle = FlatStyle.Flat;
			btnStart.ForeColor = Color.White;
			btnStart.Glyph = IconGlyph.Play;
			btnStart.IconScale = 0.34F;
			btnStart.Location = new Point(202, 236);
			btnStart.Name = "btnStart";
			btnStart.Size = new Size(136, 136);
			btnStart.TabIndex = 2;
			btnStart.UseVisualStyleBackColor = false;
			btnStart.Click += btnStart_Click;
			// 
			// btnSettings
			// 
			btnSettings.BackColor = Color.FromArgb(153, 102, 204);
			btnSettings.BorderRadius = 26;
			btnSettings.FlatStyle = FlatStyle.Flat;
			btnSettings.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
			btnSettings.ForeColor = Color.White;
			btnSettings.Location = new Point(18, 18);
			btnSettings.Name = "btnSettings";
			btnSettings.Size = new Size(52, 52);
			btnSettings.TabIndex = 3;
			btnSettings.Text = "⚙";
			btnSettings.UseVisualStyleBackColor = false;
			btnSettings.Click += btnSettings_Click;
			// 
			// btnInfo
			// 
			btnInfo.BackColor = Color.FromArgb(153, 102, 204);
			btnInfo.BorderRadius = 26;
			btnInfo.FlatStyle = FlatStyle.Flat;
			btnInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
			btnInfo.ForeColor = Color.White;
			btnInfo.Location = new Point(470, 18);
			btnInfo.Name = "btnInfo";
			btnInfo.Size = new Size(52, 52);
			btnInfo.TabIndex = 4;
			btnInfo.Text = "i";
			btnInfo.UseVisualStyleBackColor = false;
			btnInfo.Click += btnInfo_Click;
			// 
			// MainForm
			// 
			AutoScaleDimensions = new SizeF(13F, 32F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.FromArgb(34, 40, 49);
			ClientSize = new Size(540, 400);
			Controls.Add(btnInfo);
			Controls.Add(btnSettings);
			Controls.Add(btnStart);
			Controls.Add(secondsField);
			Controls.Add(minutesField);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			Icon = (Icon)resources.GetObject("$this.Icon");
			MaximizeBox = false;
			Name = "MainForm";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "MiniPomidoro";
			ResumeLayout(false);
		}

		#endregion

		private NotifyIcon notifyIcon1;
		private TimeField minutesField;
		private TimeField secondsField;
		private IconButton btnStart;
		private ModernButton btnSettings;
		private ModernButton btnInfo;
	}
}