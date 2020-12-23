using LevelEditor.Signals;
using Model.Environment;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace LevelEditor.Controller
{
	[DisallowMultipleComponent, RequireComponent(typeof(Toggle))]
	public class EnvironmentItemToggleController : MonoBehaviour
	{
#pragma warning disable 649
		[SerializeField] private EnvironmentItemType _type;

		[Inject] private readonly SignalBus _signalBus;
#pragma warning restore 649

		private void Start()
		{
			GetComponent<Toggle>().onValueChanged.AddListener(isOn =>
			{
				if (isOn) _signalBus.Fire(new SelectEnvironmentItemSignal {ItemType = _type});
			});
		}
	}
}