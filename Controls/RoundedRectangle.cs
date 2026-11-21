// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 imimimi3

using System.Drawing;
using System.Drawing.Drawing2D;

namespace MiniPomidoroWin.Controls
{
	/// <summary>
	/// Вспомогательные методы рисования
	/// </summary>
	internal static class RoundedRectangle
	{
		/// <summary>
		/// Создание пути скруглённого прямоугольника
		/// </summary>
		/// <param name="bounds">Границы</param>
		/// <param name="radius">Радиус скругления</param>
		public static GraphicsPath Create(RectangleF bounds, float radius)
		{
			GraphicsPath path = new GraphicsPath();
			float diameter = Math.Min(radius * 2f, Math.Min(bounds.Width, bounds.Height));

			if (diameter <= 0f)
			{
				path.AddRectangle(bounds);
				return path;
			}

			path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180f, 90f);
			path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270f, 90f);
			path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0f, 90f);
			path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90f, 90f);
			path.CloseFigure();

			return path;
		}
	}
}