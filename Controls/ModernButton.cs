using System.ComponentModel;
using System.Drawing.Drawing2D;
using MiniPomidoroWin.Themes;

namespace MiniPomidoroWin.Controls
{
	/// <summary>
	/// Красивая скруглённая кнопка
	/// </summary>
	public class ModernButton : Button
	{
		/// <summary>
		/// Толщина рамки
		/// </summary>
		private int _borderSize;

		/// <summary>
		/// Радиус скругления
		/// </summary>
		private int _borderRadius = 30;

		/// <summary>
		/// Цвет рамки
		/// </summary>
		private Color _borderColor = Color.White;

		/// <summary>
		/// Родитель, к изменению цвета которого подписана кнопка
		/// </summary>
		private Control? _colorSource;

		/// <summary>
		/// Курсор находится над кнопкой
		/// </summary>
		private bool _hovered;

		/// <summary>
		/// Кнопка нажата
		/// </summary>
		private bool _pressed;

		/// <summary>
		/// Минимальный размер шрифта при подгонке текста
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		/// <summary>
		/// Минимальный размер шрифта при подгонке текста
		/// </summary>
		public float MinimumFontSize { get; set; } = AutoFitText.MinimumFontSize;

		/// <summary>
		/// Толщина рамки
		/// </summary>
		[DefaultValue(0)]
		public int BorderSize
		{
			get => this._borderSize;
			set
			{
				if (this._borderSize == value)
				{
					return;
				}

				this._borderSize = value;
				this.Invalidate();
			}
		}

		/// <summary>
		/// Радиус скругления
		/// </summary>
		[DefaultValue(30)]
		public int BorderRadius
		{
			get => this._borderRadius;
			set
			{
				if (this._borderRadius == value)
				{
					return;
				}

				this._borderRadius = value;
				this.UpdateRegion();
				this.Invalidate();
			}
		}

		/// <summary>
		/// Цвет рамки
		/// </summary>
		[DefaultValue(typeof(Color), "White")]
		public Color BorderColor
		{
			get => this._borderColor;
			set
			{
				if (this._borderColor == value)
				{
					return;
				}

				this._borderColor = value;
				this.Invalidate();
			}
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		public ModernButton()
		{
			// Кнопка рисуется целиком вручную: так текст и значок
			// всегда оказываются ровно по центру клиентской области
			this.SetStyle(
				ControlStyles.UserPaint |
				ControlStyles.AllPaintingInWmPaint |
				ControlStyles.OptimizedDoubleBuffer |
				ControlStyles.ResizeRedraw,
				true);

			this.UpdateStyles();

			// Базовые настройки кнопки
			this.FlatStyle = FlatStyle.Flat;
			this.FlatAppearance.BorderSize = 0;
			this.Size = new Size(150, 40);
			this.Cursor = Cursors.Hand;
			this.TextAlign = ContentAlignment.MiddleCenter;

			// Настройка цветов
			this.BackColor = Theme.ButtonPrimary;
			this.ForeColor = Theme.TextColor;

			// Цвета при наведении и нажатии
			this.FlatAppearance.MouseOverBackColor = Theme.ButtonHover;
			this.FlatAppearance.MouseDownBackColor = Theme.ButtonPressed;
		}

		/// <summary>
		/// Обновление скругления при изменении размера
		/// </summary>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			this.UpdateRegion();
		}

		/// <summary>
		/// Подписка на изменение цвета родителя
		/// </summary>
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			this.BindParentColors();
		}

