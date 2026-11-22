using MiniPomidoroWin.Controls;

namespace MiniPomidoroWin.Forms
{
	/// <summary>
	/// Подгонка окна под рабочую область экрана
	/// </summary>
	internal static class ScreenFit
	{
		/// <summary>
		/// Уменьшение окна, если оно не помещается на экране
		/// </summary>
		public static void FitToWorkingArea(Form form, int margin = 12)
		{
			if (form.IsDisposed || form.Disposing)
			{
				return;
			}

			Rectangle area = Screen.FromControl(form).WorkingArea;

			float widthFactor = (area.Width - margin * 2f) / form.Width;
			float heightFactor = (area.Height - margin * 2f) / form.Height;
			float factor = Math.Min(1f, Math.Min(widthFactor, heightFactor));

			// Помещается — ничего не меняем
			if (factor >= 0.999f)
			{
				return;
			}

			form.SuspendLayout();

			foreach (Control control in Enumerate(form))
			{
				ScaleFont(control, factor);
			}

			// Внимание: Control.Scale меняет только границы, шрифты не трогает,
			// поэтому шрифты масштабированы отдельно выше
			form.Scale(new SizeF(factor, factor));

			form.ResumeLayout(true);
		}

		/// <summary>
		/// Все вложенные контролы
		/// </summary>
		private static IEnumerable<Control> Enumerate(Control parent)
		{
			foreach (Control child in parent.Controls)
			{
				yield return child;

				foreach (Control nested in Enumerate(child))
				{
					yield return nested;
				}
			}
		}

		/// <summary>
		/// Пропорциональное уменьшение шрифта
		/// </summary>
		private static void ScaleFont(Control control, float factor)
		{
			Font font = control.Font;
			float size = Math.Max(AutoFitText.MinimumFontSize, font.SizeInPoints * factor);

			control.Font = new Font(font.FontFamily, size, font.Style, GraphicsUnit.Point);
		}
	}
}