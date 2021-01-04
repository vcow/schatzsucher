using Model.Environment;
using UnityEngine;

namespace Settings.Environment
{
	/// <summary>
	/// Настройки элемента уровня.
	/// </summary>
	[CreateAssetMenu(fileName = "EnvironmentItemSettings", menuName = "Settings/Environment/EnvironmentItemSettings")]
	public class EnvironmentItemSettings : ScriptableObject, IEnvironmentItemSettings
	{
#pragma warning disable 649
		[SerializeField] private EnvironmentItemType _type;
		[SerializeField] private GameObject _prefab;
#pragma warning restore 649

		/// <summary>
		/// Тип элемента.
		/// </summary>
		public EnvironmentItemType Type => _type;

		public GameObject Prefab => _prefab;
	}
}