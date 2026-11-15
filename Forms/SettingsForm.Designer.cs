namespace MiniPomidoroWin.Forms
{
	partial class SettingsForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
			btnChooseSound = new MiniPomidoroWin.Controls.ModernButton();
			btnResetSound = new MiniPomidoroWin.Controls.ModernButton();
			btnLanguage = new MiniPomidoroWin.Controls.ModernButton();
			SuspendLayout();
			// 
			// btnChooseSound
			// 
			btnChooseSound.BackColor = Color.FromArgb(153, 102, 204);
			btnChooseSound.FlatStyle = FlatStyle.Flat;
			btnChooseSound.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
			btnChooseSound.ForeColor = Color.White;
			btnChooseSound.Location = new Point(224, 94);
			btnChooseSound.Name = "btnChooseSound";
			btnChooseSound.Size = new Size(334, 100);
			btnChooseSound.TabIndex = 0;
			btnChooseSound.Text = "Выбор мелодии окончания";
			btnChooseSound.UseVisualStyleBackColor = false;
			btnChooseSound.Click += btnChooseSound_Click;
			// 
			// btnResetSound
			// 
			btnResetSound.BackColor = Color.FromArgb(153, 102, 204);
			btnResetSound.FlatStyle = FlatStyle.Flat;
			btnResetSound.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
			btnResetSound.ForeColor = Color.White;
			btnResetSound.Location = new Point(306, 254);
			btnResetSound.Name = "btnResetSound";
			btnResetSound.Size = new Size(176, 100);
			btnResetSound.TabIndex = 1;
			btnResetSound.Text = "Сброс";
			btnResetSound.UseVisualStyleBackColor = false;
			btnResetSound.Click += btnResetSound_Click;
			// 
			// btnLanguage
			// 
			btnLanguage.BackColor = Color.FromArgb(153, 102, 204);
			btnLanguage.BorderRadius = 25;
			btnLanguage.FlatStyle = FlatStyle.Flat;
			btnLanguage.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
			btnLanguage.ForeColor = Color.White;
			btnLanguage.Location = new Point(306, 368);
			btnLanguage.Name = "btnLanguage";
			btnLanguage.Size = new Size(176, 52);
			btnLanguage.TabIndex = 3;
			btnLanguage.Text = "English";
			btnLanguage.UseVisualStyleBackColor = false;
			btnLanguage.Click += btnLanguage_Click;
			// 
			// SettingsForm
			// 
			AutoScaleDimensions = new SizeF(13F, 32F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = SystemColors.ControlDark;
			ClientSize = new Size(800, 450);
			Controls.Add(btnLanguage);
			Controls.Add(btnResetSound);
			Controls.Add(btnChooseSound);
			Icon = (Icon)resources.GetObject("$this.Icon");
			Name = "SettingsForm";
			Text = "Настройки";
			ResumeLayout(false);
		}

		#endregion
		private Controls.ModernButton btnChooseSound;
		private Controls.ModernButton btnResetSound;
		private Controls.ModernButton btnLanguage;
	}
}