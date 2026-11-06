using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniPomidoroWin.Service
{
	internal class SettingsService
	{
		/// <summary>
		/// Сохранение пути к мелодии в настройки
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
		/// Сброс выбранной музыки на стандартную
		/// </summary>
		public static void ResetCustomSound()
		{
			CustomSoundPath = null;
		}
	}
}
