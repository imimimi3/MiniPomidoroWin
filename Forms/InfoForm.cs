// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 imimimi3

using MiniPomidoroWin.Forms.Localization;
using MiniPomidoroWin.Themes;

namespace MiniPomidoroWin.Forms
{
	/// <summary>
	/// Форма с информацией о приложении
	/// </summary>
	public partial class InfoForm : LocalizedForm
	{
		/// <summary>
		/// Конструктор формы
		/// </summary>
		public InfoForm()
		{
			InitializeComponent();

			this.BackColor = Theme.FormBackground;

			this.ApplyLocalization();
		}

		/// <summary>
		/// Применение локализованных строк к элементам формы
		/// </summary>
		protected override void ApplyLocalization()
		{
			this.Text = AppStrings.InfoForm_Title;

			this.lblAppName.Text = AppStrings.InfoForm_AppName;
			this.lblHowToUse.Text = AppStrings.InfoForm_HowToUse;
			this.lblInstructions.Text = AppStrings.InfoForm_Instructions;
			this.lblThanks.Text = AppStrings.InfoForm_ThanksLabel;
		}
	}
}