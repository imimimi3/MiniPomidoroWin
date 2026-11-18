using MiniPomidoroWin.Constants;
using MiniPomidoroWin.Forms;
using MiniPomidoroWin.Forms.Localization;
using MiniPomidoroWin.Service;
using MiniPomidoroWin.Themes;
using System.Diagnostics;
using System.Media;

namespace MiniPomidoroWin
{
	public partial class MainForm : LocalizedForm
	{
		/// <summary>
		/// Сервис звука
		/// </summary>
		private readonly AudioService _audioService = new AudioService();

		/// <summary>
		/// Обновление подсказки значка в трее
		/// </summary>
		private readonly System.Windows.Forms.Timer _trayTooltipTimer =
			new System.Windows.Forms.Timer { Interval = AppConstants.TrayTooltipUpdateIntervalMs };

		/// <summary>
		/// Напоминание, когда таймер не запущен
		/// </summary>
		private readonly System.Windows.Forms.Timer _reminderTimer =
			new System.Windows.Forms.Timer { Interval = AppConstants.TrayReminderIntervalMs };

		/// <summary>
		/// Мини-окно с таймером обратного отсчёта
		/// </summary>
		private MiniTimerForm? _miniTimerForm;

		/// <summary>
		/// Сообщение об окончании таймера
		/// </summary>
		private NotificationForm? _notificationForm;

		/// <summary>
		/// Приложение свёрнуто в трей
		/// </summary>
		private bool _isInTray;

		/// <summary>
		/// Приложение завершает работу
		/// </summary>
		private bool _exitRequested;

		/// <summary>
		/// Конструктор формы
		/// </summary>
		public MainForm()
		{
			InitializeComponent();

			this.BackColor = Theme.FormBackground;

			this.minutesField.ValueChanged += this.TimeField_ValueChanged;
			this.secondsField.ValueChanged += this.TimeField_ValueChanged;

			this._trayTooltipTimer.Tick += this.TrayTooltipTimer_Tick;
			this._reminderTimer.Tick += this.ReminderTimer_Tick;

			this._trayTooltipTimer.Start();
			this._reminderTimer.Start();

			this.ApplyLocalization();
			this.UpdateStartButtonState();
		}

		/// <summary>
		/// Перехват закрытия главной формы: крестик сворачивает приложение в трей
		/// </summary>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (!this._exitRequested && e.CloseReason == CloseReason.UserClosing)
			{
				e.Cancel = true;
				this.HideMainFormToTray();
				return;
			}

			this._exitRequested = true;

			base.OnFormClosing(e);
		}

		/// <summary>
		/// Освобождение ресурсов
		/// </summary>
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			this._trayTooltipTimer.Stop();
			this._trayTooltipTimer.Dispose();

			this._reminderTimer.Stop();
			this._reminderTimer.Dispose();

			this._audioService.Dispose();

			if (this._notificationForm != null && !this._notificationForm.IsDisposed)
			{
				this._notificationForm.Close();
			}

			this.notifyIcon1.Visible = false;

