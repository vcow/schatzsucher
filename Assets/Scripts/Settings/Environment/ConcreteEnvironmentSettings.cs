using Model.Environment;
using UnityEngine;

namespace Settings.Environment
{
	[CreateAssetMenu(fileName = "ConcreteEnvironmentSettings", menuName = "Settings/Environment/ConcreteEnvironmentSettings")]
	public class ConcreteEnvironmentSettings : EnvironmentItemSettingsBase<ConcreteEnvironmentSettings>
	{
		protected override EnvironmentItemType ItemType => EnvironmentItemType.Concrete;
	}
}