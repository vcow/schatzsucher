using Model.Environment;
using Settings.Environment;
using UnityEngine;
using Zenject;

namespace Common.Controller
{
	/// <summary>
	/// Базовый класс контроллера графического представления элемента игрового уровня.
	/// </summary>
	[DisallowMultipleComponent]
	public abstract class EnvironmentItemController : MonoBehaviour
	{
		protected IEnvironmentItem ItemModel { get; private set; }
		protected IEnvironmentItemSettings ItemSettings { get; private set; }

		[Inject]
		protected virtual void Construct(IEnvironmentItem model, IEnvironmentSettings settings)
		{
			ItemModel = model;
			ItemSettings = settings.ItemSettingsMap[model.Type];
		}
	}
}