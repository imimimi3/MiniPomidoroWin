// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 imimimi3

using MiniPomidoroWin.Forms.Localization;
using MiniPomidoroWin.Themes;

namespace MiniPomidoroWin.Forms
{
	/// <summary>
	/// Сообщение поверх всех окон
	/// </summary>
	public partial class NotificationForm : LocalizedForm
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		public NotificationForm()
		{
			InitializeComponent();

			this.BackColor = Theme.FormBackground;

			this.ApplyLocalization();
		}

		/// <summary>
		/// Применение локализованных строк
		/// </summary>
		protected override void ApplyLocalization()
		{
			this.Text = AppStrings.MainForm_Title;
			this.lblMessage.Text = AppStrings.Notification_TimeToSwitch;
			this.btnOk.Text = AppStrings.Common_Ok;
		}

		/// <summary>
		/// Вывод окна на передний план
		/// </summary>
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			this.Activate();
			this.BringToFront();
		}

		/// <summary>
		/// Закрытие сообщения
		/// </summary>
		private void btnOk_Click(object? sender, EventArgs e)
		{
			this.Close();
		}
	}
}