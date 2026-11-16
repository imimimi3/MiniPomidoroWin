using MiniPomidoroWin.Constants;
using MiniPomidoroWin.Controls;
using MiniPomidoroWin.Forms.Localization;
using MiniPomidoroWin.Themes;

namespace MiniPomidoroWin.Forms
{
	/// <summary>
	/// Мини-окно с таймером обратного отсчёта
	/// </summary>
	public partial class MiniTimerForm : Form
	{
		/// <summary>
		/// Экран, в правом нижнем углу которого показывается окно
		/// </summary>
		private readonly Screen _screen;

		/// <summary>
		/// Конструктор для дизайнера
		/// </summary>
		public MiniTimerForm() : this(Screen.FromPoint(Cursor.Position))
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		public MiniTimerForm(Screen screen)
		{
			InitializeComponent();

			_screen = screen;

			this.BackColor = Theme.FormBackground;
			this.Text = AppStrings.MainForm_Title;

			circularTimer.TimeFinished += CircularTimer_TimeFinished;

			btnPause.Text = AppConstants.PauseIcon;
			btnStop.Text = AppConstants.StopIcon;
		}

		/// <summary>
		/// Событие окончания таймера
		/// </summary>
		public event EventHandler? TimerFinished;

		/// <summary>
		/// Запуск обратного отсчёта
		/// </summary>
		public void StartTimer(TimeSpan duration)
		{
			circularTimer.Start(duration);
			btnPause.Text = AppConstants.PauseIcon;
		}

		/// <summary>
		/// Позиционирование окна в правом нижнем углу экрана
		/// </summary>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			Rectangle area = _screen.WorkingArea;

			this.Location = new Point(
				area.Right - this.Width - AppConstants.MiniFormScreenMargin,
				area.Bottom - this.Height - AppConstants.MiniFormScreenMargin);
		}

		/// <summary>
		/// Окончание таймера
		/// </summary>
		private void CircularTimer_TimeFinished(object? sender, EventArgs e)
		{
			this.TimerFinished?.Invoke(this, EventArgs.Empty);
			this.Close();
		}

		/// <summary>
		/// Пауза и продолжение отсчёта
		/// </summary>
		private void btnPause_Click(object sender, EventArgs e)
		{
			if (circularTimer.IsRunning)
			{
				circularTimer.Pause();
				btnPause.Text = AppConstants.PlayIcon;
			}
			else if (circularTimer.IsPaused)
			{
				circularTimer.Resume();
				btnPause.Text = AppConstants.PauseIcon;
			}
		}

		/// <summary>
		/// Остановка таймера и возврат на главную форму
		/// </summary>
		private void btnStop_Click(object sender, EventArgs e)
		{
			circularTimer.Stop();
			this.Close();
		}
	}
}
