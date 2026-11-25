using MiniPomidoroWin.Controls;
using MiniPomidoroWin.Forms.Localization;
using MiniPomidoroWin.Service;
using MiniPomidoroWin.Themes;
using System.Diagnostics;
using System.IO;

namespace MiniPomidoroWin.Forms
{
	/// <summary>
	/// Форма настроек приложения
	/// </summary>
	public partial class SettingsForm : LocalizedForm
	{
		/// <summary>
		/// Сервис предпросмотра мелодии окончания
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
			this.ApplySecondaryStyle(this.btnAutoStart);

			this.ApplyLocalization();
		}

		/// <summary>
		/// Остановка предпросмотра и освобождение ресурсов
		/// </summary>
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			// Настройки можно закрыть с играющим предпросмотром
			this._audioService.Dispose();

			base.OnFormClosed(e);
		}

		/// <summary>
		/// Применение локализованных строк к элементам формы
		/// </summary>
		protected override void ApplyLocalization()
		{
			this.Text = AppStrings.SettingsForm_Title;

			this.lblSoundSection.Text = AppStrings.SettingsForm_SoundSection;
			this.lblLanguageSection.Text = AppStrings.SettingsForm_LanguageSection;

			this.lblSoundSection.Text = AppStrings.SettingsForm_SoundSection;
			this.lblLanguageSection.Text = AppStrings.SettingsForm_LanguageSection;
			this.lblStartupSection.Text = AppStrings.SettingsForm_StartupSection;

			this.btnLanguage.Text = AppStrings.SettingsForm_LanguageButton;
			this.btnChooseSound.Text = AppStrings.SettingsForm_ChooseSoundButton;
			this.btnResetSound.Text = AppStrings.SettingsForm_ResetButton;

			this.UpdateSoundLabel();
			this.UpdateAutoStartButton();
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
			string? path = SettingsService.CustomSoundPath;

			this.lblCurrentSound.Text = string.IsNullOrEmpty(path)
				? AppStrings.SettingsForm_DefaultSound
				: Path.GetFileName(path);
		}

		/// <summary>
		/// Выбор мелодии окончания с предпросмотром
		/// </summary>
		private void btnChooseSound_Click(object? sender, EventArgs e)
		{
			using (OpenFileDialog ofd = new OpenFileDialog
			{
				Title = AppStrings.SettingsForm_SoundDialogTitle,
				Filter = AppStrings.SettingsForm_SoundFileFilter
			})
			{
				if (ofd.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}

				string fileName = ofd.FileName;

				try
				{
					// Сначала предпросмотр: путь сохраняем только для рабочего файла,
					// иначе каждый конец таймера уходил бы в ошибку воспроизведения
					this._audioService.PlayCustomFile(fileName);
				}
				catch (Exception ex)
				{
					MessageBox.Show(
						this,
						string.Format(AppStrings.SettingsForm_SoundPlayError, ex.Message),
						AppStrings.Common_ErrorTitle,
						MessageBoxButtons.OK,
						MessageBoxIcon.Error);

					return;
				}

				SettingsService.CustomSoundPath = fileName;

				this.UpdateSoundLabel();

				MessageBox.Show(
					this,
					AppStrings.SettingsForm_SoundSelected,
					AppStrings.SettingsForm_SoundDialogTitle,
					MessageBoxButtons.OK,
					MessageBoxIcon.Information);
			}
		}

		/// <summary>
		/// Сброс выбранной мелодии на стандартную
		/// </summary>
		private void btnResetSound_Click(object? sender, EventArgs e)
		{
			// Предпросмотр должен замолчать сразу, а не доиграть после сброса
			this._audioService.Stop();

			SettingsService.ResetCustomSound();

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

		/// <summary>
		/// Текст и цвет кнопки автозапуска
		/// </summary>
		private void UpdateAutoStartButton()
		{
			bool supported = AutoStartService.IsSupported;
			bool enabled = supported && AutoStartService.IsEnabled;

			this.btnAutoStart.Enabled = supported;

			this.btnAutoStart.Text = !supported
				? AppStrings.SettingsForm_AutoStartUnsupported
				: enabled
					? AppStrings.SettingsForm_AutoStartEnabled
					: AppStrings.SettingsForm_AutoStartDisabled;

			this.btnAutoStart.BackColor = enabled ? Theme.ButtonPrimary : Theme.ButtonSecondary;
			this.btnAutoStart.ForeColor = Theme.TextColor;

			this.btnAutoStart.FlatAppearance.MouseOverBackColor =
				enabled ? Theme.ButtonHover : Theme.ButtonSecondaryHover;
			this.btnAutoStart.FlatAppearance.MouseDownBackColor =
				enabled ? Theme.ButtonPressed : Theme.ButtonSecondaryPressed;
		}

		/// <summary>
		/// Переключение автозапуска
		/// </summary>
		private void btnAutoStart_Click(object? sender, EventArgs e)
		{
			try
			{
				AutoStartService.SetEnabled(!AutoStartService.IsEnabled);
			}
			catch (Exception ex)
			{
				Trace.TraceError("Не удалось изменить автозапуск: {0}", ex);

				MessageBox.Show(
					this,
					string.Format(AppStrings.SettingsForm_AutoStartError, ex.Message),
					AppStrings.Common_ErrorTitle,
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
			}

			this.UpdateAutoStartButton();
		}
	}
}