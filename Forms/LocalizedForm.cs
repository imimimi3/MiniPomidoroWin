// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 imimimi3

using MiniPomidoroWin.Service;

namespace MiniPomidoroWin.Forms
{
	/// <summary>
	/// Форма, которая сама обновляет подписи при смене языка интерфейса
	/// </summary>
	public class LocalizedForm : Form
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		public LocalizedForm()
		{
			LocalizationService.Instance.LanguageChanged += this.LocalizationService_LanguageChanged;
		}

		/// <summary>
		/// Применение локализованных строк к элементам формы
		/// </summary>
		protected virtual void ApplyLocalization()
		{
		}

		/// <summary>
		/// Отписка от события смены языка
		/// </summary>
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			LocalizationService.Instance.LanguageChanged -= this.LocalizationService_LanguageChanged;

			base.OnFormClosed(e);
		}

		/// <summary>
		/// Смена языка интерфейса
		/// </summary>
		private void LocalizationService_LanguageChanged(object? sender, EventArgs e)
		{
			this.ApplyLocalization();
		}
	}
}