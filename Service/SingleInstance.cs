using System.Threading;

namespace MiniPomidoroWin.Service
{
	/// <summary>
	/// Единственный экземпляр приложения и активация уже запущенного окна
	/// </summary>
	internal sealed class SingleInstance : IDisposable
	{
		/// <summary>
		/// Имя мьютекса в пространстве текущей сессии пользователя
		/// </summary>
		private const string MutexName = @"Local\MiniPomodoroWin.SingleInstance";

		/// <summary>
		/// Имя события «покажи окно»
		/// </summary>
		private const string ActivationEventName = @"Local\MiniPomodoroWin.Activate";

		/// <summary>
		/// Мьютекс единственного экземпляра
		/// </summary>
		private readonly Mutex _mutex;

		/// <summary>
		/// Событие активации окна
		/// </summary>
		private readonly EventWaitHandle _activationEvent;

		/// <summary>
		/// Ожидание сигнала активации
		/// </summary>
		private RegisteredWaitHandle? _waiter;

		/// <summary>
		/// Ресурсы освобождены
		/// </summary>
		private bool _disposed;

		/// <summary>
		/// Конструктор: определяет, первый ли это экземпляр
		/// </summary>
		public SingleInstance()
		{
			this._mutex = new Mutex(initiallyOwned: true, MutexName, out bool createdNew);

			this.IsFirstInstance = createdNew;

			this._activationEvent = new EventWaitHandle(
				initialState: false,
				EventResetMode.AutoReset,
				ActivationEventName);
		}

		/// <summary>
		/// Второй экземпляр просит показать окно первого
		/// </summary>
		public event EventHandler? ActivationRequested;

		/// <summary>
		/// Приложение запущено первым
		/// </summary>
		public bool IsFirstInstance { get; }

		/// <summary>
		/// Сигнал уже запущенному экземпляру
		/// </summary>
		public void ActivateFirstInstance()
		{
			this._activationEvent.Set();
		}

		/// <summary>
		/// Ожидание сигналов от повторных запусков
		/// </summary>
		public void StartListening()
		{
			this._waiter ??= ThreadPool.RegisterWaitForSingleObject(
				this._activationEvent,
				this.OnActivationSignal,
				state: null,
				Timeout.Infinite,
				executeOnlyOnce: false);
		}

		/// <summary>
		/// Освобождение ресурсов и мьютекса
		/// </summary>
		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}

			this._disposed = true;

			this._waiter?.Unregister(null);
			this._waiter = null;

			this._activationEvent.Dispose();

			if (this.IsFirstInstance)
			{
				try
				{
					this._mutex.ReleaseMutex();
				}
				catch (ApplicationException)
				{
					// Мьютекс уже освобождён или принадлежит другому потоку
				}
			}

			this._mutex.Dispose();
		}

		/// <summary>
		/// Обработка сигнала от второго экземпляра
		/// </summary>
		private void OnActivationSignal(object? state, bool timedOut)
		{
			if (timedOut || this._disposed)
			{
				return;
			}

			this.ActivationRequested?.Invoke(this, EventArgs.Empty);
		}
	}
}