using System.Collections.Generic;
using Model.Environment;

namespace Settings.Environment
{
	public interface IEnvironmentSettings
	{
		IReadOnlyDictionary<EnvironmentItemType, IEnvironmentItemSettings> ItemSettingsMap { get; }
	}
}