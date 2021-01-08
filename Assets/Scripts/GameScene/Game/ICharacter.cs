using UnityEngine;

namespace GameScene.Game
{
	/// <summary>
	/// Интерфейс любого персонажа в сцене.
	/// </summary>
	public interface ICharacter
	{
		/// <summary>
		/// Уникальный идентификатор персонажа.
		/// </summary>
		string Id { get; }

		/// <summary>
		/// Границы персонажа в сцене.
		/// </summary>
		Bounds Bounds { get; }
	}
}