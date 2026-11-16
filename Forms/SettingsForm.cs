using MiniPomidoroWin.Forms.Localization;
using MiniPomidoroWin.Service;
using MiniPomidoroWin.Themes;

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

			LocalizationService.Instance.LanguageChanged += LocalizationService_LanguageChanged;

			this.ApplyLocalization();
		}

		/// <summary>
		/// Отписка от события смены языка
		/// </summary>
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			LocalizationService.Instance.LanguageChanged -= LocalizationService_LanguageChanged;

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
			btnLanguage.Text = AppStrings.SettingsForm_LanguageButton;
			btnChooseSound.Text = AppStrings.SettingsForm_ChooseSoundButton;
			btnResetSound.Text = AppStrings.SettingsForm_ResetButton;
		}

		/// <summary>
		/// Обработка нажатия на кнопку выбора мелодии
		/// </summary>
		private void btnChooseSound_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog ofd = new OpenFileDialog { Filter = AppStrings.SettingsForm_SoundFileFilter })
			{
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					CustomSoundPath = ofd.FileName;

					try
					{
						_audioService.PlayCustomFile(ofd.FileName);

						MessageBox.Show(
							AppStrings.SettingsForm_SoundSelected,
							AppStrings.SettingsForm_SoundDialogTitle,
							MessageBoxButtons.OK,
							MessageBoxIcon.Information);
					}
					catch (Exception ex)
					{
						MessageBox.Show(
							string.Format(AppStrings.SettingsForm_SoundPlayError, ex.Message),
							AppStrings.Common_ErrorTitle,
							MessageBoxButtons.OK,
							MessageBoxIcon.Error);
					}
				}
			}
		}

		/// <summary>
		/// Обработка нажатия на кнопку сброса выбранной мелодии
		/// </summary>
		private void btnResetSound_Click(object sender, EventArgs e)
		{
			ResetCustomSound();

			MessageBox.Show(
				AppStrings.SettingsForm_SoundReset,
				AppStrings.SettingsForm_SoundDialogTitle,
				MessageBoxButtons.OK,
				MessageBoxIcon.Information);
		}

		/// <summary>
		/// Переключение языка интерфейса
		/// </summary>
		private void btnLanguage_Click(object sender, EventArgs e)
		{
			LocalizationService.Instance.ToggleLanguage();
		}
	}
}