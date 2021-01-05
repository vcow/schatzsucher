using Model.Environment;
using Settings.Environment;
using UnityEngine;
using Zenject;

namespace Common.Controller
{
	/// <summary>
	/// Отрисовывает уровень. Требует наличия зависимости IEnvironment.
	/// </summary>
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
				var itemView = _container.InstantiatePrefabForComponent<EnvironmentItemController>(
					settings.Prefab, t, new object[] {item});
				itemView.transform.localPosition = new Vector3(ItemSize.x * item.Position.x + offset.x,
					ItemSize.y * item.Position.y + offset.y, 0);
				ProcessITemView(itemView);
			}
		}

		/// <summary>
		/// Виртуальный метод, позволяющий добавлять элементам уровня дополнительные контроллеры.
		/// </summary>
		/// <param name="itemView">Созданное представление элемента уровня.</param>
		protected virtual void ProcessITemView(EnvironmentItemController itemView)
		{
			// Override in children.
		}
	}
}