			base.OnFormClosed(e);
		}

		/// <summary>
		/// Применение локализованных строк к элементам формы
		/// </summary>
		protected override void ApplyLocalization()
		{
			this.Text = AppStrings.MainForm_Title;

			this.notifyMenuShow.Text = AppStrings.Tray_Show;
			this.notifyMenuExit.Text = AppStrings.Tray_Exit;

			this.minutesField.Caption = AppStrings.MainForm_MinutesShort;
			this.secondsField.Caption = AppStrings.MainForm_SecondsShort;

			this.UpdateTrayText();
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
				MessageBox.Show(
					this,
					AppStrings.MainForm_TimeZeroWarning,
					AppStrings.Common_WarningTitle,
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning);

				return;
			}

			this._audioService.Stop();

			this.ShowMiniTimer(TimeSpan.FromSeconds(seconds));
		}

		/// <summary>
		/// Показ мини-окна и скрытие главной формы
		/// </summary>
		private void ShowMiniTimer(TimeSpan duration)
		{
			if (this._miniTimerForm != null && !this._miniTimerForm.IsDisposed)
			{
				this._isInTray = false;

				this.Hide();
				this._miniTimerForm.Show();
				this._miniTimerForm.Activate();
				this._miniTimerForm.BringToFront();

				this.UpdateTrayText();

				return;
			}

			Screen screen = Screen.FromControl(this);

			this._miniTimerForm = new MiniTimerForm(screen);
			this._miniTimerForm.TimerFinished += this.MiniTimerForm_TimerFinished;
			this._miniTimerForm.FormClosed += this.MiniTimerForm_FormClosed;
			this._miniTimerForm.CollapseRequested += this.MiniTimerForm_CollapseRequested;

			this._isInTray = false;

			this.Hide();

			this._miniTimerForm.Show();
			this._miniTimerForm.StartTimer(duration);

			this.UpdateTrayText();
		}

		/// <summary>
		/// Окончание таймера в мини-окне
		/// </summary>
		private void MiniTimerForm_TimerFinished(object? sender, EventArgs e)
		{
			// Сначала сообщение, только потом звук
			this.ShowTimerFinishedNotification();
			this.PlayEndSound();
		}

		/// <summary>
		/// Сворачивание мини-окна в трей
		/// </summary>
		private void MiniTimerForm_CollapseRequested(object? sender, EventArgs e)
		{
			if (this._miniTimerForm == null || this._miniTimerForm.IsDisposed)
			{
				return;
			}

			this._isInTray = true;

			this._miniTimerForm.Hide();

			if (this.Visible)
			{
				this.Hide();
			}

			this.UpdateTrayText();
		}

		/// <summary>
		/// Возврат на главную форму после закрытия мини-окна
		/// </summary>
		private void MiniTimerForm_FormClosed(object? sender, FormClosedEventArgs e)
		{
			if (this._miniTimerForm != null)
			{
				this._miniTimerForm.TimerFinished -= this.MiniTimerForm_TimerFinished;
				this._miniTimerForm.FormClosed -= this.MiniTimerForm_FormClosed;
				this._miniTimerForm.CollapseRequested -= this.MiniTimerForm_CollapseRequested;
				this._miniTimerForm = null;
			}

			if (this._exitRequested || this.IsDisposed || this.Disposing)
			{
				return;
			}

			this.UpdateTrayText();

			if (this._isInTray)
			{
				return;
			}

			this.Show();

			if (this.WindowState == FormWindowState.Minimized)
			{
				this.WindowState = FormWindowState.Normal;
			}

			this.Activate();
			this.BringToFront();
			this.UpdateStartButtonState();
		}

		/// <summary>
		/// Обновление подсказки значка в трее
		/// </summary>
		private void TrayTooltipTimer_Tick(object? sender, EventArgs e)
		{
			this.UpdateTrayText();
		}

		/// <summary>
		/// Текст подсказки: оставшееся время или «Готов к труду»
		/// </summary>
		private void UpdateTrayText()
		{
			string text = AppStrings.Tray_Ready;

			if (this._miniTimerForm is { IsDisposed: false } miniTimer && miniTimer.IsTimerActive)
			{
				text = string.Format(AppStrings.Tray_TimeLeftFormat, FormatTime(miniTimer.TimeLeft));
			}

			if (!string.Equals(this.notifyIcon1.Text, text, StringComparison.Ordinal))
			{
				this.notifyIcon1.Text = text;
			}
		}

		/// <summary>
		/// Форматирование оставшегося времени
		/// </summary>
		private static string FormatTime(TimeSpan value)
		{
			return value.TotalHours >= 1
				? value.ToString(@"hh\:mm\:ss")
				: value.ToString(@"mm\:ss");
		}

		/// <summary>
		/// Напоминание раз в 10 минут, если таймер не запущен
		/// </summary>
		private void ReminderTimer_Tick(object? sender, EventArgs e)
		{
			if (this._miniTimerForm is { IsDisposed: false } miniTimer && miniTimer.IsTimerActive)
			{
				return;
			}

			this.ShowTrayNotification(AppStrings.Tray_Reminder);
		}

		/// <summary>
		/// Всплывающее уведомление из трея
		/// </summary>
		private void ShowTrayNotification(string message)
		{
			this.notifyIcon1.BalloonTipTitle = AppStrings.MainForm_Title;
			this.notifyIcon1.BalloonTipText = message;
			this.notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
			this.notifyIcon1.ShowBalloonTip(5000);
		}

		/// <summary>
		/// Двойной клик по значку в трее
		/// </summary>
		private void notifyIcon1_MouseDoubleClick(object? sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this.RestoreFromTray();
			}
		}

		/// <summary>
		/// Пункт меню «Развернуть»
		/// </summary>
		private void notifyMenuShow_Click(object? sender, EventArgs e)
		{
			this.RestoreFromTray();
		}

		/// <summary>
		/// Пункт меню «Выход»
		/// </summary>
		private void notifyMenuExit_Click(object? sender, EventArgs e)
		{
			this._exitRequested = true;
			this.notifyIcon1.Visible = false;

			Application.Exit();
		}

		/// <summary>
		/// Разворачивание приложения из трея
		/// </summary>
		private void RestoreFromTray()
		{
			if (this._exitRequested)
			{
				return;
			}

			this._isInTray = false;

			if (this._miniTimerForm is { IsDisposed: false } miniTimer)
			{
				if (!miniTimer.Visible)
				{
					miniTimer.Show();
				}

				if (miniTimer.WindowState == FormWindowState.Minimized)
				{
					miniTimer.WindowState = FormWindowState.Normal;
				}

				miniTimer.Activate();
				miniTimer.BringToFront();

				this.UpdateTrayText();

				return;
			}

			this.Show();

			if (this.WindowState == FormWindowState.Minimized)
			{
				this.WindowState = FormWindowState.Normal;
			}

			this.Activate();
			this.BringToFront();

			this.UpdateStartButtonState();
			this.UpdateTrayText();
		}

		/// <summary>
		/// Скрытие главной формы в трей
		/// </summary>
		private void HideMainFormToTray()
		{
			this._isInTray = true;
			this.Hide();
			this.UpdateTrayText();
		}

		/// <summary>
		/// Воспроизведение мелодии окончания таймера
		/// </summary>
		private void PlayEndSound()
		{
			// Файл мог исчезнуть с диска — тогда звучит встроенная мелодия
			if (SettingsService.HasCustomSound && SettingsService.CustomSoundPath is string path)
			{
				try
				{
					this._audioService.PlayCustomFile(path);
					return;
				}
				catch (Exception ex)
				{
					MessageBox.Show(
						this,
						string.Format(AppStrings.MainForm_AudioPlaybackError, ex.Message),
						AppStrings.Common_ErrorTitle,
						MessageBoxButtons.OK,
						MessageBoxIcon.Error);
				}
			}

			try
			{
				this._audioService.PlayEmbedded(Properties.Resources.DefaultEndTimerMusic);
			}
			catch (Exception ex)
			{
				// Мелодия не заиграла — хотя бы системный звук и запись в лог
				Trace.TraceError("Не удалось воспроизвести встроенную мелодию: {0}", ex);
				SystemSounds.Exclamation.Play();
			}
		}

		/// <summary>
		/// Сообщение «Пора переключиться!» поверх всех окон
		/// </summary>
		private void ShowTimerFinishedNotification()
		{
			if (this._exitRequested || this.IsDisposed || this.Disposing)
			{
				return;
			}

			if (this._notificationForm != null && !this._notificationForm.IsDisposed)
			{
				this._notificationForm.Close();
			}

			NotificationForm notification = new NotificationForm();
			notification.FormClosed += this.NotificationForm_FormClosed;

			this._notificationForm = notification;

			notification.Show();
			notification.Refresh();
			notification.Activate();
			notification.BringToFront();
		}

		/// <summary>
		/// Закрытие сообщения
		/// </summary>
		private void NotificationForm_FormClosed(object? sender, FormClosedEventArgs e)
		{
			if (sender is NotificationForm notification)
			{
				notification.FormClosed -= this.NotificationForm_FormClosed;
			}

			if (ReferenceEquals(this._notificationForm, sender))
			{
				this._notificationForm = null;
			}
		}

		/// <summary>
		/// Обработчик нажатия кнопки Настройки
		/// </summary>
		private void btnSettings_Click(object? sender, EventArgs e)
		{
			using (SettingsForm settings = new SettingsForm())
			{
				settings.ShowDialog(this);
			}
		}

		/// <summary>
		/// Обработчик нажатия кнопки Информация
		/// </summary>
		private void btnInfo_Click(object? sender, EventArgs e)
		{
			using (InfoForm info = new InfoForm())
			{
				info.ShowDialog(this);
			}
		}
	}
}