		/// <summary>
		/// Переподписка при смене родителя
		/// </summary>
		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);

			this.BindParentColors();
		}

		/// <summary>
		/// Курсор вошёл на кнопку
		/// </summary>
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);

			this._hovered = true;
			this.Invalidate();
		}

		/// <summary>
		/// Курсор покинул кнопку
		/// </summary>
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);

			this._hovered = false;
			this._pressed = false;
			this.Invalidate();
		}

		/// <summary>
		/// Нажатие кнопки мышью
		/// </summary>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (e.Button == MouseButtons.Left)
			{
				this._pressed = true;
				this.Invalidate();
			}
		}

		/// <summary>
		/// Отпускание кнопки мыши
		/// </summary>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			if (this._pressed)
			{
				this._pressed = false;
				this.Invalidate();
			}
		}

		/// <summary>
		/// Изменение доступности кнопки
		/// </summary>
		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);

			this.Invalidate();
		}

		/// <summary>
		/// Отписка от событий и освобождение ресурсов
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.UnbindParentColors();
			}

			base.Dispose(disposing);
		}

		/// <summary>
		/// Отрисовка фона, рамки и текста
		/// </summary>
		protected override void OnPaint(PaintEventArgs pevent)
		{
			Graphics graphics = pevent.Graphics;
			Rectangle surface = this.ClientRectangle;

			if (surface.Width <= 0 || surface.Height <= 0)
			{
				return;
			}

			// Фон родителя нужен для сглаживания скруглённых краёв
			using (Brush parentBrush = new SolidBrush(this.Parent?.BackColor ?? this.BackColor))
			{
				graphics.FillRectangle(parentBrush, surface);
			}

			this.PaintSurface(graphics, surface);
			this.PaintBorder(graphics, surface);
			this.PaintText(graphics, surface);
		}

		/// <summary>
		/// Заливка кнопки с учётом состояния
		/// </summary>
		private void PaintSurface(Graphics graphics, Rectangle surface)
		{
			using (Brush brush = new SolidBrush(this.GetFillColor()))
			{
				if (this._borderRadius <= 2)
				{
					graphics.FillRectangle(brush, surface);

					return;
				}

				graphics.SmoothingMode = SmoothingMode.AntiAlias;

				using (GraphicsPath path = RoundedRectangle.Create(surface, this.GetRadius()))
				{
					graphics.FillPath(brush, path);
				}
			}
		}

		/// <summary>
		/// Рамка кнопки
		/// </summary>
		private void PaintBorder(Graphics graphics, Rectangle surface)
		{
			if (this._borderSize < 1)
			{
				return;
			}

			if (this._borderRadius <= 2)
			{
				graphics.SmoothingMode = SmoothingMode.None;

				using (Pen penBorder = new Pen(this._borderColor, this._borderSize))
				{
					penBorder.Alignment = PenAlignment.Inset;
					graphics.DrawRectangle(penBorder, 0, 0, surface.Width - 1, surface.Height - 1);
				}

				return;
			}

			float radius = this.GetRadius();
			Rectangle border = Rectangle.Inflate(surface, -this._borderSize, -this._borderSize);

			graphics.SmoothingMode = SmoothingMode.AntiAlias;

			using (GraphicsPath pathBorder = RoundedRectangle.Create(border, Math.Max(0f, radius - this._borderSize)))
			using (Pen penBorder = new Pen(this._borderColor, this._borderSize))
			{
				graphics.DrawPath(penBorder, pathBorder);
			}
		}

		/// <summary>
		/// Текст по центру клиентской области
		/// </summary>
		private void PaintText(Graphics graphics, Rectangle surface)
		{
			if (string.IsNullOrEmpty(this.Text))
			{
				return;
			}

			TextFormatFlags flags =
				TextFormatFlags.HorizontalCenter |
				TextFormatFlags.VerticalCenter |
				TextFormatFlags.SingleLine |
				TextFormatFlags.NoPadding;

			if (!this.UseMnemonic)
			{
				flags |= TextFormatFlags.NoPrefix;
			}

			Color color = this.Enabled ? this.ForeColor : Theme.FieldCaption;

			Rectangle textBounds = Rectangle.Inflate(surface, -this._borderSize - 6, -this._borderSize - 4);

			using (Font font = AutoFitText.FitSingleLine(this.Font, this.Text, textBounds.Size, flags, this.MinimumFontSize))
			{
				TextRenderer.DrawText(graphics, this.Text, font, textBounds, color, flags);
			}
		}

		/// <summary>
		/// Цвет заливки с учётом наведения и нажатия
		/// </summary>
		private Color GetFillColor()
		{
			if (!this.Enabled)
			{
				return this.BackColor;
			}

			if (this._pressed)
			{
				Color pressed = this.FlatAppearance.MouseDownBackColor;

				return pressed.IsEmpty ? Blend(this.BackColor, Color.Black, 0.15f) : pressed;
			}

			if (this._hovered)
			{
				Color hover = this.FlatAppearance.MouseOverBackColor;

				return hover.IsEmpty ? Blend(this.BackColor, Color.White, 0.12f) : hover;
			}

			return this.BackColor;
		}

		/// <summary>
		/// Смешивание двух цветов
		/// </summary>
		private static Color Blend(Color source, Color target, float amount)
		{
			float rest = 1f - amount;

			return Color.FromArgb(
				source.A,
				(int)((source.R * rest) + (target.R * amount)),
				(int)((source.G * rest) + (target.G * amount)),
				(int)((source.B * rest) + (target.B * amount)));
		}

		/// <summary>
		/// Радиус скругления с учётом размеров кнопки
		/// </summary>
		private float GetRadius()
		{
			return Math.Min(this._borderRadius, Math.Min(this.Width, this.Height) / 2f);
		}

		/// <summary>
		/// Обновление скруглённой области кнопки
		/// </summary>
		private void UpdateRegion()
		{
			Region? previous = this.Region;
			Region next;

			if (this._borderRadius > 2 && this.Width > 0 && this.Height > 0)
			{
				using (GraphicsPath path = RoundedRectangle.Create(this.ClientRectangle, this.GetRadius()))
				{
					next = new Region(path);
				}
			}
			else
			{
				next = new Region(this.ClientRectangle);
			}

			this.Region = next;

			// Без этого каждый ресайз оставлял бы регион в GDI неосвобождённым
			previous?.Dispose();
		}

		/// <summary>
		/// Подписка на изменение цвета родителя
		/// </summary>
		private void BindParentColors()
		{
			Control? parent = this.Parent;

			if (ReferenceEquals(this._colorSource, parent))
			{
				return;
			}

			this.UnbindParentColors();

			this._colorSource = parent;

			if (this._colorSource != null)
			{
				this._colorSource.BackColorChanged += this.Parent_BackColorChanged;
			}
		}

		/// <summary>
		/// Отписка от изменения цвета родителя
		/// </summary>
		private void UnbindParentColors()
		{
			if (this._colorSource != null)
			{
				this._colorSource.BackColorChanged -= this.Parent_BackColorChanged;
				this._colorSource = null;
			}
		}

		/// <summary>
		/// Перерисовка при изменении цвета родителя
		/// </summary>
		private void Parent_BackColorChanged(object? sender, EventArgs e)
		{
			this.Invalidate();
		}
	}
}
