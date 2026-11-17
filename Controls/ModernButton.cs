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
		/// Переопределение базового класса рисования
		/// </summary>
		protected override void OnPaint(PaintEventArgs pevent)
		{
			base.OnPaint(pevent);

			Graphics graphics = pevent.Graphics;
			Rectangle surface = this.ClientRectangle;
			int borderSize = this._borderSize;
			int smoothSize = borderSize > 0 ? borderSize : 2;

			if (this._borderRadius <= 2)
			{
				if (borderSize >= 1)
				{
					graphics.SmoothingMode = SmoothingMode.None;

					using (Pen penBorder = new Pen(this._borderColor, borderSize))
					{
						penBorder.Alignment = PenAlignment.Inset;
						graphics.DrawRectangle(penBorder, 0, 0, surface.Width - 1, surface.Height - 1);
					}
				}

				return;
			}

			float radius = this.GetRadius();
			Rectangle border = Rectangle.Inflate(surface, -borderSize, -borderSize);

			graphics.SmoothingMode = SmoothingMode.AntiAlias;

			// Кайма цветом фона родителя сглаживает скругление по краям
			using (GraphicsPath pathSurface = RoundedRectangle.Create(surface, radius))
			using (Pen penSurface = new Pen(this.Parent?.BackColor ?? this.BackColor, smoothSize))
			{
				graphics.DrawPath(penSurface, pathSurface);
			}

			if (borderSize < 1)
			{
				return;
			}

			using (GraphicsPath pathBorder = RoundedRectangle.Create(border, Math.Max(0f, radius - borderSize)))
			using (Pen penBorder = new Pen(this._borderColor, borderSize))
			{
				graphics.DrawPath(penBorder, pathBorder);
			}
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