using LevelEditor.Signals;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace LevelEditor.Controller
{
	[DisallowMultipleComponent, RequireComponent(typeof(RawImage))]
	public class GridCellController : MonoBehaviour, IPointerClickHandler
	{
		private static readonly Color CellColor = new Color(1f, 1f, 1f, 0.3f);

		private Vector2Int _cellPosition;

#pragma warning disable 649
		[Inject] private readonly SignalBus _signalBus;
#pragma warning restore 649

		[Inject]
		private void Construct(Vector2Int cellPosition)
		{
			_cellPosition = cellPosition;
		}

		private void Start()
		{
			GetComponent<RawImage>().color = CellColor;
		}

		void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			_signalBus.Fire(new GridCellSelectSignal {CellPosition = _cellPosition});
		}
	}
}