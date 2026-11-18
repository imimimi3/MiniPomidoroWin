using MiniPomidoroWin.Forms;
using MiniPomidoroWin.Service;

namespace MiniPomidoroWin
{
	internal static class Program
	{
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			// В single-file сборке конфигурация извлекается из ресурсов
			// до первого обращения к настройкам приложения
			AppConfigBootstrap.Initialize();

			// To customize application configuration such as set high DPI settings or default font,
			// see https://aka.ms/applicationconfiguration.
			ApplicationConfiguration.Initialize();

			// Применение сохранённого языка интерфейса до создания форм
			LocalizationService.Instance.Initialize();

			Application.Run(new MainForm());
		}
	}
}