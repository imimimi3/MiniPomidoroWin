// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 imimimi3

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniPomidoroWin.Constants
{
	internal class AppConstants
	{
		/// <summary>
		/// Отступ мини-окна от краёв экрана
		/// </summary>
		public const int MiniFormScreenMargin = 12;

		/// <summary>
		/// Интервал обновления подсказки значка в трее
		/// </summary>
		public const int TrayTooltipUpdateIntervalMs = 1000;

		/// <summary>
		/// Интервал напоминания, когда таймер не запущен: 10 минут
		/// </summary>
		public const int TrayReminderIntervalMs = 10 * 60 * 1000;
	}
}
