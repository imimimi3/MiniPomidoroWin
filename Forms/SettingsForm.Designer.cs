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
			lblSoundSection = new Label();
			lblCurrentSound = new Label();
			btnChooseSound = new MiniPomidoroWin.Controls.ModernButton();
			btnResetSound = new MiniPomidoroWin.Controls.ModernButton();
			pnlDivider = new Panel();
			lblLanguageSection = new Label();
			btnLanguage = new MiniPomidoroWin.Controls.ModernButton();
			SuspendLayout();
			// 
			// lblSoundSection
			// 
			lblSoundSection.AutoSize = true;
			lblSoundSection.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
			lblSoundSection.ForeColor = Color.FromArgb(138, 147, 165);
			lblSoundSection.Location = new Point(40, 28);
			lblSoundSection.Name = "lblSoundSection";
			lblSoundSection.Size = new Size(252, 41);
			lblSoundSection.TabIndex = 0;
			lblSoundSection.Text = "Звук окончания";
			// 
			// lblCurrentSound
			// 
			lblCurrentSound.AutoEllipsis = true;
			lblCurrentSound.Font = new Font("Segoe UI", 12F);
			lblCurrentSound.ForeColor = Color.White;
			lblCurrentSound.Location = new Point(40, 78);
			lblCurrentSound.Name = "lblCurrentSound";
			lblCurrentSound.Size = new Size(586, 54);
			lblCurrentSound.TabIndex = 1;
			lblCurrentSound.Text = "Стандартный звук";
			lblCurrentSound.TextAlign = ContentAlignment.MiddleLeft;
			// 
			// btnChooseSound
			// 
			btnChooseSound.BackColor = Color.FromArgb(153, 102, 204);
			btnChooseSound.BorderRadius = 40;
			btnChooseSound.FlatStyle = FlatStyle.Flat;
			btnChooseSound.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
			btnChooseSound.ForeColor = Color.White;
			btnChooseSound.Location = new Point(40, 146);
			btnChooseSound.Name = "btnChooseSound";
			btnChooseSound.Size = new Size(586, 80);
			btnChooseSound.TabIndex = 2;
			btnChooseSound.Text = "Выбор мелодии окончания";
			btnChooseSound.UseVisualStyleBackColor = false;
			btnChooseSound.Click += btnChooseSound_Click;
			// 
			// btnResetSound
			// 
			btnResetSound.BackColor = Color.FromArgb(44, 52, 64);
			btnResetSound.BorderRadius = 32;
			btnResetSound.FlatAppearance.MouseDownBackColor = Color.FromArgb(36, 44, 55);
			btnResetSound.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 68, 82);
			btnResetSound.FlatStyle = FlatStyle.Flat;
			btnResetSound.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
			btnResetSound.ForeColor = Color.White;
			btnResetSound.Location = new Point(466, 242);
			btnResetSound.Name = "btnResetSound";
			btnResetSound.Size = new Size(160, 64);
			btnResetSound.TabIndex = 3;
			btnResetSound.Text = "Сброс";
			btnResetSound.UseVisualStyleBackColor = false;
			btnResetSound.Click += btnResetSound_Click;
			// 
			// pnlDivider
			// 
			pnlDivider.BackColor = Color.FromArgb(58, 68, 82);
			pnlDivider.Location = new Point(40, 320);
			pnlDivider.Name = "pnlDivider";
			pnlDivider.Size = new Size(586, 1);
			pnlDivider.TabIndex = 4;
			// 
			// lblLanguageSection
			// 
			lblLanguageSection.AutoSize = true;
			lblLanguageSection.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
			lblLanguageSection.ForeColor = Color.FromArgb(138, 147, 165);
			lblLanguageSection.Location = new Point(30, 342);
			lblLanguageSection.Name = "lblLanguageSection";
			lblLanguageSection.Size = new Size(275, 41);
			lblLanguageSection.TabIndex = 5;
			lblLanguageSection.Text = "Язык интерфейса";
			// 
			// btnLanguage
			// 
			btnLanguage.BackColor = Color.FromArgb(44, 52, 64);
			btnLanguage.BorderRadius = 32;
			btnLanguage.FlatAppearance.MouseDownBackColor = Color.FromArgb(36, 44, 55);
			btnLanguage.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 68, 82);
			btnLanguage.FlatStyle = FlatStyle.Flat;
			btnLanguage.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
			btnLanguage.ForeColor = Color.White;
			btnLanguage.Location = new Point(30, 400);
			btnLanguage.Name = "btnLanguage";
			btnLanguage.Size = new Size(180, 64);
			btnLanguage.TabIndex = 6;
			btnLanguage.Text = "Русский";
			btnLanguage.UseVisualStyleBackColor = false;
			btnLanguage.Click += btnLanguage_Click;
			// 
			// SettingsForm
			// 
			AutoScaleDimensions = new SizeF(13F, 32F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.FromArgb(34, 40, 49);
			ClientSize = new Size(666, 504);
			Controls.Add(btnLanguage);
			Controls.Add(lblLanguageSection);
			Controls.Add(pnlDivider);
			Controls.Add(btnResetSound);
			Controls.Add(btnChooseSound);
			Controls.Add(lblCurrentSound);
			Controls.Add(lblSoundSection);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			Icon = (Icon)resources.GetObject("$this.Icon");
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "SettingsForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Настройки";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Label lblSoundSection;
		private Label lblCurrentSound;
		private Controls.ModernButton btnChooseSound;
		private Controls.ModernButton btnResetSound;
		private Panel pnlDivider;
		private Label lblLanguageSection;
		private Controls.ModernButton btnLanguage;
	}
}