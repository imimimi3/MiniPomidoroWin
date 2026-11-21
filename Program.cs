// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 imimimi3

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
			// Р’ single-file СЃР±РѕСЂРєРµ РєРѕРЅС„РёРіСѓСЂР°С†РёСЏ РёР·РІР»РµРєР°РµС‚СЃСЏ РёР· СЂРµСЃСѓСЂСЃРѕРІ
			// РґРѕ РїРµСЂРІРѕРіРѕ РѕР±СЂР°С‰РµРЅРёСЏ Рє РЅР°СЃС‚СЂРѕР№РєР°Рј РїСЂРёР»РѕР¶РµРЅРёСЏ
			AppConfigBootstrap.Initialize();

			// To customize application configuration such as set high DPI settings or default font,
			// see https://aka.ms/applicationconfiguration.
			ApplicationConfiguration.Initialize();

			// РџСЂРёРјРµРЅРµРЅРёРµ СЃРѕС…СЂР°РЅС‘РЅРЅРѕРіРѕ СЏР·С‹РєР° РёРЅС‚РµСЂС„РµР№СЃР° РґРѕ СЃРѕР·РґР°РЅРёСЏ С„РѕСЂРј
			LocalizationService.Instance.Initialize();

			Application.Run(new MainForm());
		}
	}
}