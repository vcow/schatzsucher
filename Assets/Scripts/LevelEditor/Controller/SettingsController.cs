using Model.Environment;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace LevelEditor.Controller
{
	/// <summary>
	/// Контроллер окна настроек.
	/// </summary>
	[DisallowMultipleComponent]
	public class SettingsController : MonoBehaviour
	{
#pragma warning disable 649
		[SerializeField] private InputField _width;
		[SerializeField] private InputField _height;

		[Inject] private readonly IEnvironment _model;
#pragma warning restore 649

		private void OnEnable()
		{
			_width.text = _model.Size.x.ToString();
			_height.text = _model.Size.y.ToString();
		}
	}
}