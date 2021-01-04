using UnityEngine;

namespace Settings.Environment
{
	/// <summary>
	/// Интерфейс настроек элемента уровня.
	/// </summary>
	public interface IEnvironmentItemSettings
	{
		/// <summary>
		/// Префаб представления элемента.
		/// </summary>
		GameObject Prefab { get; }
	}
}