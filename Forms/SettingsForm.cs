using MiniPomidoroWin.Service;
using MiniPomidoroWin.Themes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniPomidoroWin.Forms
{
	public partial class SettingsForm : Form
	{
		public static string? CustomSoundPath
		{
			get => Properties.Settings.Default.CustomSoundPath;
			set
			{
				Properties.Settings.Default.CustomSoundPath = value;
				Properties.Settings.Default.Save();
			}
		}

		public static void ResetCustomSound()
		{
			CustomSoundPath = null;
		}
		private readonly AudioService _audioService = new AudioService();

		public SettingsForm()
		{
			InitializeComponent();

			this.BackColor = Theme.FormBackground;
		}

		/// <summary>
		/// Обработка нажатия на кнопку выбора мелодии
		/// </summary>
		private void btnChooseSound_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog ofd = new OpenFileDialog { Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*" })
			{
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					CustomSoundPath = ofd.FileName;

					Properties.Settings.Default.CustomSoundPath = CustomSoundPath;
					Properties.Settings.Default.Save();

					try
					{
						_audioService.PlayCustomFile(CustomSoundPath);
						MessageBox.Show("Пользовательский звук выбран и сохранен.", "Настройки звука", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					catch (Exception ex)
					{
						MessageBox.Show($"Не удалось воспроизвести файл: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
		}

		/// <summary>
		/// Обработка нажатия на кнопку сброса выбранной мелодии
		/// </summary>
		private void btnResetSound_Click(object sender, EventArgs e)
		{
			ResetCustomSound();

			MessageBox.Show("Пользовательский звук сброшен.", "Настройки звука", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
	}
}
