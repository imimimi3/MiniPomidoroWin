using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniPomidoroWin.Service
{
	/// <summary>
	/// Сервис локализации: переключение языка интерфейса без перезапуска приложения
	/// </summary>
	internal class LocalizationService
	{
		/// <summary>
		/// Имя культуры русского языка
		/// </summary>
		private const string RussianCultureName = "ru";

		/// <summary>
		/// Имя культуры английского языка
		/// </summary>
		private const string EnglishCultureName = "en";

		private static readonly Lazy<LocalizationService> _instance =
			new Lazy<LocalizationService>(() => new LocalizationService());

		/// <summary>
		/// Единственный экземпляр сервиса
		/// </summary>
		public static LocalizationService Instance => _instance.Value;

		private LocalizationService()
		{
			this.CurrentLanguage = ParseLanguage(Properties.Settings.Default.Language);
		}

		/// <summary>
		/// Текущий язык интерфейса
		/// </summary>
		public AppLanguage CurrentLanguage { get; private set; }

		/// <summary>
		/// Событие смены языка интерфейса
		/// </summary>
		public event EventHandler? LanguageChanged;

		/// <summary>
		/// Применение сохранённого языка. Вызывать один раз при старте приложения
		/// </summary>
		public void Initialize()
		{
			ApplyCulture(this.CurrentLanguage);
		}

		/// <summary>
		/// Установка языка интерфейса
		/// </summary>
		public void SetLanguage(AppLanguage language)
		{
			if (language == this.CurrentLanguage)
			{
				return;
			}

			this.CurrentLanguage = language;
			ApplyCulture(language);
			SaveLanguage(language);

			this.LanguageChanged?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		/// Переключение языка на противоположный
		/// </summary>
		public void ToggleLanguage()
		{
			AppLanguage next = this.CurrentLanguage == AppLanguage.Russian
				? AppLanguage.English
				: AppLanguage.Russian;

			this.SetLanguage(next);
		}

		/// <summary>
		/// Применение культуры к текущему и будущим потокам
		/// </summary>
		private static void ApplyCulture(AppLanguage language)
		{
			CultureInfo culture = language == AppLanguage.English
				? CultureInfo.GetCultureInfo(EnglishCultureName)
				: CultureInfo.GetCultureInfo(RussianCultureName);

			CultureInfo.DefaultThreadCurrentUICulture = culture;
			Thread.CurrentThread.CurrentUICulture = culture;
		}

		/// <summary>
		/// Разбор сохранённого значения языка
		/// </summary>
		private static AppLanguage ParseLanguage(string? value)
		{
			return string.Equals(value, EnglishCultureName, StringComparison.OrdinalIgnoreCase)
				? AppLanguage.English
				: AppLanguage.Russian;
		}

		/// <summary>
		/// Сохранение выбранного языка в настройки
		/// </summary>
		private static void SaveLanguage(AppLanguage language)
		{
			Properties.Settings.Default.Language = language == AppLanguage.English
				? EnglishCultureName
				: RussianCultureName;

			Properties.Settings.Default.Save();
		}
	}
}
