// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 imimimi3

using System.ComponentModel;
using System.Diagnostics;
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
		/// Основной таймер перерисовки
		/// </summary>
		private readonly System.Windows.Forms.Timer timer;

		/// <summary>
		/// Точный отсчёт прошедшего времени
		/// </summary>
		private readonly Stopwatch stopwatch = new Stopwatch();

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
			get => this.thickness;
			set
			{
				int newValue = Math.Max(1, value);

				if (this.thickness == newValue)
				{
					return;
				}

				this.thickness = newValue;
				this.Invalidate();
			}
		}

		/// <summary>
		/// Событие окончания таймера
		/// </summary>
		public event EventHandler? TimeFinished;

		/// <summary>
		/// Оставшееся время
		/// </summary>
		public TimeSpan TimeLeft => this.timeLeft;

		/// <summary>
		/// Общее время
		/// </summary>
		public TimeSpan TotalTime => this.totalTime;

		/// <summary>
		/// Запущен ли
		/// </summary>
		public bool IsRunning => this.timer.Enabled && this.timeLeft > TimeSpan.Zero;

		/// <summary>
		/// На паузе ли
		/// </summary>
		public bool IsPaused => !this.timer.Enabled && this.timeLeft > TimeSpan.Zero;

		/// <summary>
		/// Конструктор
		/// </summary>
		public CircularTimer()
		{
			this.SetStyle(
				ControlStyles.AllPaintingInWmPaint |
				ControlStyles.OptimizedDoubleBuffer |
				ControlStyles.UserPaint |
				ControlStyles.ResizeRedraw,
				true);

			this.textFormat = new StringFormat
			{
				Alignment = StringAlignment.Center,
				LineAlignment = StringAlignment.Center,
				FormatFlags = StringFormatFlags.NoWrap
			};

			this.timer = new System.Windows.Forms.Timer { Interval = 100 };
			this.timer.Tick += this.Timer_Tick;

			this.Size = new Size(200, 200);
		}

		/// <summary>
		/// Начало таймера
		/// </summary>
		public void Start(TimeSpan duration)
		{
			this.timer.Stop();
			this.stopwatch.Reset();

			this.totalTime = duration > TimeSpan.Zero ? duration : TimeSpan.Zero;
			this.timeLeft = this.totalTime;

			if (this.totalTime > TimeSpan.Zero)
			{
				this.stopwatch.Start();
				this.timer.Start();
			}

			this.Invalidate();
		}

		/// <summary>
		/// Пауза
		/// </summary>
		public void Pause()
		{
			if (!this.timer.Enabled)
			{
				return;
			}

			this.timeLeft = this.GetTimeLeft();
			this.timer.Stop();
			this.stopwatch.Stop();
			this.Invalidate();
		}

		/// <summary>
		/// Продолжение таймера
		/// </summary>
		public void Resume()
		{
			if (this.timer.Enabled || this.timeLeft <= TimeSpan.Zero)
			{
				return;
			}

			this.stopwatch.Start();
			this.timer.Start();
		}

		/// <summary>
		/// Сброс таймера
		/// </summary>
		public void Stop()
		{
			this.timer.Stop();
			this.stopwatch.Reset();

			this.timeLeft = TimeSpan.Zero;
			this.totalTime = TimeSpan.Zero;

			this.Invalidate();
		}

		/// <summary>
		/// Освобождение ресурсов
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.timer.Stop();
				this.timer.Dispose();
				this.textFormat.Dispose();
			}

			base.Dispose(disposing);
		}

		/// <summary>
		/// Тик таймера
		/// </summary>
		private void Timer_Tick(object? sender, EventArgs e)
		{
			this.timeLeft = this.GetTimeLeft();

			if (this.timeLeft > TimeSpan.Zero)
			{
				this.Invalidate();
				return;
			}

			this.timeLeft = TimeSpan.Zero;
			this.timer.Stop();
			this.stopwatch.Reset();
			this.Invalidate();

			this.TimeFinished?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		/// Оставшееся время по точным часам
		/// </summary>
		private TimeSpan GetTimeLeft()
		{
			TimeSpan left = this.totalTime - this.stopwatch.Elapsed;

			return left > TimeSpan.Zero ? left : TimeSpan.Zero;
		}

		/// <summary>
		/// Рисование кругового таймера
		/// </summary>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			Graphics g = e.Graphics;
			int diameter = Math.Min(this.Width, this.Height);

			if (diameter <= this.thickness * 2 + 8)
			{
				return;
			}

			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			Rectangle rect = new Rectangle(
				(this.Width - diameter) / 2 + this.thickness / 2,
				(this.Height - diameter) / 2 + this.thickness / 2,
				diameter - this.thickness,
				diameter - this.thickness);

			// Фоновая дорожка
			using (Pen trackPen = new Pen(Theme.TimerTrack, this.thickness))
			{
				g.DrawEllipse(trackPen, rect);
			}

			// Прогресс
			if (this.totalTime > TimeSpan.Zero)
			{
				float progress = (float)(this.timeLeft.TotalMilliseconds / this.totalTime.TotalMilliseconds);

				if (progress > 0.001f)
				{
					using (Pen progressPen = new Pen(Theme.TimerProgress, this.thickness))
					{
						progressPen.StartCap = LineCap.Round;
						progressPen.EndCap = LineCap.Round;

						g.DrawArc(progressPen, rect, -90f, -progress * 360f);
					}
				}
			}

			// Текст
			string timeText = this.FormatTime(this.timeLeft);

			using (Font font = this.CreateTimeFont(g, timeText, diameter))
			using (Brush textBrush = new SolidBrush(Theme.TextColor))
			{
				g.DrawString(
					timeText,
					font,
					textBrush,
					new RectangleF(0f, 0f, this.Width, this.Height),
					this.textFormat);
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

			float maxWidth = diameter - this.thickness * 2 - diameter * 0.08f;
			float maxHeight = diameter - this.thickness * 2 - diameter * 0.12f;

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