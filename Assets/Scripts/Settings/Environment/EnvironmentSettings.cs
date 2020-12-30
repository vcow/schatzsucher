using System;
using System.Collections.Generic;
using System.Linq;
using Model.Environment;
using UnityEngine;
using Zenject;

namespace Settings.Environment
{
	[CreateAssetMenu(fileName = "EnvironmentSettings", menuName = "Settings/Environment/EnvironmentSettings")]
	public class EnvironmentSettings : ScriptableObjectInstaller<EnvironmentSettings>, IEnvironmentSettings
	{
		private readonly Lazy<IReadOnlyDictionary<EnvironmentItemType, IEnvironmentItemSettings>> _itemSettingsMap;

#pragma warning disable 649
		[SerializeField] private List<EnvironmentItemSettings> _itemSettings = new List<EnvironmentItemSettings>();
#pragma warning restore 649

		public override void InstallBindings()
		{
			Container.Bind<IEnvironmentSettings>().FromInstance(this).AsSingle();
		}

		public EnvironmentSettings()
		{
			_itemSettingsMap = new Lazy<IReadOnlyDictionary<EnvironmentItemType, IEnvironmentItemSettings>>(
				() => _itemSettings.ToDictionary(settings => settings.Type,
					settings => (IEnvironmentItemSettings) settings));
		}

		public IReadOnlyDictionary<EnvironmentItemType, IEnvironmentItemSettings> ItemSettingsMap =>
			_itemSettingsMap.Value;
	}
}