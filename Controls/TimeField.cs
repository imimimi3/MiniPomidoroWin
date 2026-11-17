using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using MiniPomidoroWin.Themes;

namespace MiniPomidoroWin.Controls
{
	/// <summary>
	/// Поле выбора числового значения со стрелками увеличения и уменьшения
	/// </summary>
	public class TimeField : Control
	{
		/// <summary>
		/// Радиус скругления поля
		/// </summary>
		private const float CornerRadius = 14f;

		/// <summary>
		/// Доля ширины поля, отведённая под стрелки
		/// </summary>
		private const float ArrowsWidthRatio = 0.26f;

		/// <summary>
		/// Отношение высоты шрифта значения к высоте поля
		/// </summary>
		private const float ValueFontRatio = 0.34f;

		/// <summary>
		/// Отношение высоты шрифта подписи к высоте поля
		/// </summary>
		private const float CaptionFontRatio = 0.13f;

		/// <summary>
		/// Шаг изменения значения стрелками и клавишами
		/// </summary>
		private const int SmallStep = 1;

		/// <summary>
		/// Шаг изменения значения клавишами Page Up / Page Down
		/// </summary>
		private const int LargeStep = 10;

		/// <summary>
		/// Форматирование текста по центру без переноса
		/// </summary>
		private static readonly StringFormat CenteredFormat = new StringFormat
		{
			Alignment = StringAlignment.Center,
			LineAlignment = StringAlignment.Center,
			FormatFlags = StringFormatFlags.NoWrap
		};

		/// <summary>
		/// Минимальное значение
		/// </summary>
		private int minimum;

		/// <summary>
		/// Максимальное значение
		/// </summary>
		private int maximum = 99;

		/// <summary>
		/// Текущее значение
		/// </summary>
		private int currentValue;

		/// <summary>
		/// Подпись поля
		/// </summary>
		private string caption = string.Empty;

		/// <summary>
		/// Зона стрелок под указателем: 0 — увеличение, 1 — уменьшение, -1 — нет
		/// </summary>
		private int hoveredZone = -1;

		/// <summary>
		/// Нажатая зона стрелок: 0 — увеличение, 1 — уменьшение, -1 — нет
		/// </summary>
		private int pressedZone = -1;

		/// <summary>
		/// Минимальное значение
		/// </summary>
		[DefaultValue(0)]
		[Category("Behavior")]
		public int Minimum
		{
			get => this.minimum;
			set
			{
				if (this.minimum == value)
				{
					return;
				}

				this.minimum = value;

				if (this.maximum < this.minimum)
				{
					this.maximum = this.minimum;
				}

				this.Value = this.currentValue;
			}
		}

		/// <summary>
		/// Максимальное значение
		/// </summary>
		[DefaultValue(99)]
		[Category("Behavior")]
		public int Maximum
		{
			get => this.maximum;
			set
			{
				if (this.maximum == value)
				{
					return;
				}

				this.maximum = value;

				if (this.minimum > this.maximum)
				{
					this.minimum = this.maximum;
				}

				this.Value = this.currentValue;
			}
		}

		/// <summary>
		/// Текущее значение
		/// </summary>
		[DefaultValue(0)]
		[Category("Behavior")]
		public int Value
		{
			get => this.currentValue;
			set
			{
				int clamped = Math.Clamp(value, this.minimum, this.maximum);

				if (this.currentValue == clamped)
				{
					return;
				}

				this.currentValue = clamped;
				this.Invalidate();
				this.ValueChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Подпись поля
		/// </summary>
		[DefaultValue("")]
		[Category("Appearance")]
		public string Caption
		{
			get => this.caption;
			set
			{
				string newValue = value ?? string.Empty;

				if (this.caption == newValue)
				{
					return;
				}

				this.caption = newValue;
				this.Invalidate();
			}
		}

		/// <summary>
		/// Событие изменения значения
		/// </summary>
		public event EventHandler? ValueChanged;

		/// <summary>
		/// Конструктор
		/// </summary>
		public TimeField()
		{
			this.SetStyle(
				ControlStyles.AllPaintingInWmPaint |
				ControlStyles.OptimizedDoubleBuffer |
				ControlStyles.UserPaint |
				ControlStyles.ResizeRedraw |
				ControlStyles.Selectable |
				ControlStyles.SupportsTransparentBackColor,
				true);

			this.BackColor = Color.Transparent;
			this.TabStop = true;

			this.Size = new Size(160, 110);
		}

		/// <summary>
		/// Изменение значения на заданную величину
		/// </summary>
		private void Step(int delta)
		{
			this.Value = this.currentValue + delta;
		}

		/// <summary>
		/// Границы поля
		/// </summary>
		private RectangleF GetFieldBounds()
		{
			return new RectangleF(1f, 1f, Math.Max(2f, this.Width - 2f), Math.Max(2f, this.Height - 2f));
		}

		/// <summary>
		/// X-координата начала зоны стрелок
		/// </summary>
		private float GetArrowsLeft()
		{
			return this.Width * (1f - ArrowsWidthRatio);
		}

		/// <summary>
		/// Зона стрелок под точкой: 0 — увеличение, 1 — уменьшение, -1 — нет
		/// </summary>
		private int GetZoneAt(Point location)
		{
			if (location.X < this.GetArrowsLeft())
			{
				return -1;
			}

			return location.Y < this.Height / 2 ? 0 : 1;
		}

		/// <summary>
		/// Прокрутка колесом мыши
		/// </summary>
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			this.Step(e.Delta > 0 ? SmallStep : -SmallStep);
			base.OnMouseWheel(e);
		}

		/// <summary>
		/// Перемещение указателя
		/// </summary>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			int zone = this.GetZoneAt(e.Location);

			if (this.hoveredZone == zone)
			{
				return;
			}

			this.hoveredZone = zone;
			this.Invalidate();
		}

