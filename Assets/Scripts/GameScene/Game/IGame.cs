using UniRx;

namespace GameScene.Game
{
	/// <summary>
	/// Интерфейс игры.
	/// </summary>
	public interface IGame
	{
		/// <summary>
		/// Признак того, что игра активна.
		/// </summary>
		IReadOnlyReactiveProperty<bool> IsActive { get; }

		/// <summary>
		/// Старт игрового процесса.
		/// </summary>
		void Start();

		/// <summary>
		/// Остановка игрового процесса.
		/// </summary>
		void Stop();
	}
}