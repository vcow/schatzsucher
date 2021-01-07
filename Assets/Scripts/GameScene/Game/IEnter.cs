using GameScene.Controller;
using UniRx;

namespace GameScene.Game
{
	/// <summary>
	/// Интерфейс точки спавна игрока.
	/// </summary>
	public interface IEnter
	{
		/// <summary>
		/// Признак того, что точка занята каким-то из персонажей.
		/// </summary>
		IReadOnlyReactiveProperty<bool> IsBusy { get; }

		/// <summary>
		/// Спавн игрока.
		/// </summary>
		/// <param name="player">Контроллер представления игрока в сцене.</param>
		void Spawn(PlayerController player);
	}
}