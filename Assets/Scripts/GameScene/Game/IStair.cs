using UnityEngine;

namespace GameScene.Game
{
	/// <summary>
	/// Интерфейс лестницы.
	/// </summary>
	public interface IStair
	{
		/// <summary>
		/// Границы элемента в сцене.
		/// </summary>
		Bounds Bounds { get; }
	}
}