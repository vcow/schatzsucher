using System.Collections.Generic;
using Model.Environment;

namespace Settings.Environment
{
	/// <summary>
	/// Интерфейс настроек представления уровня.
	/// </summary>
	public interface IEnvironmentSettings
	{
		/// <summary>
		/// Словарь настроек элементов уровня где в качестве ключа выступает тип элемента.
		/// </summary>
		IReadOnlyDictionary<EnvironmentItemType, IEnvironmentItemSettings> ItemSettingsMap { get; }
	}
}