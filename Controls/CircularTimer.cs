using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MiniPomidoroWin.Themes;

namespace MiniPomidoroWin.Controls
{
	/// <summary>
	/// Круговой таймер обратного отсчёта
	/// </summary>
	public class CircularTimer : Control
	{
		/// <summary>
		/// Доля диаметра, которую занимает высота шрифта отсчёта
		/// </summary>
		private const float TextScale = 0.22f;

		/// <summary>
		/// Основной таймер
		/// </summary>
		private readonly System.Windows.Forms.Timer timer;

		/// <summary>
		/// Форматирование текста по центру без переноса
		/// </summary>
		private readonly StringFormat textFormat;

		/// <summary>
		/// Общее время
		/// </summary>
		private TimeSpan totalTime;

		/// <summary>
		/// Оставшееся время
		/// </summary>
		private TimeSpan timeLeft;

		/// <summary>
		/// Толщина кольца
		/// </summary>
		private int thickness = 15;

		/// <summary>
		/// Толщина кольца
		/// </summary>
		[DefaultValue(15)]
		[Category("Appearance")]
		public int Thickness
		{
			get => thickness;
			set
			{
				int newValue = Math.Max(1, value);

				if (thickness == newValue)
				{
					return;
				}

				thickness = newValue;
				Invalidate();
			}
		}

		/// <summary>
		/// Событие окончания таймера
		/// </summary>
		public event EventHandler? TimeFinished;

		/// <summary>
		/// Оставшееся время
		/// </summary>
		public TimeSpan TimeLeft => timeLeft;

		/// <summary>
		/// Общее время
		/// </summary>
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
			SetStyle(
				ControlStyles.AllPaintingInWmPaint |
				ControlStyles.OptimizedDoubleBuffer |
				ControlStyles.UserPaint |
				ControlStyles.ResizeRedraw,
				true);

			textFormat = new StringFormat
			{
				Alignment = StringAlignment.Center,
				LineAlignment = StringAlignment.Center,
				FormatFlags = StringFormatFlags.NoWrap
			};

			timer = new System.Windows.Forms.Timer { Interval = 100 };
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
			{
				timer.Start();
			}
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
				Invalidate();
				TimeFinished?.Invoke(this, EventArgs.Empty);
				return;
			}

			Invalidate();
		}

		/// <summary>
		/// Освобождение ресурсов
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				timer.Stop();
				timer.Dispose();
				textFormat.Dispose();
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
			int diameter = Math.Min(Width, Height);

			if (diameter <= thickness * 2 + 8)
			{
				return;
			}

			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			Rectangle rect = new Rectangle(
				(Width - diameter) / 2 + thickness / 2,
				(Height - diameter) / 2 + thickness / 2,
				diameter - thickness,
				diameter - thickness);

			// Фоновая дорожка
			using (Pen trackPen = new Pen(Theme.TimerTrack, thickness))
			{
				g.DrawEllipse(trackPen, rect);
			}

			// Прогресс
			if (totalTime > TimeSpan.Zero)
			{
				float progress = (float)(timeLeft.TotalMilliseconds / totalTime.TotalMilliseconds);

				if (progress > 0.001f)
				{
					using (Pen progressPen = new Pen(Theme.TimerProgress, thickness))
					{
						progressPen.StartCap = LineCap.Round;
						progressPen.EndCap = LineCap.Round;

						g.DrawArc(progressPen, rect, -90f, -progress * 360f);
					}
				}
			}

			// Текст
			string timeText = this.FormatTime(timeLeft);

			using (Font font = this.CreateTimeFont(g, timeText, diameter))
			using (Brush textBrush = new SolidBrush(Theme.TextColor))
			{
				g.DrawString(
					timeText,
					font,
					textBrush,
					new RectangleF(0f, 0f, Width, Height),
					textFormat);
			}
		}

		/// <summary>
		/// Форматирование оставшегося времени
		/// </summary>
		private string FormatTime(TimeSpan value)
		{
			return value.TotalHours >= 1
				? value.ToString(@"hh\:mm\:ss")
				: value.ToString(@"mm\:ss");
		}

		/// <summary>
		/// Шрифт отсчёта, подобранный под размер круга и длину строки
		/// </summary>
		private Font CreateTimeFont(Graphics g, string text, int diameter)
		{
			// Размер в пикселях: шрифт масштабируется вместе с контролом, а не с DPI
			float size = Math.Max(8f, diameter * TextScale);

			Font font = new Font("Segoe UI", size, FontStyle.Bold, GraphicsUnit.Pixel);

			float maxWidth = diameter - thickness * 2 - diameter * 0.08f;
			float maxHeight = diameter - thickness * 2 - diameter * 0.12f;

			SizeF measured = g.MeasureString(text, font);
			float factor = 1f;

			if (maxWidth > 0f && measured.Width > maxWidth)
			{
				factor = Math.Min(factor, maxWidth / measured.Width);
			}

			if (maxHeight > 0f && measured.Height > maxHeight)
			{
				factor = Math.Min(factor, maxHeight / measured.Height);
			}

			if (factor < 1f)
			{
				font.Dispose();
				font = new Font("Segoe UI", Math.Max(6f, size * factor), FontStyle.Bold, GraphicsUnit.Pixel);
			}

			return font;
		}
	}
}