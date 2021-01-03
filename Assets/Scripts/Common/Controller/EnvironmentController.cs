using Model.Environment;
using Settings.Environment;
using UnityEngine;
using Zenject;

namespace Common.Controller
{
	[DisallowMultipleComponent]
	public class EnvironmentController : MonoBehaviour
	{
		private static readonly Vector3 ItemSize = Vector3.one;

#pragma warning disable 649
		[Inject] private readonly DiContainer _container;
		[Inject] private readonly IEnvironmentSettings _environmentSettings;
		[Inject] private IEnvironment _model;
#pragma warning restore 649

		private void Start()
		{
			if (_model == null) return;

			var t = transform;
			Vector2 offset = ItemSize * 0.5f;
			foreach (var item in _model.Items)
			{
				var settings = _environmentSettings.ItemSettingsMap[item.Type];
				var itemView = _container.InstantiatePrefab(settings.Prefab, t);
				itemView.transform.localPosition = new Vector3(ItemSize.x * item.Position.x + offset.x,
					ItemSize.y * item.Position.y + offset.y, 0);
			}
		}
	}
}