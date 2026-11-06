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
		private int _borderSize = 0;

		/// <summary>
		/// Радиус скругления
		/// </summary>
		private int _borderRadius = 30;
		private Color _borderColor = Color.White;
		
		/// <summary>
		/// Толщина рамки
		/// </summary>
		[DefaultValue(0)]
		public int BorderSize
		{
			get => _borderSize;
			set { _borderSize = value; UpdateRegion(); Invalidate(); }
		}

		/// <summary>
		/// Радиус скругления
		/// </summary>
		[DefaultValue(30)]
		public int BorderRadius
		{
			get => _borderRadius;
			set { _borderRadius = value; UpdateRegion(); Invalidate(); }
		}

		/// <summary>
		/// Цвет рамки
		/// </summary>
		[DefaultValue(typeof(Color), "White")]
		public Color BorderColor
		{
			get => _borderColor;
			set { _borderColor = value; Invalidate(); }
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
			this.ForeColor = Color.White;

			// Цвета при нажатии на кнопку
			this.FlatAppearance.MouseOverBackColor = Theme.ButtonHover;
			this.FlatAppearance.MouseDownBackColor = Theme.ButtonPressed;
		}

		/// <summary>
		/// Обновление при изменении размера
		/// </summary> 
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			UpdateRegion();
		}

		/// <summary>
		///
		/// </summary> 
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			if (this.Parent != null)
			{
				this.Parent.BackColorChanged += (s, ev) => this.Invalidate();
			}
		}

		/// <summary>
		/// Перерисовка
		/// </summary> 
		private void UpdateRegion()
		{
			if (_borderRadius > 2 && this.Width > 0 && this.Height > 0)
			{
				int safeRadius = Math.Min(_borderRadius, Math.Min(this.Width, this.Height));
				using (GraphicsPath path = GetFigurePath(this.ClientRectangle, safeRadius))
				{
					this.Region = new Region(path);
				}
			}
			else
			{
				this.Region = new Region(this.ClientRectangle);
			}
		}

		/// <summary>
		/// Переопределение базового класса рисования
		/// </summary> 
		protected override void OnPaint(PaintEventArgs pevent)
		{
			base.OnPaint(pevent);

			Rectangle rectSurface = this.ClientRectangle;
			Rectangle rectBorder = Rectangle.Inflate(rectSurface, -_borderSize, -_borderSize);
			int smoothSize = 2;
			if (_borderSize > 0) smoothSize = _borderSize;

			if (_borderRadius > 2)
			{
				int safeRadius = Math.Min(_borderRadius, Math.Min(this.Width, this.Height));

				using (GraphicsPath pathSurface = GetFigurePath(rectSurface, safeRadius))
				using (GraphicsPath pathBorder = GetFigurePath(rectBorder, safeRadius - _borderSize))
				
				using (Pen penSurface = new Pen(this.Parent?.BackColor ?? this.BackColor, smoothSize))
				using (Pen penBorder = new Pen(_borderColor, _borderSize))
				{
					pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

					pevent.Graphics.DrawPath(penSurface, pathSurface);

					if (_borderSize >= 1)
					{
						pevent.Graphics.DrawPath(penBorder, pathBorder);
					}
				}
			}
			else
			{
				pevent.Graphics.SmoothingMode = SmoothingMode.None;
				if (_borderSize >= 1)
				{
					using (Pen penBorder = new Pen(_borderColor, _borderSize))
					{
						penBorder.Alignment = PenAlignment.Inset;
						pevent.Graphics.DrawRectangle(penBorder, 0, 0, this.Width - 1, this.Height - 1);
					}
				}
			}
		}

		/// <summary>
		/// Вспомогательный метод для создания закругленного прямоугольника
		/// </summary>  
		private GraphicsPath GetFigurePath(Rectangle rect, int radius)
		{
			GraphicsPath path = new GraphicsPath();
			float curveSize = radius * 2F;

			path.StartFigure();
			path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
			path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
			path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
			path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
			path.CloseFigure();
			return path;
		}
	}
}