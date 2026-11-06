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
	}
}
