using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MiniPomidoroWin.Themes;

namespace MiniPomidoroWin.Controls
{
	/// <summary>
	/// Красивый круговой таймер
	/// </summary>
	public class CircularTimer : Control
	{
		/// <summary>
		/// Основной таймер
		/// </summary>
		private System.Windows.Forms.Timer timer;

		/// <summary>
		/// Общее время
		/// </summary>
		private TimeSpan totalTime;

		/// <summary>
		/// Прошедшее время
		/// </summary>
		private TimeSpan timeLeft;

		/// <summary>
		/// Толщина
		/// </summary>
		[DefaultValue(15)]
		public int Thickness { get; set; } = 15;

		/// <summary>
		/// Событие окончания таймера
		/// </summary>
		public event EventHandler? TimeFinished;

		public TimeSpan TimeLeft => timeLeft;
		public TimeSpan TotalTime => totalTime;

		/// <summary>
		/// Запущен ли
		/// </summary>
		public bool IsRunning => timer.Enabled && timeLeft.TotalMilliseconds > 0;

		/// <summary>
		/// На паузе ли
		/// </summary>
		public bool IsPaused => !timer.Enabled && timeLeft.TotalMilliseconds > 0;

		/// <summary>
		/// Конструктор
		/// </summary>
		public CircularTimer()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);

			timer = new System.Windows.Forms.Timer();
			timer.Interval = 100;
			timer.Tick += Timer_Tick;
			Size = new Size(200, 200);
		}

		/// <summary>
		/// Начало таймера
		/// </summary>
		public void Start(TimeSpan duration)
		{
			totalTime = duration;
			timeLeft = duration;
			timer.Start();
			Invalidate();
		}

		/// <summary>
		/// Пауза
		/// </summary>
		public void Pause()
		{
			timer.Stop();
		}

		/// <summary>
		/// Продолжение таймера
		/// </summary>
		public void Resume()
		{
			if (timeLeft.TotalMilliseconds > 0)
				timer.Start();
		}

		/// <summary>
		/// Сброс таймера
		/// </summary>
		public void Stop()
		{
			timer.Stop();
			timeLeft = TimeSpan.Zero;
			totalTime = TimeSpan.Zero;
			Invalidate();
		}

		/// <summary>
		/// Тик таймера
		/// </summary>
		private void Timer_Tick(object? sender, EventArgs e)
		{
			timeLeft = timeLeft.Subtract(TimeSpan.FromMilliseconds(timer.Interval));

			if (timeLeft.TotalMilliseconds <= 0)
			{
				timeLeft = TimeSpan.Zero;
				timer.Stop();
				TimeFinished?.Invoke(this, EventArgs.Empty);
			}

			Invalidate();
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				timer?.Stop();
				timer?.Dispose();
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// Рисование кругового таймера
		/// </summary>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Graphics g = e.Graphics;

			// Включаем сглаживание (чтобы круги были ровными, без "лесенок")
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			// 1. Рисуем фоновую дорожку (серый круг)
			Rectangle rect = new Rectangle(Thickness / 2, Thickness / 2, Width - Thickness, Height - Thickness);

			using (Pen trackPen = new Pen(Theme.TimerTrack, Thickness))
			{
				g.DrawEllipse(trackPen, rect);
			}

			// 2. Рисуем прогресс (цветной круг)
			if (totalTime.TotalMilliseconds > 0)
			{
				float progress = (float)(timeLeft.TotalMilliseconds / totalTime.TotalMilliseconds);
				float sweepAngle = - progress * 360f;

				using (Pen progressPen = new Pen(Theme.TimerProgress, Thickness))
				{
					progressPen.StartCap = LineCap.Round;
					progressPen.EndCap = LineCap.Round;

					// Начинаем рисовать с 12 часов (угол -90 градусов)
					g.DrawArc(progressPen, rect, -90, sweepAngle);
				}
			}

			// 3. Рисуем текст (оставшееся время)
			string timeText = timeLeft.ToString(@"mm\:ss");
			if (timeLeft.TotalHours >= 1) timeText = timeLeft.ToString(@"hh\:mm\:ss");

			using (Font font = new Font("Segoe UI", 28, FontStyle.Bold))
			using (Brush textBrush = new SolidBrush(Theme.TextColor))
			using (StringFormat sf = new StringFormat())
			{
				sf.Alignment = StringAlignment.Center;
				sf.LineAlignment = StringAlignment.Center;

				RectangleF textRect = new RectangleF(0, 0, Width, Height);
				g.DrawString(timeText, font, textBrush, textRect, sf);
			}
		}

	}
}
