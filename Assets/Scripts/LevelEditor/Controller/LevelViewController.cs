using LevelEditor.Signals;
using UnityEngine;
using Zenject;

namespace LevelEditor.Controller
{
	[DisallowMultipleComponent]
	public class LevelViewController : MonoBehaviour
	{
		private const float CellSize = 100f;
		private const float ScaleFactor = 3f;

		private float _zoom;
		private Vector2Int _levelSize = Vector2Int.one;

#pragma warning disable 649
		[SerializeField] private RectTransform _gridContainer;

		[Inject] private readonly DiContainer _container;
		[Inject] private readonly SignalBus _signalBus;
#pragma warning disable 649

		private void Start()
		{
			_signalBus.Subscribe<GridCellSelectSignal>(OnSelectCell);

			UpdateGrid();
		}

		private void OnDestroy()
		{
			_signalBus.Unsubscribe<GridCellSelectSignal>(OnSelectCell);
		}

		private void OnSelectCell(GridCellSelectSignal signal)
		{
			Debug.LogFormat("Cell {0}:{1} selected!", signal.CellPosition.x, signal.CellPosition.y);
		}

		public void OnZoom(float zoom)
		{
			_zoom = zoom;
			UpdateGrid();
		}

		public void OnWidthChanged(string value)
		{
			var width = Mathf.Max(int.Parse(value), 1);
			_levelSize = new Vector2Int(width, _levelSize.y);
			UpdateGrid();
		}

		public void OnHeightChanged(string value)
		{
			var height = Mathf.Max(int.Parse(value), 1);
			_levelSize = new Vector2Int(_levelSize.x, height);
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
			_gridContainer.sizeDelta = new Vector2(_levelSize.x * cellSize, _levelSize.y * cellSize) +
				Vector2.one * (padding * 2f);

			for (var y = 0; y < _levelSize.y; ++y)
			{
				for (var x = 0; x < _levelSize.x; ++x)
				{
					var cell = CreateCell(cellSize, x, y);
					cell.SetParent(_gridContainer, false);
					cell.anchoredPosition = new Vector2(x * cellSize + padding, y * cellSize + padding);
				}
			}
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
				new object[] {new Vector2Int(posX, posY)}).GetComponent<RectTransform>();
			cell.SetParent(cellContainer, false);
			cell.anchorMin = Vector2.zero;
			cell.anchorMax = Vector2.one;
			cell.offsetMin = Vector2.one * gap;
			cell.offsetMax = Vector2.one * -gap;

			return cellContainer;
		}
	}
}