using System.Runtime.CompilerServices;

namespace MiniPomidoroWin.Controls
{
	/// <summary>
	/// Подгонка шрифта текста под размер элемента
	/// </summary>
	internal static class AutoFitText
	{
		/// <summary>
		/// Минимальный размер шрифта: меньше не уменьшаем, иначе текст нечитаем
		/// </summary>
		public const float MinimumFontSize = 6.5f;

		/// <summary>
		/// Шаг уменьшения шрифта, пункты
		/// </summary>
		private const float SizeStep = 0.5f;

		/// <summary>
		/// Кэш созданных шрифтов: размеры дискретны, поэтому шрифтов немного
		/// </summary>
		private static readonly Dictionary<(string Family, FontStyle Style, float Size), Font> FontCache =
			new Dictionary<(string Family, FontStyle Style, float Size), Font>();

		/// <summary>
		/// Исходные (дизайнерские) шрифты подписей
		/// </summary>
		private static readonly ConditionalWeakTable<Control, Font> BaseFonts =
			new ConditionalWeakTable<Control, Font>();

		/// <summary>
		/// Подгонка шрифта всех подписей контейнера
		/// </summary>
		public static void Apply(Control root)
		{
			foreach (Control child in root.Controls)
			{
				if (child is Label label)
				{
					FitLabel(label);
				}

				Apply(child);
			}
		}

		/// <summary>
		/// Забыть исходные шрифты: нужно после изменения масштаба формы
		/// </summary>
		public static void Reset(Control root)
		{
			BaseFonts.Remove(root);

			foreach (Control child in root.Controls)
			{
				Reset(child);
			}
		}

		/// <summary>
		/// Шрифт, вписывающий текст в одну строку
		/// </summary>
		public static Font FitSingleLine(
			Font baseFont,
			string? text,
			Size bounds,
			TextFormatFlags flags,
			float minimumSize = MinimumFontSize)
		{
			float size = baseFont.SizeInPoints;

			if (string.IsNullOrEmpty(text) || bounds.Width <= 0 || bounds.Height <= 0)
			{
				return new Font(baseFont.FontFamily, size, baseFont.Style, GraphicsUnit.Point);
			}

			for (; ; )
			{
				Font font = new Font(baseFont.FontFamily, size, baseFont.Style, GraphicsUnit.Point);

				Size measured = TextRenderer.MeasureText(
					text,
					font,
					new Size(int.MaxValue, int.MaxValue),
					flags);

				if ((measured.Width <= bounds.Width && measured.Height <= bounds.Height) ||
					size <= minimumSize)
				{
					return font;
				}

				font.Dispose();

				size = Math.Max(minimumSize, size - SizeStep);
			}
		}

		/// <summary>
		/// Подгонка одной подписи
		/// </summary>
		private static void FitLabel(Label label)
		{
			// У AutoSize-подписи размер определяется текстом, уменьшать нечего:
			// переводим её на фиксированные границы из дизайнера
			if (label.AutoSize)
			{
				label.AutoSize = false;
			}

			Font baseFont = BaseFonts.GetValue(label, static control => control.Font);

			Rectangle area = label.ClientRectangle;
			area = Rectangle.Inflate(area, -label.Padding.Horizontal, -label.Padding.Vertical);

			if (area.Width <= 0 || area.Height <= 0)
			{
				return;
			}

			TextFormatFlags flags = TextFormatFlags.WordBreak | TextFormatFlags.NoPadding;
			float size = baseFont.SizeInPoints;

			for (; ; )
			{
				Size measured = TextRenderer.MeasureText(
					label.Text,
					GetFont(baseFont, size),
					new Size(area.Width, int.MaxValue),
					flags);

				bool fits = measured.Width <= area.Width && measured.Height <= area.Height;

				if (fits || size <= MinimumFontSize)
				{
					label.Font = GetFont(baseFont, size);
					return;
				}

				size = Math.Max(MinimumFontSize, size - SizeStep);
			}
		}

		/// <summary>
		/// Шрифт из кэша
		/// </summary>
		private static Font GetFont(Font baseFont, float size)
		{
			var key = (baseFont.FontFamily.Name, baseFont.Style, size);

			if (!FontCache.TryGetValue(key, out Font? font))
			{
				font = new Font(baseFont.FontFamily, size, baseFont.Style, GraphicsUnit.Point);
				FontCache[key] = font;
			}

			return font;
		}
	}
}