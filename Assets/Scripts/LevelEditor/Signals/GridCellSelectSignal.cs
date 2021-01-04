using UnityEngine;

namespace LevelEditor.Signals
{
	/// <summary>
	/// Сигнал выбора ячейки разметочной сетки.
	/// </summary>
	public class GridCellSelectSignal
	{
		public Vector2Int CellPosition; /// Позиция выбранной ячейки.
	}
}