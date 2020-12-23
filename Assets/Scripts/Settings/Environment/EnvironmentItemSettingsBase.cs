using Model.Environment;
using UnityEngine;
using Zenject;

namespace Settings.Environment
{
	public abstract class EnvironmentItemSettingsBase<T> : ScriptableObjectInstaller<T>, IEnvironmentItemSettings
		where T : ScriptableObjectInstaller<T>
	{
#pragma warning disable 649
		[SerializeField] private GameObject _prefab;
#pragma warning restore 649

		protected abstract EnvironmentItemType ItemType { get; }

		public override void InstallBindings()
		{
			Container.Bind<IEnvironmentItemSettings>().WithId(ItemType).FromInstance(this).AsCached();
		}

		public GameObject Prefab => _prefab;
	}
}