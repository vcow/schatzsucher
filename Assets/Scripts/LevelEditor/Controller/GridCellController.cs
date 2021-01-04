using System.Linq;
using LevelEditor.Signals;
using Model.Environment;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace LevelEditor.Controller
{
	/// <summary>
	/// Контроллер ячейки разметочной сетки редактора уровней.
	/// </summary>
	[DisallowMultipleComponent, RequireComponent(typeof(RawImage))]
	public class GridCellController : MonoBehaviour, IPointerClickHandler
	{
		private static readonly Color CellColor = new Color(1f, 1f, 1f, 0.05f);

		private Vector2Int _cellPosition;
		private IEnvironment _environment;

#pragma warning disable 649
		[Inject] private readonly SignalBus _signalBus;
#pragma warning restore 649

		[Inject]
		private void Construct(Vector2Int cellPosition, IEnvironment environment)
		{
			_cellPosition = cellPosition;
			_environment = environment;
		}

		private void Start()
		{
			UpdateView();
		}

		private void UpdateView()
		{
			GetComponent<RawImage>().color = _environment.Items.Any(item => item.Position == _cellPosition)
				? Color.clear
				: CellColor;
		}

		void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			_signalBus.Fire(new GridCellSelectSignal {CellPosition = _cellPosition});
			UpdateView();
		}
	}
}