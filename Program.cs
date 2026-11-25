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
		static void Main(string[] args)
		{
			// Второй экземпляр не запускается: он только показывает окно первого
			using (SingleInstance singleInstance = new SingleInstance())
			{
				if (!singleInstance.IsFirstInstance)
				{
					singleInstance.ActivateFirstInstance();

					return;
				}

				// В single-file сборке конфигурация извлекается из ресурсов
				// до первого обращения к настройкам приложения
				AppConfigBootstrap.Initialize();

				// To customize application configuration such as set high DPI settings or default font,
				// see https://aka.ms/applicationconfiguration.
				ApplicationConfiguration.Initialize();

				// Применение сохранённого языка интерфейса до создания форм
				LocalizationService.Instance.Initialize();

				// Путь в автозагрузке мог устареть, если exe перенесли
				AutoStartService.UpdatePathIfNeeded();

				// Автозагрузка запускает приложение сразу в трей
				bool startHidden = args.Any(arg =>
					string.Equals(arg, "--tray", StringComparison.OrdinalIgnoreCase));

				MainForm mainForm = new MainForm(startHidden);

				singleInstance.ActivationRequested += (sender, e) => ActivateMainForm(mainForm);

				if (mainForm.IsHandleCreated)
				{
					singleInstance.StartListening();
				}
				else
				{
					mainForm.HandleCreated += (sender, e) => singleInstance.StartListening();
				}

				Application.Run(mainForm);
			}
		}

		/// <summary>
		/// Показ окна при повторном запуске приложения
		/// </summary>
		private static void ActivateMainForm(MainForm form)
		{
			if (form.IsDisposed || form.Disposing || !form.IsHandleCreated)
			{
				return;
			}

			try
			{
				// Сигнал приходит из пула потоков — переключаемся в UI-поток
				form.BeginInvoke(new Action(form.ActivateFromSecondInstance));
			}
			catch (InvalidOperationException)
			{
				// Окно уже закрывается — активировать нечего
			}
		}
	}
}