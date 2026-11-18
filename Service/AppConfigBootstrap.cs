using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace MiniPomidoroWin.Service
{
	/// <summary>
	/// Подключение конфигурации приложения в single-file сборке.
	/// В single-file приложении рядом с exe нет файла конфигурации, поэтому
	/// он извлекается из ресурсов в папку локальных данных и подключается вручную.
	/// </summary>
	internal static class AppConfigBootstrap
	{
		/// <summary>
		/// Имя папки приложения в локальных данных
		/// </summary>
		private const string ApplicationFolderName = "MiniPomidoroWin";

		/// <summary>
		/// Имя файла конфигурации, которое ожидает System.Configuration
		/// </summary>
		private const string ConfigFileName = "MiniPomidoroWin.dll.config";

		/// <summary>
		/// Имя встроенного ресурса с конфигурацией
		/// </summary>
		private const string ConfigResourceName = "MiniPomidoroWin.App.config";

		/// <summary>
		/// Признак выполненной инициализации
		/// </summary>
		private static bool _initialized;

		/// <summary>
		/// Подключение конфигурации. Вызывать до первого обращения к Properties.Settings
		/// </summary>
		public static void Initialize()
		{
			if (_initialized)
			{
				return;
			}

			_initialized = true;

			if (File.Exists(Path.Combine(AppContext.BaseDirectory, ConfigFileName)))
			{
				return;
			}

			try
			{
				UseConfig(ExtractConfig(GetLocalConfigDirectory()));
			}
			catch (Exception ex)
			{
				Trace.TraceError("Не удалось подключить конфигурацию из локальных данных: {0}", ex);

				try
				{
					// Резервный путь: временный каталог пользователя
					UseConfig(ExtractConfig(Path.Combine(Path.GetTempPath(), ApplicationFolderName)));
				}
				catch (Exception fallbackError)
				{
					Trace.TraceError("Не удалось подключить резервную конфигурацию: {0}", fallbackError);
				}
			}
		}

		private static string GetLocalConfigDirectory()
		{
			return Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
				ApplicationFolderName);
		}

		/// <summary>
		/// Извлечение встроенной конфигурации в папку локальных данных
		/// </summary>
		private static string ExtractConfig(string directory)
		{
			byte[] content = ReadEmbeddedConfig();

			Directory.CreateDirectory(directory);

			string path = Path.Combine(directory, ConfigFileName);

			if (!IsSameContent(path, content))
			{
				File.WriteAllBytes(path, content);
			}

			return path;
		}

		private static void UseConfig(string path)
		{
			AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", path);
			ResetConfigurationManager();
		}

		/// <summary>
		/// Чтение встроенной конфигурации
		/// </summary>
		private static byte[] ReadEmbeddedConfig()
		{
			Assembly assembly = typeof(AppConfigBootstrap).Assembly;

			using Stream? stream = assembly.GetManifestResourceStream(ConfigResourceName);

			if (stream == null)
			{
				// Конфигурация без секций: значения по умолчанию берутся из атрибутов настроек
				return "<?xml version=\"1.0\" encoding=\"utf-8\"?><configuration />"u8.ToArray();
			}

			using MemoryStream buffer = new MemoryStream();
			stream.CopyTo(buffer);

			return buffer.ToArray();
		}

		/// <summary>
		/// Совпадает ли содержимое файла с ожидаемым
		/// </summary>
		private static bool IsSameContent(string path, byte[] content)
		{
			try
			{
				if (!File.Exists(path))
				{
					return false;
				}

				return File.ReadAllBytes(path).AsSpan().SequenceEqual(content);
			}
			catch (Exception ex)
			{
				Trace.TraceError("Не удалось прочитать файл конфигурации {0}: {1}", path, ex);
				return false;
			}
		}

		/// <summary>
		/// Сброс закэшированных путей System.Configuration,
		/// чтобы конфигурация читалась из нового файла
		/// </summary>
		private static void ResetConfigurationManager()
		{
			const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Static;

			Type managerType = typeof(ConfigurationManager);

			managerType.GetField("s_initState", flags)?.SetValue(null, 0);
			managerType.GetField("s_configSystem", flags)?.SetValue(null, null);
			managerType.GetField("s_initError", flags)?.SetValue(null, null);

			Type? pathsType = managerType.Assembly.GetType("System.Configuration.ClientConfigPaths");
			pathsType?.GetMethod("RefreshCurrent", flags)?.Invoke(null, null);
		}
	}
}
