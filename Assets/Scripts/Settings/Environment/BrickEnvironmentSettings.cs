using Model.Environment;
using UnityEngine;

namespace Settings.Environment
{
	[CreateAssetMenu(fileName = "BrickEnvironmentSettings", menuName = "Settings/Environment/BrickEnvironmentSettings")]
	public class BrickEnvironmentSettings : EnvironmentItemSettingsBase<BrickEnvironmentSettings>
	{
		protected override EnvironmentItemType ItemType => EnvironmentItemType.Brick;
	}
}