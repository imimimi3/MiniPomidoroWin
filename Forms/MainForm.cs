using MiniPomidoroWin.Constants;
using MiniPomidoroWin.Forms;
using MiniPomidoroWin.Forms.Localization;
using MiniPomidoroWin.Service;
using MiniPomidoroWin.Themes;
using NAudio.Wave;
using System.ComponentModel;
using System.IO;

namespace MiniPomidoroWin
{
	public partial class MainForm : Form, INotifyPropertyChanged
	{
		/// <summary>
		/// Сервис звука
		/// </summary>
		private readonly AudioService _audioService = new AudioService();

		/// <summary>
		/// Иконка кнопки плей/пауза
		/// </summary>
		private string _btnPlayPauseText = AppConstants.PlayIcon;

		/// <summary>
		/// Мини-окно с таймером обратного отсчёта
		/// </summary>
		private MiniTimerForm? _miniTimerForm;

		/// <summary>
		/// Изменение иконки кнопки плей/пауза
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string PlayPauseButtonText
		{
			get => _btnPlayPauseText;
			set
			{
				_btnPlayPauseText = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Конструктор формы
		/// </summary>
		public MainForm()
		{
			InitializeComponent();

			// Настройка темы формы
			this.BackColor = Theme.FormBackground;

			btnPlayPause.DataBindings.Add(
			   nameof(btnPlayPause.Text),
			   this,
			   nameof(PlayPauseButtonText),
			   false,
			   DataSourceUpdateMode.OnPropertyChanged
		   );

			LocalizationService.Instance.LanguageChanged += LocalizationService_LanguageChanged;

			this.ApplyLocalization();
		}

		/// <summary>
		/// Событие изменения свойств
		/// </summary>
		public event PropertyChangedEventHandler? PropertyChanged;

		protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

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
			this.Text = AppStrings.MainForm_Title;
			notifyIcon1.Text = AppStrings.MainForm_Title;
			lblMinutes.Text = AppStrings.MainForm_MinutesLabel;
			lblSeconds.Text = AppStrings.MainForm_SecondsLabel;
			btnTimerStop.Text = AppStrings.MainForm_StopButton;
		}

		/// <summary>
		/// Обработка нажатия кнопки Старт: запуск таймера в мини-окне
		/// </summary>
		private void btnPlayPause_Click(object sender, EventArgs e)
		{
			int seconds = (Convert.ToInt32(minutesNum.Value) * 60) + Convert.ToInt32(secondsNum.Value);

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
			_miniTimerForm.TimerFinished += MiniTimerForm_TimerFinished;
			_miniTimerForm.FormClosed += MiniTimerForm_FormClosed;

			PlayPauseButtonText = AppConstants.PauseIcon;

			this.Hide();

			_miniTimerForm.Show();
			_miniTimerForm.StartTimer(duration);
		}

		/// <summary>
		/// Окончание таймера в мини-окне
		/// </summary>
		private void MiniTimerForm_TimerFinished(object? sender, EventArgs e)
		{
			PlayPauseButtonText = AppConstants.PlayIcon;
			this.PlayEndSound();
		}

		/// <summary>
		/// Возврат на главную форму после закрытия мини-окна
		/// </summary>
		private void MiniTimerForm_FormClosed(object? sender, FormClosedEventArgs e)
		{
			if (_miniTimerForm != null)
			{
				_miniTimerForm.TimerFinished -= MiniTimerForm_TimerFinished;
				_miniTimerForm.FormClosed -= MiniTimerForm_FormClosed;
				_miniTimerForm = null;
			}

			if (this.IsDisposed || this.Disposing)
			{
				return;
			}

			PlayPauseButtonText = AppConstants.PlayIcon;

			this.Show();
			this.Activate();
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
		/// Обработчик нажатия кнопки Стоп
		/// </summary>
		private void btnTimerStop_Click(object sender, EventArgs e)
		{
			circularTimer.Stop();
			PlayPauseButtonText = AppConstants.PlayIcon;
			_audioService.Stop();
		}

		/// <summary>
		/// Обработчик нажатия кнопки Настройки
		/// </summary>
		private void btnSettings_Click(object sender, EventArgs e)
		{
			using (SettingsForm settings = new SettingsForm())
			{
				settings.ShowDialog();
			}
		}

		/// <summary>
		/// Обработчик нажатия кнопки Информация
		/// </summary>
		private void btnInfo_Click(object sender, EventArgs e)
		{
			using (InfoForm info = new InfoForm())
			{
				info.ShowDialog();
			}
		}

		/// <summary>
		/// Выключение аудио при закрытии формы
		/// </summary>
		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			_audioService.Dispose();
		}
	}
}