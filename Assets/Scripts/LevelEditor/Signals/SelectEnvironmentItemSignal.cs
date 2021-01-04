using Model.Environment;

namespace LevelEditor.Signals
{
	/// <summary>
	/// Сигнал выбора текущего элемента редактора уровней.
	/// </summary>
	public class SelectEnvironmentItemSignal
	{
		public EnvironmentItemType ItemType; /// Тип выбранного элемента.
	}
}