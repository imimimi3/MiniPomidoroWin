namespace MiniPomidoroWin.Forms
{
	partial class InfoForm
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
			lblAppName = new Label();
			lblHowToUse = new Label();
			lblInstructions = new Label();
			pnlDivider = new Panel();
			lblThanks = new Label();
			SuspendLayout();
			// 
			// lblAppName
			// 
			lblAppName.AutoSize = true;
			lblAppName.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
			lblAppName.ForeColor = Color.FromArgb(153, 102, 204);
			lblAppName.Location = new Point(40, 24);
			lblAppName.Name = "lblAppName";
			lblAppName.Size = new Size(432, 78);
			lblAppName.TabIndex = 0;
			lblAppName.Text = "MiniPomidoro";
			// 
			// lblHowToUse
			// 
			lblHowToUse.AutoSize = true;
			lblHowToUse.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
			lblHowToUse.ForeColor = Color.FromArgb(138, 147, 165);
			lblHowToUse.Location = new Point(40, 120);
			lblHowToUse.Name = "lblHowToUse";
			lblHowToUse.Size = new Size(299, 45);
			lblHowToUse.TabIndex = 1;
			lblHowToUse.Text = "Как пользоваться";
			// 
			// lblInstructions
			// 
			lblInstructions.Font = new Font("Segoe UI", 12F);
			lblInstructions.ForeColor = Color.White;
			lblInstructions.Location = new Point(40, 180);
			lblInstructions.Name = "lblInstructions";
			lblInstructions.Size = new Size(586, 335);
			lblInstructions.TabIndex = 2;
			lblInstructions.Text = "1. Задайте время: минуты и секунды.\r\n2. Нажмите ▶ — откроется мини-таймер.\r\n3. Пауза и стоп — на мини-окне.\r\n4. Esc — остановить и вернуться.\r\n5. Звук и язык — в настройках.";
			// 
			// pnlDivider
			// 
			pnlDivider.BackColor = Color.FromArgb(58, 68, 82);
			pnlDivider.Location = new Point(40, 530);
			pnlDivider.Name = "pnlDivider";
			pnlDivider.Size = new Size(586, 1);
			pnlDivider.TabIndex = 3;
			// 
			// lblThanks
			// 
			lblThanks.Font = new Font("Segoe UI", 11F);
			lblThanks.ForeColor = Color.FromArgb(138, 147, 165);
			lblThanks.Location = new Point(40, 540);
			lblThanks.Name = "lblThanks";
			lblThanks.Size = new Size(586, 105);
			lblThanks.TabIndex = 4;
			lblThanks.Text = "Спасибо за использование MiniPomidoro)";
			lblThanks.TextAlign = ContentAlignment.MiddleLeft;
			// 
			// InfoForm
			// 
			AutoScaleDimensions = new SizeF(13F, 32F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.FromArgb(34, 40, 49);
			ClientSize = new Size(666, 666);
			Controls.Add(lblThanks);
			Controls.Add(pnlDivider);
			Controls.Add(lblInstructions);
			Controls.Add(lblHowToUse);
			Controls.Add(lblAppName);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "InfoForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Информация";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Label lblAppName;
		private Label lblHowToUse;
		private Label lblInstructions;
		private Panel pnlDivider;
		private Label lblThanks;
	}
}