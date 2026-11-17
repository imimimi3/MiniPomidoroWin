using System.Drawing.Drawing2D;
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
		/// Радиус скругления окна
		/// </summary>
		private const float CornerRadius = 24f;

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

			this.circularTimer.TimeFinished += this.CircularTimer_TimeFinished;
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
			this.circularTimer.Start(duration);
			this.btnPause.Glyph = IconGlyph.Pause;
		}

		/// <summary>
		/// Позиционирование окна и скругление углов
		/// </summary>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			Rectangle area = _screen.WorkingArea;
			int margin = (int)Math.Round(AppConstants.MiniFormScreenMargin * this.DeviceDpi / 96.0);

			this.Location = new Point(
				area.Right - this.Width - margin,
				area.Bottom - this.Height - margin);

			this.UpdateRoundedRegion();
		}

		/// <summary>
		/// Обновление скруглённой области окна при изменении размеров
		/// </summary>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			this.UpdateRoundedRegion();
		}

		/// <summary>
		/// Обработка клавиши Escape
		/// </summary>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (e.KeyCode != Keys.Escape)
			{
				return;
			}

			e.Handled = true;
			this.StopTimer();
		}

		/// <summary>
		/// Отписка от события таймера
		/// </summary>
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			this.circularTimer.TimeFinished -= this.CircularTimer_TimeFinished;
			base.OnFormClosed(e);
		}

		/// <summary>
		/// Скругление углов окна
		/// </summary>
		private void UpdateRoundedRegion()
		{
			if (this.ClientSize.Width <= 0 || this.ClientSize.Height <= 0)
			{
				return;
			}

			RectangleF bounds = new RectangleF(0f, 0f, this.ClientSize.Width, this.ClientSize.Height);

			using (GraphicsPath path = RoundedRectangle.Create(bounds, CornerRadius))
			{
				Region? previous = this.Region;
				this.Region = new Region(path);
				previous?.Dispose();
			}
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
		private void btnPause_Click(object? sender, EventArgs e)
		{
			if (this.circularTimer.IsRunning)
			{
				this.circularTimer.Pause();
				this.btnPause.Glyph = IconGlyph.Play;
			}
			else if (this.circularTimer.IsPaused)
			{
				this.circularTimer.Resume();
				this.btnPause.Glyph = IconGlyph.Pause;
			}
		}

		/// <summary>
		/// Остановка таймера и закрытие окна
		/// </summary>
		private void btnStop_Click(object? sender, EventArgs e)
		{
			this.StopTimer();
		}

		/// <summary>
		/// Остановка таймера и закрытие окна
		/// </summary>
		private void StopTimer()
		{
			this.circularTimer.Stop();
			this.Close();
		}
	}
}