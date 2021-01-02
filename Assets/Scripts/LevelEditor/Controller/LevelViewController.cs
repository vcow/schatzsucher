using System.Linq;
using Common.Controller;
using LevelEditor.Signals;
using Model.Environment;
using UnityEngine;
using Zenject;

namespace LevelEditor.Controller
{
	[DisallowMultipleComponent]
	public class LevelViewController : MonoBehaviour
	{
		private const float CellSize = 100f;
		private const float ScaleFactor = 3f;
		private const float CameraDistance = 20f;

		private float _zoom;

		private readonly EnvironmentModel _environmentModel = new EnvironmentModel {Size = Vector2Int.one * 10};
		private EnvironmentController _environment;

		private EnvironmentItemType _environmentItemType = EnvironmentItemType.None;

		private Transform _cameraTransform;
		private Rect _screenRect;
		private float _canvasScaleFactor;

		private Vector2 _cameraSceneSize;

#pragma warning disable 649
		[SerializeField] private RectTransform _gridContainer;
		[SerializeField] private Camera _camera;

		[Inject] private readonly DiContainer _container;
		[Inject] private readonly SignalBus _signalBus;
#pragma warning disable 649

		private void Start()
		{
			_signalBus.Subscribe<GridCellSelectSignal>(OnSelectCell);
			_signalBus.Subscribe<SelectEnvironmentItemSignal>(OnSelectEnvironmentItem);

			_environment = _container.InstantiateComponentOnNewGameObject<EnvironmentController>(
				"Environment", new object[] {_environmentModel});

			_cameraTransform = _camera.transform;
			_canvasScaleFactor = _gridContainer.GetComponentInParent<Canvas>().scaleFactor;
			_screenRect = new Rect(0, 0, Screen.width, Screen.height);

			_cameraTransform.position = Vector3.back * (CameraDistance + 0.5f);

			UpdateGrid();
		}

		private void OnDestroy()
		{
			_signalBus.Unsubscribe<GridCellSelectSignal>(OnSelectCell);
			_signalBus.Unsubscribe<SelectEnvironmentItemSignal>(OnSelectEnvironmentItem);
		}

		private void Update()
		{
			var pos = _gridContainer.position;
			var piv = _gridContainer.pivot;
			var sd = _gridContainer.rect.size;
			var gridRect = new Rect(pos.x - sd.x * _canvasScaleFactor * piv.x,
				pos.y - sd.y * _canvasScaleFactor * piv.y, sd.x * _canvasScaleFactor, sd.y * _canvasScaleFactor);

			var l = _screenRect.center - gridRect.center;

			_cameraTransform.position = new Vector3(
				_environmentModel.Size.x * 0.5f + l.x * _cameraSceneSize.x / _screenRect.width,
				_environmentModel.Size.y * 0.5f + l.y * _cameraSceneSize.y / _screenRect.height,
				_cameraTransform.position.z);
		}

		private void OnSelectCell(GridCellSelectSignal signal)
		{
			if (_environmentItemType == EnvironmentItemType.None)
			{
				var item = _environmentModel.Items.SingleOrDefault(i => i.Position == signal.CellPosition);
				if (item != null)
				{
					_environmentModel.Items.Remove(item);
					_environment.Model = _environmentModel;
				}
			}
			else
			{
				var newItem = new EnvironmentItem
				{
					Type = _environmentItemType,
					Position = signal.CellPosition
				};

				var oldItem = _environmentModel.Items.SingleOrDefault(i => i.Position == signal.CellPosition);
				if (oldItem != null)
				{
					if (newItem.Equals(oldItem)) return;
					_environmentModel.Items.Remove(oldItem);
				}

				_environmentModel.Items.Add(newItem);
				_environment.Model = _environmentModel;
			}
		}

		private void OnSelectEnvironmentItem(SelectEnvironmentItemSignal signal)
		{
			_environmentItemType = signal.ItemType;
		}

		public void OnZoom(float zoom)
		{
			_zoom = zoom;
			UpdateGrid();
		}

		public void OnWidthChanged(string value)
		{
			var width = Mathf.Max(int.Parse(value), 1);
			_environmentModel.Size = new Vector2Int(width, _environmentModel.Size.y);
			UpdateGrid();
		}

		public void OnHeightChanged(string value)
		{
			var height = Mathf.Max(int.Parse(value), 1);
			_environmentModel.Size = new Vector2Int(_environmentModel.Size.x, height);
			UpdateGrid();
		}

		private void UpdateGrid()
		{
			foreach (Transform child in _gridContainer)
			{
				Destroy(child.gameObject);
			}

			const float padding = 10f;
			var cellSize = Mathf.Lerp(CellSize, CellSize * ScaleFactor, _zoom);
			_gridContainer.sizeDelta = new Vector2(_environmentModel.Size.x * cellSize,
				_environmentModel.Size.y * cellSize) + Vector2.one * (padding * 2f);

			for (var y = 0; y < _environmentModel.Size.y; ++y)
			{
				for (var x = 0; x < _environmentModel.Size.x; ++x)
				{
					var cell = CreateCell(cellSize, x, y);
					cell.SetParent(_gridContainer, false);
					cell.anchoredPosition = new Vector2(x * cellSize + padding, y * cellSize + padding);
				}
			}

			_environment.Model = _environmentModel;

			var vertical = Screen.height / (cellSize * _canvasScaleFactor);
			var ang = Mathf.Atan2(vertical * 0.5f, CameraDistance) * 2f * Mathf.Rad2Deg;
			_camera.fieldOfView = ang;

			var lb = _camera.ViewportToWorldPoint(new Vector3(0, 0, CameraDistance));
			var rt = _camera.ViewportToWorldPoint(new Vector3(1, 1, CameraDistance));
			_cameraSceneSize = new Vector2(rt.x - lb.x, rt.y - lb.y);
		}

		private RectTransform CreateCell(float cellSize, int posX, int posY)
		{
			var cellContainer = new GameObject($"cell {posX}:{posY}", typeof(RectTransform))
				.GetComponent<RectTransform>();
			cellContainer.sizeDelta = Vector2.one * cellSize;
			cellContainer.pivot = Vector2.zero;
			cellContainer.anchorMin = Vector2.zero;
			cellContainer.anchorMax = Vector2.zero;

			const float gap = 2f;
			var cell = _container.InstantiateComponentOnNewGameObject<GridCellController>("view",
				new object[] {new Vector2Int(posX, posY), _environmentModel}).GetComponent<RectTransform>();
			cell.SetParent(cellContainer, false);
			cell.anchorMin = Vector2.zero;
			cell.anchorMax = Vector2.one;
			cell.offsetMin = Vector2.one * gap;
			cell.offsetMax = Vector2.one * -gap;

			return cellContainer;
		}
	}
}