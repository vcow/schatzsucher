using UnityEngine;

namespace LevelEditor.UI
{
	[DisallowMultipleComponent]
	public class UiSelectorController : MonoBehaviour
	{
#pragma warning disable 649
		[SerializeField] private Canvas _surfacesUi;
		[SerializeField] private Canvas _trapsUi;
		[SerializeField] private Canvas _staffUi;
#pragma warning restore 649

		public void OnSelectSurfaces(bool isOn)
		{
			if (!isOn) return;
			
			_surfacesUi.gameObject.SetActive(true);
			_trapsUi.gameObject.SetActive(false);
			_staffUi.gameObject.SetActive(false);
		}

		public void OnSelectTraps(bool isOn)
		{
			if (!isOn) return;
			
			_surfacesUi.gameObject.SetActive(false);
			_trapsUi.gameObject.SetActive(true);
			_staffUi.gameObject.SetActive(false);
		}

		public void OnSelectStaff(bool isOn)
		{
			if (!isOn) return;
			
			_surfacesUi.gameObject.SetActive(false);
			_trapsUi.gameObject.SetActive(false);
			_staffUi.gameObject.SetActive(true);
		}
	}
}