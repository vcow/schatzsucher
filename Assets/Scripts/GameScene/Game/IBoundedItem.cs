using UnityEngine;

namespace GameScene.Game
{
	/// <summary>
	/// Базовый интерфейс элемента сцены, имеющего заданные границы.
	/// </summary>
	public interface IBoundedItem
	{
		/// <summary>
		/// Границы элемента в сцене.
		/// </summary>
		Bounds Bounds { get; }
	}
}