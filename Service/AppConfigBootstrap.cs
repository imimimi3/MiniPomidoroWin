// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 imimimi3

using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace MiniPomidoroWin.Service
{
	/// <summary>
	/// РџРѕРґРєР»СЋС‡РµРЅРёРµ РєРѕРЅС„РёРіСѓСЂР°С†РёРё РїСЂРёР»РѕР¶РµРЅРёСЏ РІ single-file СЃР±РѕСЂРєРµ.
	/// Р’ single-file РїСЂРёР»РѕР¶РµРЅРёРё СЂСЏРґРѕРј СЃ exe РЅРµС‚ С„Р°Р№Р»Р° РєРѕРЅС„РёРіСѓСЂР°С†РёРё, РїРѕСЌС‚РѕРјСѓ
	/// РѕРЅ РёР·РІР»РµРєР°РµС‚СЃСЏ РёР· СЂРµСЃСѓСЂСЃРѕРІ РІ РїР°РїРєСѓ Р»РѕРєР°Р»СЊРЅС‹С… РґР°РЅРЅС‹С… Рё РїРѕРґРєР»СЋС‡Р°РµС‚СЃСЏ РІСЂСѓС‡РЅСѓСЋ.
	/// </summary>
	internal static class AppConfigBootstrap
	{
		/// <summary>
		/// РРјСЏ РїР°РїРєРё РїСЂРёР»РѕР¶РµРЅРёСЏ РІ Р»РѕРєР°Р»СЊРЅС‹С… РґР°РЅРЅС‹С…
		/// </summary>
		private const string ApplicationFolderName = "MiniPomidoroWin";

		/// <summary>
		/// РРјСЏ С„Р°Р№Р»Р° РєРѕРЅС„РёРіСѓСЂР°С†РёРё, РєРѕС‚РѕСЂРѕРµ РѕР¶РёРґР°РµС‚ System.Configuration
		/// </summary>
		private const string ConfigFileName = "MiniPomidoroWin.dll.config";

		/// <summary>
		/// РРјСЏ РІСЃС‚СЂРѕРµРЅРЅРѕРіРѕ СЂРµСЃСѓСЂСЃР° СЃ РєРѕРЅС„РёРіСѓСЂР°С†РёРµР№
		/// </summary>
		private const string ConfigResourceName = "MiniPomidoroWin.App.config";

		/// <summary>
		/// РџСЂРёР·РЅР°Рє РІС‹РїРѕР»РЅРµРЅРЅРѕР№ РёРЅРёС†РёР°Р»РёР·Р°С†РёРё
		/// </summary>
		private static bool _initialized;

		/// <summary>
		/// РџРѕРґРєР»СЋС‡РµРЅРёРµ РєРѕРЅС„РёРіСѓСЂР°С†РёРё. Р’С‹Р·С‹РІР°С‚СЊ РґРѕ РїРµСЂРІРѕРіРѕ РѕР±СЂР°С‰РµРЅРёСЏ Рє Properties.Settings
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
				Trace.TraceError("РќРµ СѓРґР°Р»РѕСЃСЊ РїРѕРґРєР»СЋС‡РёС‚СЊ РєРѕРЅС„РёРіСѓСЂР°С†РёСЋ РёР· Р»РѕРєР°Р»СЊРЅС‹С… РґР°РЅРЅС‹С…: {0}", ex);

				try
				{
					// Р РµР·РµСЂРІРЅС‹Р№ РїСѓС‚СЊ: РІСЂРµРјРµРЅРЅС‹Р№ РєР°С‚Р°Р»РѕРі РїРѕР»СЊР·РѕРІР°С‚РµР»СЏ
					UseConfig(ExtractConfig(Path.Combine(Path.GetTempPath(), ApplicationFolderName)));
				}
				catch (Exception fallbackError)
				{
					Trace.TraceError("РќРµ СѓРґР°Р»РѕСЃСЊ РїРѕРґРєР»СЋС‡РёС‚СЊ СЂРµР·РµСЂРІРЅСѓСЋ РєРѕРЅС„РёРіСѓСЂР°С†РёСЋ: {0}", fallbackError);
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
		/// РР·РІР»РµС‡РµРЅРёРµ РІСЃС‚СЂРѕРµРЅРЅРѕР№ РєРѕРЅС„РёРіСѓСЂР°С†РёРё РІ РїР°РїРєСѓ Р»РѕРєР°Р»СЊРЅС‹С… РґР°РЅРЅС‹С…
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
		/// Р§С‚РµРЅРёРµ РІСЃС‚СЂРѕРµРЅРЅРѕР№ РєРѕРЅС„РёРіСѓСЂР°С†РёРё
		/// </summary>
		private static byte[] ReadEmbeddedConfig()
		{
			Assembly assembly = typeof(AppConfigBootstrap).Assembly;

			using Stream? stream = assembly.GetManifestResourceStream(ConfigResourceName);

			if (stream == null)
			{
				// РљРѕРЅС„РёРіСѓСЂР°С†РёСЏ Р±РµР· СЃРµРєС†РёР№: Р·РЅР°С‡РµРЅРёСЏ РїРѕ СѓРјРѕР»С‡Р°РЅРёСЋ Р±РµСЂСѓС‚СЃСЏ РёР· Р°С‚СЂРёР±СѓС‚РѕРІ РЅР°СЃС‚СЂРѕРµРє
				return "<?xml version=\"1.0\" encoding=\"utf-8\"?><configuration />"u8.ToArray();
			}

			using MemoryStream buffer = new MemoryStream();
			stream.CopyTo(buffer);

			return buffer.ToArray();
		}

		/// <summary>
		/// РЎРѕРІРїР°РґР°РµС‚ Р»Рё СЃРѕРґРµСЂР¶РёРјРѕРµ С„Р°Р№Р»Р° СЃ РѕР¶РёРґР°РµРјС‹Рј
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
				Trace.TraceError("РќРµ СѓРґР°Р»РѕСЃСЊ РїСЂРѕС‡РёС‚Р°С‚СЊ С„Р°Р№Р» РєРѕРЅС„РёРіСѓСЂР°С†РёРё {0}: {1}", path, ex);
				return false;
			}
		}

		/// <summary>
		/// РЎР±СЂРѕСЃ Р·Р°РєСЌС€РёСЂРѕРІР°РЅРЅС‹С… РїСѓС‚РµР№ System.Configuration,
		/// С‡С‚РѕР±С‹ РєРѕРЅС„РёРіСѓСЂР°С†РёСЏ С‡РёС‚Р°Р»Р°СЃСЊ РёР· РЅРѕРІРѕРіРѕ С„Р°Р№Р»Р°
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
