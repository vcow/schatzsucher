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

		private IEnvironment _model;

#pragma warning disable 649
		[Inject] private readonly DiContainer _container;
#pragma warning restore 649

		[Inject]
		private void Construct([InjectOptional] IEnvironment model)
		{
			Model = model;
		}

		public IEnvironment Model
		{
			set
			{
				_model = value;
				UpdateEnvironment();
			}

			private get => _model;
		}

		private void UpdateEnvironment()
		{
			var t = transform;
			foreach (Transform child in t)
			{
				Destroy(child.gameObject);
			}

			if (_model == null) return;

			Vector2 offset = ItemSize * 0.5f;
			foreach (var item in _model.Items)
			{
				var settings = _container.ResolveId<IEnvironmentItemSettings>(item.Type);
				var itemView = _container.InstantiatePrefab(settings.Prefab, t);
				itemView.transform.localPosition = new Vector3(ItemSize.x * item.Position.x + offset.x,
					ItemSize.y * item.Position.y + offset.y, 0);
			}
		}
	}
}