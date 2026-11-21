// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 imimimi3

using System.IO;

namespace MiniPomidoroWin.Service
{
	/// <summary>
	/// Пользовательские настройки приложения
	/// </summary>
	internal static class SettingsService
	{
		/// <summary>
		/// Путь к выбранной пользователем мелодии окончания (null — стандартный звук)
		/// </summary>
		public static string? CustomSoundPath
		{
			get => Properties.Settings.Default.CustomSoundPath;
			set
			{
				Properties.Settings.Default.CustomSoundPath = value;
				Properties.Settings.Default.Save();
			}
		}

		/// <summary>
		/// Выбрана ли мелодия, файл которой существует на диске
		/// </summary>
		public static bool HasCustomSound
		{
			get
			{
				string? path = CustomSoundPath;

				return !string.IsNullOrWhiteSpace(path) && File.Exists(path);
			}
		}

		/// <summary>
		/// Сброс выбранной мелодии на стандартную
		/// </summary>
		public static void ResetCustomSound()
		{
			CustomSoundPath = null;
		}
	}
}