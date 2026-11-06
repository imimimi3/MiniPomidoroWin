using NAudio.Wave;
using System.IO;

namespace MiniPomidoroWin.Service
{
	internal class AudioService : IDisposable
	{
		/// <summary>
		/// 
		/// </summary>
		private IWavePlayer? _outputDevice;

		/// <summary>
		/// Аудиофайл выбранный пользователем
		/// </summary>
		private AudioFileReader? _audioFile;

		/// <summary>
		/// Для встроенной мелодии
		/// </summary>
		private WaveOut? _embeddedOutput;

		/// <summary>
		/// Для встроенной мелодии
		/// </summary>
		private Mp3FileReader? _embeddedReader;

		/// <summary>
		/// Воспроизведение пользовательского файла мелодии
		/// </summary>
		public void PlayCustomFile(string filePath)
		{
			Stop(); 

			_audioFile = new AudioFileReader(filePath);
			_outputDevice = new WaveOut();
			_outputDevice.Init(_audioFile);
			_outputDevice.Play();
		}

		/// <summary>
		/// Воспроизведение встроенной мелодии
		/// </summary>
		public void PlayEmbedded(UnmanagedMemoryStream mp3Stream)
		{
			Stop();

			_embeddedReader = new Mp3FileReader(mp3Stream);
			_embeddedOutput = new WaveOut();

			_embeddedOutput.PlaybackStopped += (s, e) =>
			{
				_embeddedOutput?.Dispose();
				_embeddedReader?.Dispose();
				_embeddedOutput = null;
				_embeddedReader = null;
			};

			_embeddedOutput.Init(_embeddedReader);
			_embeddedOutput.Play();
		}

		/// <summary>
		/// Выключение воспроизведения
		/// </summary>
		public void Stop()
		{
			if (_outputDevice != null)
			{
				_outputDevice.Stop();
				_outputDevice.Dispose();
				_outputDevice = null;
			}

			if (_audioFile != null)
			{
				_audioFile.Dispose();
				_audioFile = null;
			}

			_embeddedOutput?.Stop();
		}

		/// <summary>
		/// Освобождение ресурсов
		/// </summary>
		public void Dispose() => Stop();
	}
}
