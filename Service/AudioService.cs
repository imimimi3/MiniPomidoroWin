// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 imimimi3

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
		/// Номер текущего воспроизведения
		/// </summary>
		private int _playbackId;

		/// <summary>
		/// Номер воспроизведения, остановленного вручную
		/// </summary>
		private int _stoppedPlaybackId = -1;

		/// <summary>
		/// Событие естественного окончания воспроизведения
		/// </summary>
		public event EventHandler? PlaybackFinished;

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
		public void PlayEmbedded(Stream stream)
		{
			ObjectDisposedException.ThrowIf(this._disposed, this);

			this.Play(CreateEmbeddedReader(stream));
		}

		/// <summary>
		/// Чтение встроенной мелодии с определением формата по заголовку
		/// </summary>
		private static WaveStream CreateEmbeddedReader(Stream stream)
		{
			return IsWave(stream)
				? new WaveFileReader(stream)
				: new Mp3FileReader(stream);
		}

		/// <summary>
		/// Проверка, что поток содержит WAV (RIFF....WAVE)
		/// </summary>
		private static bool IsWave(Stream stream)
		{
			long position = stream.Position;
			byte[] header = new byte[12];

			try
			{
				int read = stream.Read(header, 0, header.Length);

				return read == header.Length
					&& header[0] == (byte)'R'
					&& header[1] == (byte)'I'
					&& header[2] == (byte)'F'
					&& header[3] == (byte)'F'
					&& header[8] == (byte)'W'
					&& header[9] == (byte)'A'
					&& header[10] == (byte)'V'
					&& header[11] == (byte)'E';
			}
			finally
			{
				stream.Position = position;
			}
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
				this._stoppedPlaybackId = this._playbackId;

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
			int playbackId;

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

				playbackId = ++this._playbackId;

				previousOutput = this._output;
				previousReader = this._reader;

				this._output = output;
				this._reader = reader;
			}

			output.PlaybackStopped += (sender, args) => this.OnPlaybackStopped(playbackId);

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
		/// Обработка окончания воспроизведения
		/// </summary>
		private void OnPlaybackStopped(int playbackId)
		{
			IWavePlayer? output;
			WaveStream? reader;

			lock (this._sync)
			{
				if (this._disposed ||
					playbackId != this._playbackId ||
					playbackId == this._stoppedPlaybackId)
				{
					return;
				}

				this._stoppedPlaybackId = playbackId;

				output = this._output;
				reader = this._reader;

				this._output = null;
				this._reader = null;
			}

			// Здесь воспроизведение уже остановилось естественным образом,
			// поэтому дополнительный Stop не нужен
			output?.Dispose();
			reader?.Dispose();

			this.PlaybackFinished?.Invoke(this, EventArgs.Empty);
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