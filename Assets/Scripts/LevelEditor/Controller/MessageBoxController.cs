using System;
using LevelEditor.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace LevelEditor.Controller
{
	[DisallowMultipleComponent]
	public class MessageBoxController : MonoBehaviour
	{
		private MessageBoxCallback _callback;

#pragma warning disable 649
		[SerializeField] private Text _message;
		[SerializeField] private Button _okButon;
		[SerializeField] private Button _cancelButon;
		[SerializeField] private Button _yesButon;
		[SerializeField] private Button _noButon;

		[Inject] private SignalBus _signalBus;
#pragma warning restore 649

		private void Start()
		{
			_signalBus.Subscribe<ShowMessageSignal>(OnShowMessage);
			gameObject.SetActive(false);
		}

		private void OnDestroy()
		{
			_signalBus.Unsubscribe<ShowMessageSignal>(OnShowMessage);
		}

		private void OnShowMessage(ShowMessageSignal signal)
		{
			_message.text = signal.Message;
			_okButon.gameObject.SetActive(signal.Buttons == MessageBoxButton.None ||
			                              (signal.Buttons & MessageBoxButton.Ok) != 0);
			_cancelButon.gameObject.SetActive((signal.Buttons & MessageBoxButton.Cancel) != 0);
			_yesButon.gameObject.SetActive((signal.Buttons & MessageBoxButton.Yes) != 0);
			_noButon.gameObject.SetActive((signal.Buttons & MessageBoxButton.No) != 0);
			
			gameObject.SetActive(true);
		}

		public void OnButtonClick(int result)
		{
			_callback?.Invoke((MessageBoxButton) result);
			gameObject.SetActive(false);
		}
	}
}