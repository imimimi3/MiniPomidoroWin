using MiniPomidoroWin.Controls;
using MiniPomidoroWin.Forms.Localization;
using MiniPomidoroWin.Service;
using MiniPomidoroWin.Themes;
using System.IO;

namespace MiniPomidoroWin.Forms
{
	public partial class SettingsForm : Form
	{
		/// <summary>
		/// Путь к пользовательской мелодии
		/// </summary>
		public static string? CustomSoundPath
		{
			get => Properties.Settings.Default.CustomSoundPath;
			set
			{
				Properties.Settings.Default.CustomSoundPath = value;
				Properties.Settings.Default.Save();
			}
		}

		/// <summary>
		/// Сброс выбранной музыки на стандартную
		/// </summary>
		public static void ResetCustomSound()
		{
			CustomSoundPath = null;
		}

		/// <summary>
		/// Сервис звука
		/// </summary>
		private readonly AudioService _audioService = new AudioService();

		/// <summary>
		/// Конструктор формы
		/// </summary>
		public SettingsForm()
		{
			InitializeComponent();

			this.BackColor = Theme.FormBackground;

			this.ApplySecondaryStyle(this.btnResetSound);
			this.ApplySecondaryStyle(this.btnLanguage);

			LocalizationService.Instance.LanguageChanged += this.LocalizationService_LanguageChanged;

			this.ApplyLocalization();
		}

		/// <summary>
		/// Отписка от события смены языка
		/// </summary>
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			LocalizationService.Instance.LanguageChanged -= this.LocalizationService_LanguageChanged;

			_audioService.Dispose();

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
			this.Text = AppStrings.SettingsForm_Title;

			this.lblSoundSection.Text = AppStrings.SettingsForm_SoundSection;
			this.lblLanguageSection.Text = AppStrings.SettingsForm_LanguageSection;

			this.btnLanguage.Text = AppStrings.SettingsForm_LanguageButton;
			this.btnChooseSound.Text = AppStrings.SettingsForm_ChooseSoundButton;
			this.btnResetSound.Text = AppStrings.SettingsForm_ResetButton;

			this.UpdateSoundLabel();
		}

		/// <summary>
		/// Оформление второстепенной кнопки
		/// </summary>
		private void ApplySecondaryStyle(ModernButton button)
		{
			button.BackColor = Theme.ButtonSecondary;
			button.ForeColor = Theme.TextColor;
			button.FlatAppearance.MouseOverBackColor = Theme.ButtonSecondaryHover;
			button.FlatAppearance.MouseDownBackColor = Theme.ButtonSecondaryPressed;
		}

		/// <summary>
		/// Показ текущей мелодии окончания
		/// </summary>
		private void UpdateSoundLabel()
		{
			string? path = CustomSoundPath;

			this.lblCurrentSound.Text = string.IsNullOrEmpty(path)
				? AppStrings.SettingsForm_DefaultSound
				: Path.GetFileName(path);
		}

		/// <summary>
		/// Обработка нажатия на кнопку выбора мелодии
		/// </summary>
		private void btnChooseSound_Click(object? sender, EventArgs e)
		{
			using (OpenFileDialog ofd = new OpenFileDialog { Filter = AppStrings.SettingsForm_SoundFileFilter })
			{
				if (ofd.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}

				CustomSoundPath = ofd.FileName;

				try
				{
					_audioService.PlayCustomFile(ofd.FileName);

					this.UpdateSoundLabel();

					MessageBox.Show(
						this,
						AppStrings.SettingsForm_SoundSelected,
						AppStrings.SettingsForm_SoundDialogTitle,
						MessageBoxButtons.OK,
						MessageBoxIcon.Information);
				}
				catch (Exception ex)
				{
					MessageBox.Show(
						this,
						string.Format(AppStrings.SettingsForm_SoundPlayError, ex.Message),
						AppStrings.Common_ErrorTitle,
						MessageBoxButtons.OK,
						MessageBoxIcon.Error);
				}
			}
		}

		/// <summary>
		/// Обработка нажатия на кнопку сброса выбранной мелодии
		/// </summary>
		private void btnResetSound_Click(object? sender, EventArgs e)
		{
			ResetCustomSound();

			this.UpdateSoundLabel();

			MessageBox.Show(
				this,
				AppStrings.SettingsForm_SoundReset,
				AppStrings.SettingsForm_SoundDialogTitle,
				MessageBoxButtons.OK,
				MessageBoxIcon.Information);
		}

		/// <summary>
		/// Переключение языка интерфейса
		/// </summary>
		private void btnLanguage_Click(object? sender, EventArgs e)
		{
			LocalizationService.Instance.ToggleLanguage();
		}
	}
}