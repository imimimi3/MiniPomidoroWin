using MiniPomidoroWin.Forms.Localization;
using MiniPomidoroWin.Service;

namespace MiniPomidoroWin.Forms
{
	public partial class InfoForm : Form
	{
		/// <summary>
		/// Конструктор формы
		/// </summary>
		public InfoForm()
		{
			InitializeComponent();

			LocalizationService.Instance.LanguageChanged += LocalizationService_LanguageChanged;

			this.ApplyLocalization();
		}

		/// <summary>
		/// Отписка от события смены языка
		/// </summary>
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			LocalizationService.Instance.LanguageChanged -= LocalizationService_LanguageChanged;
			base.OnFormClosed(e);
		}

		/// <summary>
		/// Смена языка интерфейса
		/// </summary>
		private void LocalizationService_LanguageChanged(object? sender, EventArgs e)
		{
			this.ApplyLocalization();
		}

		/// <summary>
		/// Применение локализованных строк к элементам формы
		/// </summary>
		private void ApplyLocalization()
		{
			this.Text = AppStrings.InfoForm_Title;
			label1.Text = AppStrings.InfoForm_ThanksLabel;
		}
	}
}