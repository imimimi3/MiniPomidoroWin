using MiniPomidoroWin.Constants;
using MiniPomidoroWin.Forms;
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

			circularTimer.TimeFinished += CircularTimer_TimeFinished;

			btnPlayPause.DataBindings.Add(
			   nameof(btnPlayPause.Text),
			   this,
			   nameof(PlayPauseButtonText),
			   false,
			   DataSourceUpdateMode.OnPropertyChanged
		   );
		}

		/// <summary>
		/// Событие изменения свойств
		/// </summary>
		public event PropertyChangedEventHandler? PropertyChanged;
		protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		/// <summary>
		/// Окончание таймера - воспроизведение звука
		/// </summary>
		private void CircularTimer_TimeFinished(object? sender, EventArgs e)
		{
			PlayPauseButtonText = AppConstants.PlayIcon;

			if (!string.IsNullOrEmpty(SettingsService.CustomSoundPath) && File.Exists(SettingsService.CustomSoundPath))
			{
				try
				{
					_audioService.PlayCustomFile(SettingsService.CustomSoundPath);
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Не удалось воспроизвести выбранный файл: {ex.Message}\nБудет использован стандартный звук.", "Ошибка");
					_audioService.PlayEmbedded(Properties.Resources.DefaultEndTimerMusic);
				}
			}
			else
			{
				_audioService.PlayEmbedded(Properties.Resources.DefaultEndTimerMusic);
			}
		}

		/// <summary>
		/// Обработка нажатия кнопки Плей/пауза
		/// </summary>
		private void btnPlayPause_Click(object sender, EventArgs e)
		{
			if (circularTimer.IsRunning)
			{
				circularTimer.Pause();
				PlayPauseButtonText = AppConstants.PlayIcon;
			}
			else if (circularTimer.IsPaused)
			{
				circularTimer.Resume();
				PlayPauseButtonText = AppConstants.PauseIcon;
			}
			else
			{
				int seconds = (Convert.ToInt32(minutesNum.Value) * 60) + Convert.ToInt32(secondsNum.Value);

				if (seconds <= 0)
				{
					MessageBox.Show("Установите время больше 0!", "Внимание");
					return;
				}

				circularTimer.Start(TimeSpan.FromSeconds(seconds));
				PlayPauseButtonText = AppConstants.PauseIcon;
				_audioService.Stop();
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
