using System.Collections.Generic;
using UnityEngine;

namespace Model.Environment
{
	/// <summary>
	/// Интерфейс модели игрового уровня.
	/// </summary>
	public interface IEnvironment
	{
		/// <summary>
		/// Размер уровня в ячейках.
		/// </summary>
		Vector2Int Size { get; }
		
		/// <summary>
		/// Список элементов сцены игрового уровня.
		/// </summary>
		IReadOnlyList<IEnvironmentItem> Items { get; }
	}
}