		/// <summary>
		/// Уход указателя с поля
		/// </summary>
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);

			if (this.hoveredZone == -1)
			{
				return;
			}

			this.hoveredZone = -1;
			this.Invalidate();
		}

		/// <summary>
		/// Нажатие кнопки мыши
		/// </summary>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			this.Focus();

			if (e.Button != MouseButtons.Left)
			{
				return;
			}

			int zone = this.GetZoneAt(e.Location);

			if (zone < 0)
			{
				return;
			}

			this.pressedZone = zone;
			this.Invalidate();
			this.Step(zone == 0 ? SmallStep : -SmallStep);
		}

		/// <summary>
		/// Отпускание кнопки мыши
		/// </summary>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			if (this.pressedZone == -1)
			{
				return;
			}

			this.pressedZone = -1;
			this.Invalidate();
		}

		/// <summary>
		/// Клавиши стрелок должны попадать в поле, а не менять фокус
		/// </summary>
		protected override bool IsInputKey(Keys keyData)
		{
			switch (keyData)
			{
				case Keys.Up:
				case Keys.Down:
				case Keys.PageUp:
				case Keys.PageDown:
				case Keys.Home:
				case Keys.End:
					return true;
			}

			return base.IsInputKey(keyData);
		}

		/// <summary>
		/// Обработка клавиатуры
		/// </summary>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			switch (e.KeyCode)
			{
				case Keys.Up:
					this.Step(SmallStep);
					break;

				case Keys.Down:
					this.Step(-SmallStep);
					break;

				case Keys.PageUp:
					this.Step(LargeStep);
					break;

				case Keys.PageDown:
					this.Step(-LargeStep);
					break;

				case Keys.Home:
					this.Value = this.minimum;
					break;

				case Keys.End:
					this.Value = this.maximum;
					break;

				default:
					return;
			}

			e.Handled = true;
		}

		/// <summary>
		/// Получение фокуса
		/// </summary>
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			this.Invalidate();
		}

		/// <summary>
		/// Потеря фокуса
		/// </summary>
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			this.Invalidate();
		}

		/// <summary>
		/// Рисование поля
		/// </summary>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

			RectangleF bounds = this.GetFieldBounds();
			float radius = Math.Min(CornerRadius, bounds.Height / 2f);

			using (GraphicsPath path = RoundedRectangle.Create(bounds, radius))
			using (SolidBrush background = new SolidBrush(Theme.FieldBackground))
			using (Pen border = new Pen(this.Focused ? Theme.ButtonPrimary : Theme.FieldBorder, this.Focused ? 2f : 1f))
			{
				g.FillPath(background, path);
				g.DrawPath(border, path);
			}

			float arrowsLeft = this.GetArrowsLeft();

			// Подпись
			using (Font captionFont = this.CreateCaptionFont())
			using (Brush captionBrush = new SolidBrush(Theme.FieldCaption))
			{
				RectangleF captionBounds = new RectangleF(
					bounds.Left,
					bounds.Top + bounds.Height * 0.11f,
					bounds.Width,
					bounds.Height * 0.18f);

				g.DrawString(this.caption, captionFont, captionBrush, captionBounds, CenteredFormat);
			}

			// Значение
			string text = this.currentValue.ToString();
			RectangleF valueBounds = new RectangleF(
				bounds.Left,
				bounds.Top + bounds.Height * 0.26f,
				Math.Max(1f, arrowsLeft - bounds.Left),
				bounds.Height * 0.66f);

			using (Font valueFont = this.CreateValueFont(g, text, valueBounds))
			using (Brush valueBrush = new SolidBrush(this.Enabled ? Theme.TextColor : Theme.FieldCaption))
			{
				g.DrawString(text, valueFont, valueBrush, valueBounds, CenteredFormat);
			}

			// Стрелки
			RectangleF arrows = new RectangleF(arrowsLeft, bounds.Top, bounds.Right - arrowsLeft, bounds.Height);
			this.DrawArrows(g, arrows);
		}

		/// <summary>
		/// Рисование разделителей и стрелок
		/// </summary>
		private void DrawArrows(Graphics g, RectangleF zone)
		{
			float lineWidth = Math.Max(1f, this.Height * 0.012f);
			float penWidth = Math.Max(1.5f, this.Height * 0.02f);

			using (Pen linePen = new Pen(Theme.FieldBorder, lineWidth))
			{
				g.DrawLine(
					linePen,
					zone.Left,
					zone.Top + zone.Height * 0.22f,
					zone.Left,
					zone.Bottom - zone.Height * 0.22f);

				g.DrawLine(
					linePen,
					zone.Left + zone.Width * 0.18f,
					zone.Top + zone.Height / 2f,
					zone.Right - zone.Width * 0.18f,
					zone.Top + zone.Height / 2f);
			}

			RectangleF topHalf = new RectangleF(zone.Left, zone.Top, zone.Width, zone.Height / 2f);
			RectangleF bottomHalf = new RectangleF(zone.Left, zone.Top + zone.Height / 2f, zone.Width, zone.Height / 2f);

			this.DrawChevron(g, topHalf, true, this.hoveredZone == 0 || this.pressedZone == 0, penWidth);
			this.DrawChevron(g, bottomHalf, false, this.hoveredZone == 1 || this.pressedZone == 1, penWidth);
		}

		/// <summary>
		/// Рисование одной стрелки
		/// </summary>
		private void DrawChevron(Graphics g, RectangleF half, bool up, bool highlighted, float penWidth)
		{
			if (highlighted)
			{
				float radius = Math.Min(8f, half.Height / 3f);
				RectangleF highlight = new RectangleF(
					half.Left + half.Width * 0.12f,
					half.Top + half.Height * 0.14f,
					half.Width * 0.76f,
					half.Height * 0.72f);

				using (GraphicsPath path = RoundedRectangle.Create(highlight, radius))
				using (SolidBrush brush = new SolidBrush(Theme.FieldBorder))
				{
					g.FillPath(brush, path);
				}
			}

			float centerX = half.Left + half.Width / 2f;
			float centerY = half.Top + half.Height / 2f;
			float halfWidth = Math.Min(half.Width * 0.30f, half.Height * 0.30f);
			float halfHeight = halfWidth * 0.5f;

			PointF left = new PointF(centerX - halfWidth, up ? centerY + halfHeight : centerY - halfHeight);
			PointF middle = new PointF(centerX, up ? centerY - halfHeight : centerY + halfHeight);
			PointF right = new PointF(centerX + halfWidth, left.Y);

			using (Pen pen = new Pen(highlighted ? Theme.TextColor : Theme.FieldCaption, penWidth))
			{
				pen.StartCap = LineCap.Round;
				pen.EndCap = LineCap.Round;
				pen.LineJoin = LineJoin.Round;

				g.DrawLines(pen, new[] { left, middle, right });
			}
		}

		/// <summary>
		/// Шрифт подписи
		/// </summary>
		private Font CreateCaptionFont()
		{
			float size = Math.Max(6f, this.Height * CaptionFontRatio);

			return new Font("Segoe UI", size, FontStyle.Regular, GraphicsUnit.Pixel);
		}

		/// <summary>
		/// Шрифт значения, подогнанный под размер поля
		/// </summary>
		private Font CreateValueFont(Graphics g, string text, RectangleF bounds)
		{
			float size = Math.Max(8f, this.Height * ValueFontRatio);
			Font font = new Font("Segoe UI", size, FontStyle.Bold, GraphicsUnit.Pixel);

			SizeF measured = g.MeasureString(text, font);
			float factor = 1f;

			if (measured.Width > 0f && bounds.Width > 0f)
			{
				factor = Math.Min(factor, bounds.Width * 0.9f / measured.Width);
			}

			if (measured.Height > 0f && bounds.Height > 0f)
			{
				factor = Math.Min(factor, bounds.Height * 0.9f / measured.Height);
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