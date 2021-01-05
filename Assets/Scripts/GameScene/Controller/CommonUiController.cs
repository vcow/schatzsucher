using System;
using Model.Environment;
using Model.Game;
using UnityEngine;
using Zenject;

namespace GameScene.Controller
{
	/// <summary>
	/// Контроллер главного UI игровой сцены.
	/// </summary>
	[DisallowMultipleComponent]
	public class CommonUiController : MonoBehaviour
	{
		private string _backSceneId;

#pragma warning disable 649
		[Inject] private readonly ZenjectSceneLoader _sceneLoader;
		[Inject] private readonly IEnvironment _environment;
		[Inject] private readonly IGameResult _gameResult;
#pragma warning restore 649

		[Inject]
		private void Construct([InjectOptional] string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				throw new Exception("GameScene must inject the back scene Id as string.");
			}

			_backSceneId = id;
		}

		public void OnBack()
		{
			switch (_backSceneId)
			{
				case Const.EditorSceneID:
					var environmentModel = (EnvironmentModel) _environment;
					_sceneLoader.LoadSceneAsync(Const.EditorSceneID, extraBindings: container =>
						container.Bind(typeof(EnvironmentModel), typeof(IEnvironment))
							.FromInstance(environmentModel).AsSingle());
					break;
				default:
					_sceneLoader.LoadSceneAsync(_backSceneId, extraBindings: container =>
						container.Bind<IGameResult>().FromInstance(_gameResult).AsCached());
					break;
			}
		}
	}
}