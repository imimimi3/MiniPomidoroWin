using MiniPomidoroWin.Forms;
using MiniPomidoroWin.Forms.Localization;
using MiniPomidoroWin.Service;
using MiniPomidoroWin.Themes;
using System.IO;

namespace MiniPomidoroWin
{
	public partial class MainForm : Form
	{
		/// <summary>
		/// Сервис звука
		/// </summary>
		private readonly AudioService _audioService = new AudioService();

		/// <summary>
		/// Мини-окно с таймером обратного отсчёта
		/// </summary>
		private MiniTimerForm? _miniTimerForm;

		/// <summary>
		/// Конструктор формы
		/// </summary>
		public MainForm()
		{
			InitializeComponent();

			this.BackColor = Theme.FormBackground;

			this.minutesField.ValueChanged += this.TimeField_ValueChanged;
			this.secondsField.ValueChanged += this.TimeField_ValueChanged;

			LocalizationService.Instance.LanguageChanged += this.LocalizationService_LanguageChanged;

			this.ApplyLocalization();
			this.UpdateStartButtonState();
		}

		/// <summary>
		/// Отписка от событий и освобождение ресурсов
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
			this.Text = AppStrings.MainForm_Title;
			this.notifyIcon1.Text = AppStrings.MainForm_Title;

			this.minutesField.Caption = AppStrings.MainForm_MinutesShort;
			this.secondsField.Caption = AppStrings.MainForm_SecondsShort;
		}

		/// <summary>
		/// Изменение минут или секунд
		/// </summary>
		private void TimeField_ValueChanged(object? sender, EventArgs e)
		{
			this.UpdateStartButtonState();
		}

		/// <summary>
		/// Общее время в секундах
		/// </summary>
		private int GetTotalSeconds()
		{
			return (this.minutesField.Value * 60) + this.secondsField.Value;
		}

		/// <summary>
		/// Доступность кнопки Старт
		/// </summary>
		private void UpdateStartButtonState()
		{
			bool hasTime = this.GetTotalSeconds() > 0;

			this.btnStart.Enabled = hasTime;
			this.btnStart.BackColor = hasTime ? Theme.ButtonPrimary : Theme.FieldBackground;
			this.btnStart.ForeColor = hasTime ? Theme.TextColor : Theme.FieldCaption;
		}

		/// <summary>
		/// Обработка нажатия кнопки Старт: запуск таймера в мини-окне
		/// </summary>
		private void btnStart_Click(object? sender, EventArgs e)
		{
			int seconds = this.GetTotalSeconds();

			if (seconds <= 0)
			{
				MessageBox.Show(AppStrings.MainForm_TimeZeroWarning, AppStrings.Common_WarningTitle);
				return;
			}

			_audioService.Stop();

			this.ShowMiniTimer(TimeSpan.FromSeconds(seconds));
		}

		/// <summary>
		/// Показ мини-окна и скрытие главной формы
		/// </summary>
		private void ShowMiniTimer(TimeSpan duration)
		{
			if (_miniTimerForm != null && !_miniTimerForm.IsDisposed)
			{
				_miniTimerForm.Activate();
				return;
			}

			Screen screen = Screen.FromControl(this);

			_miniTimerForm = new MiniTimerForm(screen);
			_miniTimerForm.TimerFinished += this.MiniTimerForm_TimerFinished;
			_miniTimerForm.FormClosed += this.MiniTimerForm_FormClosed;

			this.Hide();

			_miniTimerForm.Show();
			_miniTimerForm.StartTimer(duration);
		}

		/// <summary>
		/// Окончание таймера в мини-окне
		/// </summary>
		private void MiniTimerForm_TimerFinished(object? sender, EventArgs e)
		{
			this.PlayEndSound();
		}

		/// <summary>
		/// Возврат на главную форму после закрытия мини-окна
		/// </summary>
		private void MiniTimerForm_FormClosed(object? sender, FormClosedEventArgs e)
		{
			if (_miniTimerForm != null)
			{
				_miniTimerForm.TimerFinished -= this.MiniTimerForm_TimerFinished;
				_miniTimerForm.FormClosed -= this.MiniTimerForm_FormClosed;
				_miniTimerForm = null;
			}

			if (this.IsDisposed || this.Disposing)
			{
				return;
			}

			this.Show();
			this.Activate();
			this.UpdateStartButtonState();
		}

		/// <summary>
		/// Воспроизведение мелодии окончания таймера
		/// </summary>
		private void PlayEndSound()
		{
			if (!string.IsNullOrEmpty(SettingsService.CustomSoundPath) && File.Exists(SettingsService.CustomSoundPath))
			{
				try
				{
					_audioService.PlayCustomFile(SettingsService.CustomSoundPath);
				}
				catch (Exception ex)
				{
					MessageBox.Show(
						string.Format(AppStrings.MainForm_AudioPlaybackError, ex.Message),
						AppStrings.Common_ErrorTitle);

					_audioService.PlayEmbedded(Properties.Resources.DefaultEndTimerMusic);
				}
			}
			else
			{
				_audioService.PlayEmbedded(Properties.Resources.DefaultEndTimerMusic);
			}
		}

		/// <summary>
		/// Обработчик нажатия кнопки Настройки
		/// </summary>
		private void btnSettings_Click(object? sender, EventArgs e)
		{
			using (SettingsForm settings = new SettingsForm())
			{
				settings.ShowDialog();
			}
		}

		/// <summary>
		/// Обработчик нажатия кнопки Информация
		/// </summary>
		private void btnInfo_Click(object? sender, EventArgs e)
		{
			using (InfoForm info = new InfoForm())
			{
				info.ShowDialog();
			}
		}
	}
}