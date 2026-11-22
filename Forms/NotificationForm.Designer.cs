namespace MiniPomidoroWin.Forms
{
	partial class NotificationForm
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
			lblMessage = new Label();
			btnOk = new MiniPomidoroWin.Controls.ModernButton();
			SuspendLayout();
			// 
			// lblMessage
			// 
			lblMessage.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
			lblMessage.ForeColor = Color.White;
			lblMessage.Location = new Point(30, 30);
			lblMessage.Name = "lblMessage";
			lblMessage.Size = new Size(672, 76);
			lblMessage.TabIndex = 0;
			lblMessage.Text = "Пора переключиться!";
			lblMessage.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// btnOk
			// 
			btnOk.BackColor = Color.FromArgb(153, 102, 204);
			btnOk.BorderRadius = 28;
			btnOk.FlatStyle = FlatStyle.Flat;
			btnOk.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
			btnOk.ForeColor = Color.White;
			btnOk.Location = new Point(274, 132);
			btnOk.Name = "btnOk";
			btnOk.Size = new Size(160, 56);
			btnOk.TabIndex = 1;
			btnOk.Text = "OK";
			btnOk.UseVisualStyleBackColor = false;
			btnOk.Click += btnOk_Click;
			// 
			// NotificationForm
			// 
			AutoScaleDimensions = new SizeF(192F, 192F);
			AutoScaleMode = AutoScaleMode.Dpi;
			BackColor = Color.FromArgb(34, 40, 49);
			ClientSize = new Size(722, 216);
			Controls.Add(btnOk);
			Controls.Add(lblMessage);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "NotificationForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterScreen;
			Text = "MiniPomidoro";
			TopMost = true;
			ResumeLayout(false);
		}

		#endregion

		private Label lblMessage;
		private Controls.ModernButton btnOk;
	}
}