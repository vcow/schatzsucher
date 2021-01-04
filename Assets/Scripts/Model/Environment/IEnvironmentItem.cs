using UnityEngine;

namespace Model.Environment
{
	/// <summary>
	/// Интерфейс элемента игрового уровня.
	/// </summary>
	public interface IEnvironmentItem
	{
		/// <summary>
		/// Тип элемента.
		/// </summary>
		EnvironmentItemType Type { get; }
		
		/// <summary>
		/// Позиция элемента в представлении уровня.
		/// </summary>
		Vector2Int Position { get; }
	}
}