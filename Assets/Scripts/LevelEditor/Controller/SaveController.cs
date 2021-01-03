using LevelEditor.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace LevelEditor.Controller
{
	[DisallowMultipleComponent]
	public class SaveController : MonoBehaviour
	{
#pragma warning disable 649
		[SerializeField] private InputField _name;

		[Inject] private readonly SignalBus _signalBus;
#pragma warning restore 649

		public void OnSaveLevel()
		{
			if (string.IsNullOrEmpty(_name.text)) return;
			_signalBus.Fire(new SaveLevelSignal{LevelName = _name.text});
			gameObject.SetActive(false);
		}
	}
}