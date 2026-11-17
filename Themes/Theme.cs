namespace MiniPomidoroWin.Themes
{
	/// <summary>
	/// Палитра приложения
	/// </summary>
	internal static class Theme
	{
		/// <summary>
		/// Основной цвет текста
		/// </summary>
		public static readonly Color TextColor = Color.White;

		/// <summary>
		/// Лавандовый фон основной кнопки
		/// </summary>
		public static readonly Color ButtonPrimary = Color.FromArgb(153, 102, 204);

		/// <summary>
		/// Светлый лавандовый: наведение на основную кнопку
		/// </summary>
		public static readonly Color ButtonHover = Color.FromArgb(173, 122, 224);

		/// <summary>
		/// Тёмный лавандовый: нажатие основной кнопки
		/// </summary>
		public static readonly Color ButtonPressed = Color.FromArgb(133, 82, 184);

		/// <summary>
		/// Тёмно-серый фон дорожки кругового таймера
		/// </summary>
		public static readonly Color TimerTrack = Color.FromArgb(57, 62, 70);

		/// <summary>
		/// Оранжевый прогресс кругового таймера
		/// </summary>
		public static readonly Color TimerProgress = Color.FromArgb(192, 64, 0);

		/// <summary>
		/// Тёмный фон форм
		/// </summary>
		public static readonly Color FormBackground = Color.FromArgb(34, 40, 49);

		/// <summary>
		/// Фон полей ввода времени и второстепенных кнопок
		/// </summary>
		public static readonly Color FieldBackground = Color.FromArgb(44, 52, 64);

		/// <summary>
		/// Рамка полей и наведение на второстепенную кнопку
		/// </summary>
		public static readonly Color FieldBorder = Color.FromArgb(58, 68, 82);

		/// <summary>
		/// Подписи и стрелки полей
		/// </summary>
		public static readonly Color FieldCaption = Color.FromArgb(138, 147, 165);

		/// <summary>
		/// Фон второстепенной кнопки
		/// </summary>
		public static readonly Color ButtonSecondary = FieldBackground;

		/// <summary>
		/// Наведение на второстепенную кнопку
		/// </summary>
		public static readonly Color ButtonSecondaryHover = FieldBorder;

		/// <summary>
		/// Нажатие второстепенной кнопки
		/// </summary>
		public static readonly Color ButtonSecondaryPressed = Color.FromArgb(36, 44, 55);
	}
}