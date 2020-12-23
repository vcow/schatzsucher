using Model.Environment;
using UnityEngine;

namespace Settings.Environment
{
	[CreateAssetMenu(fileName = "WoodEnvironmentSettings", menuName = "Settings/Environment/WoodEnvironmentSettings")]
	public class WoodEnvironmentSettings : EnvironmentItemSettingsBase<WoodEnvironmentSettings>
	{
		protected override EnvironmentItemType ItemType => EnvironmentItemType.Wood;
	}
}