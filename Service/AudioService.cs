using NAudio.Wave;
using System.IO;

namespace MiniPomidoroWin.Service
{
	/// <summary>
	/// Сервис воспроизведения мелодии окончания таймера
	/// </summary>
	internal sealed class AudioService : IDisposable
	{
		/// <summary>
		/// Синхронизация доступа к полям воспроизведения
		/// </summary>
		private readonly object _sync = new object();

		/// <summary>
		/// Устройство вывода
		/// </summary>
		private IWavePlayer? _output;

		/// <summary>
		/// Текущий аудиопоток
		/// </summary>
		private WaveStream? _reader;

		/// <summary>
		/// Признак освобождения сервиса
		/// </summary>
		private bool _disposed;

		/// <summary>
		/// Воспроизведение пользовательского файла
		/// </summary>
		public void PlayCustomFile(string filePath)
		{
			ObjectDisposedException.ThrowIf(this._disposed, this);

			this.Play(new AudioFileReader(filePath));
		}

		/// <summary>
		/// Воспроизведение встроенной мелодии
		/// </summary>
		public void PlayEmbedded(UnmanagedMemoryStream mp3Stream)
		{
			ObjectDisposedException.ThrowIf(this._disposed, this);

			this.Play(new Mp3FileReader(mp3Stream));
		}

		/// <summary>
		/// Остановка воспроизведения
		/// </summary>
		public void Stop()
		{
			IWavePlayer? output;
			WaveStream? reader;

			lock (this._sync)
			{
				output = this._output;
				reader = this._reader;

				this._output = null;
				this._reader = null;
			}

			DisposePlayback(output, reader);
		}

		/// <summary>
		/// Освобождение ресурсов
		/// </summary>
		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}

			this._disposed = true;
			this.Stop();
		}

		/// <summary>
		/// Запуск нового потока с остановкой предыдущего
		/// </summary>
		private void Play(WaveStream reader)
		{
			IWavePlayer output = new WaveOut();

			try
			{
				output.Init(reader);
			}
			catch
			{
				output.Dispose();
				reader.Dispose();
				throw;
			}

			IWavePlayer? previousOutput;
			WaveStream? previousReader;

			lock (this._sync)
			{
				if (this._disposed)
				{
					output.Dispose();
					reader.Dispose();
					return;
				}

				previousOutput = this._output;
				previousReader = this._reader;

				this._output = output;
				this._reader = reader;
			}

			// Предыдущий звук останавливаем только после подмены полей,
			// чтобы его остановка не могла затронуть новый поток
			DisposePlayback(previousOutput, previousReader);

			try
			{
				output.Play();
			}
			catch
			{
				this.Stop();
				throw;
			}
		}

		/// <summary>
		/// Остановка и освобождение конкретного воспроизведения
		/// </summary>
		private static void DisposePlayback(IWavePlayer? output, WaveStream? reader)
		{
			if (output != null)
			{
				output.Stop();
				output.Dispose();
			}

			reader?.Dispose();
		}
	}
}