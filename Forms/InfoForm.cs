using MiniPomidoroWin.Forms.Localization;
using MiniPomidoroWin.Service;
using MiniPomidoroWin.Themes;

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

			this.BackColor = Theme.FormBackground;

			LocalizationService.Instance.LanguageChanged += this.LocalizationService_LanguageChanged;

			this.ApplyLocalization();
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

		/// <summary>
		/// Применение локализованных строк к элементам формы
		/// </summary>
		private void ApplyLocalization()
		{
			this.Text = AppStrings.InfoForm_Title;

			this.lblAppName.Text = AppStrings.MainForm_Title;
			this.lblHowToUse.Text = AppStrings.InfoForm_HowToUse;
			this.lblInstructions.Text = AppStrings.InfoForm_Instructions;
			this.lblThanks.Text = AppStrings.InfoForm_ThanksLabel;
		}
	}
}