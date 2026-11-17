using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using MiniPomidoroWin.Themes;

namespace MiniPomidoroWin.Controls
{
	/// <summary>
	/// Значок кнопки
	/// </summary>
	public enum IconGlyph
	{
		/// <summary>
		/// Плей
		/// </summary>
		Play,

		/// <summary>
		/// Пауза
		/// </summary>
		Pause,

		/// <summary>
		/// Стоп
		/// </summary>
		Stop
	}

	/// <summary>
	/// Кнопка со значком, нарисованным средствами GDI+
	/// </summary>
	public class IconButton : ModernButton
	{
		/// <summary>
		/// Значок кнопки
		/// </summary>
		private IconGlyph glyph = IconGlyph.Play;

		/// <summary>
		/// Доля меньшей стороны, которую занимает значок
		/// </summary>
		private float iconScale = 0.36f;

		/// <summary>
		/// Значок кнопки
		/// </summary>
		[DefaultValue(IconGlyph.Play)]
		[Category("Appearance")]
		public IconGlyph Glyph
		{
			get => glyph;
			set
			{
				if (glyph == value)
				{
					return;
				}

				glyph = value;
				Invalidate();
			}
		}

		/// <summary>
		/// Доля меньшей стороны, которую занимает значок
		/// </summary>
		[DefaultValue(0.36f)]
		[Category("Appearance")]
		public float IconScale
		{
			get => iconScale;
			set
			{
				float clamped = Math.Clamp(value, 0.15f, 0.8f);

				if (Math.Abs(iconScale - clamped) < 0.001f)
				{
					return;
				}

				iconScale = clamped;
				Invalidate();
			}
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		public IconButton()
		{
			this.Text = string.Empty;
			this.UseMnemonic = false;
		}

		/// <summary>
		/// Рисование кнопки и значка
		/// </summary>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;

			float size = Math.Min(this.Width, this.Height) * iconScale;
			RectangleF area = new RectangleF(
				(this.Width - size) / 2f,
				(this.Height - size) / 2f,
				size,
				size);

			Color color = this.Enabled ? this.ForeColor : Theme.FieldCaption;

			using (Brush brush = new SolidBrush(color))
			{
				switch (glyph)
				{
					case IconGlyph.Play:
						DrawPlay(g, brush, area);
						break;

					case IconGlyph.Pause:
						DrawPause(g, brush, area);
						break;

					case IconGlyph.Stop:
						DrawStop(g, brush, area);
						break;
				}
			}
		}

		/// <summary>
		/// Рисование треугольника "Плей"
		/// </summary>
		private static void DrawPlay(Graphics g, Brush brush, RectangleF area)
		{
			// Небольшой сдвиг вправо компенсирует оптический центр треугольника
			float offset = area.Width * 0.06f;

			PointF[] points =
			{
				new PointF(area.Left + offset, area.Top),
				new PointF(area.Right + offset, area.Top + area.Height / 2f),
				new PointF(area.Left + offset, area.Bottom)
			};

			g.FillPolygon(brush, points);
		}

		/// <summary>
		/// Рисование двух полос "Пауза"
		/// </summary>
		private static void DrawPause(Graphics g, Brush brush, RectangleF area)
		{
			float barWidth = area.Width * 0.3f;
			float gap = area.Width * 0.22f;
			float totalWidth = barWidth * 2f + gap;
			float left = area.Left + (area.Width - totalWidth) / 2f;
			float radius = barWidth / 2f;

			using (GraphicsPath first = RoundedRectangle.Create(
				new RectangleF(left, area.Top, barWidth, area.Height), radius))
			using (GraphicsPath second = RoundedRectangle.Create(
				new RectangleF(left + barWidth + gap, area.Top, barWidth, area.Height), radius))
			{
				g.FillPath(brush, first);
				g.FillPath(brush, second);
			}
		}

		/// <summary>
		/// Рисование квадрата "Стоп"
		/// </summary>
		private static void DrawStop(Graphics g, Brush brush, RectangleF area)
		{
			float side = area.Width * 0.82f;
			float offset = (area.Width - side) / 2f;
			float radius = side * 0.22f;

			using (GraphicsPath path = RoundedRectangle.Create(
				new RectangleF(area.Left + offset, area.Top + offset, side, side), radius))
			{
				g.FillPath(brush, path);
			}
		}
	}
}