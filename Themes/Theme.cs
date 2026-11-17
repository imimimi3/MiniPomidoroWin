namespace MiniPomidoroWin.Themes
{
	internal class Theme
	{
		public static readonly Color TextColor = Color.White;

		// Кнопки
		public static readonly Color ButtonPrimary = Color.FromArgb(153, 102, 204); // Лавандовый
		public static readonly Color ButtonHover = Color.FromArgb(173, 122, 224); // Светлый лавандовый
		public static readonly Color ButtonPressed = Color.FromArgb(133, 82, 184); // Темный лавандовый

		// Таймер
		public static readonly Color TimerTrack = Color.FromArgb(57, 62, 70); // Темно-серый фон круга
		public static readonly Color TimerProgress = Color.FromArgb(192, 64, 0); // Оранжевый

		// Цвета для формы
		public static readonly Color FormBackground = Color.FromArgb(34, 40, 49); // Темный фон форм

		// Поля ввода времени
		public static readonly Color FieldBackground = Color.FromArgb(44, 52, 64); // Фон поля
		public static readonly Color FieldBorder = Color.FromArgb(58, 68, 82); // Рамка поля
		public static readonly Color FieldCaption = Color.FromArgb(138, 147, 165); // Подписи и стрелки

		// Второстепенные кнопки
		public static readonly Color ButtonSecondary = FieldBackground; // Фон
		public static readonly Color ButtonSecondaryHover = FieldBorder; // Наведение
		public static readonly Color ButtonSecondaryPressed = Color.FromArgb(36, 44, 55); // Нажатие
	}
}