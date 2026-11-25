using Microsoft.Win32;
using System.Diagnostics;
using System.IO;

namespace MiniPomidoroWin.Service
{
	/// <summary>
	/// Автозапуск приложения вместе с Windows
	/// </summary>
	internal static class AutoStartService
	{
		/// <summary>
		/// Раздел реестра со списком автозагрузки текущего пользователя
		/// </summary>
		private const string RunKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";

		/// <summary>
		/// Раздел с состоянием автозагрузки (Диспетчер задач → Автозагрузка)
		/// </summary>
		private const string ApprovedKeyPath =
			@"Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run";

		/// <summary>
		/// Имя значения в реестре
		/// </summary>
		private const string ValueName = "MiniPomodoroWin";

		/// <summary>
		/// Аргументы запуска при автозагрузке: сразу в трей, без окна
		/// </summary>
		private const string StartupArguments = "--tray";

		/// <summary>
		/// Путь к исполняемому файлу приложения
		/// </summary>
		private static string? ExecutablePath => Environment.ProcessPath;

		/// <summary>
		/// Автозапуск можно настроить для этого способа запуска
		/// </summary>
		public static bool IsSupported
		{
			get
			{
				string? path = ExecutablePath;

				return !string.IsNullOrEmpty(path)
					&& File.Exists(path)
					&& !string.Equals(Path.GetFileName(path), "dotnet.exe", StringComparison.OrdinalIgnoreCase);
			}
		}

		/// <summary>
		/// Автозапуск включён и не отключён пользователем в Windows
		/// </summary>
		public static bool IsEnabled
		{
			get
			{
				try
				{
					using (RegistryKey? runKey = Registry.CurrentUser.OpenSubKey(RunKeyPath))
					{
						if (runKey?.GetValue(ValueName) is not string command || command.Length == 0)
						{
							return false;
						}
					}

					return !IsDisabledByUser();
				}
				catch (Exception ex)
				{
					Trace.TraceError("Не удалось прочитать состояние автозапуска: {0}", ex);

					return false;
				}
			}
		}

		/// <summary>
		/// Включение или выключение автозапуска
		/// </summary>
		public static void SetEnabled(bool enabled)
		{
			if (enabled && !IsSupported)
			{
				throw new InvalidOperationException("Автозапуск недоступен для этого способа запуска.");
			}

			using (RegistryKey runKey = Registry.CurrentUser.CreateSubKey(RunKeyPath, writable: true)
				?? throw new InvalidOperationException("Не удалось открыть раздел автозагрузки."))
			{
				if (enabled)
				{
					runKey.SetValue(ValueName, BuildCommand(), RegistryValueKind.String);
				}
				else
				{
					runKey.DeleteValue(ValueName, throwOnMissingValue: false);
				}
			}

			// Отметка «отключено» из Диспетчера задач больше не актуальна
			using (RegistryKey? approved = Registry.CurrentUser.OpenSubKey(ApprovedKeyPath, writable: true))
			{
				approved?.DeleteValue(ValueName, throwOnMissingValue: false);
			}
		}

		/// <summary>
		/// Обновление пути, если exe перенесли в другое место
		/// </summary>
		public static void UpdatePathIfNeeded()
		{
			if (!IsSupported)
			{
				return;
			}

			try
			{
				using (RegistryKey? runKey = Registry.CurrentUser.OpenSubKey(RunKeyPath, writable: true))
				{
					if (runKey?.GetValue(ValueName) is not string current || current.Length == 0)
					{
						return;
					}

					string expected = BuildCommand();

					if (!string.Equals(current, expected, StringComparison.OrdinalIgnoreCase))
					{
						runKey.SetValue(ValueName, expected, RegistryValueKind.String);
					}
				}
			}
			catch (Exception ex)
			{
				Trace.TraceError("Не удалось обновить путь автозапуска: {0}", ex);
			}
		}

		/// <summary>
		/// Команда запуска: путь в кавычках и аргумент «в трей»
		/// </summary>
		private static string BuildCommand()
		{
			string path = ExecutablePath
				?? throw new InvalidOperationException("Не удалось определить путь приложения.");

			return $"\"{path}\" {StartupArguments}";
		}

		/// <summary>
		/// Пользователь отключил приложение в Диспетчере задач
		/// </summary>
		private static bool IsDisabledByUser()
		{
			using RegistryKey? approved = Registry.CurrentUser.OpenSubKey(ApprovedKeyPath);

			if (approved?.GetValue(ValueName) is not byte[] state || state.Length == 0)
			{
				return false;
			}

			// 0x02 — включено, 0x03 — отключено пользователем
			return (state[0] & 1) != 0;
		}
	}
}