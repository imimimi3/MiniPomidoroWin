using MiniPomidoroWin.Controls;
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
		/// Подгонка формы под экран, а текста — под элементы
		/// </summary>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			ScreenFit.FitToWorkingArea(this);
			AutoFitText.Apply(this);
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
			AutoFitText.Apply(this);
		}
	}
}