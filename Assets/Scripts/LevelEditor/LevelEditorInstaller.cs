using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Common;
using GameScene.Input;
using LevelEditor.Signals;
using Model.Character;
using Model.Environment;
using SimpleFileBrowser;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace LevelEditor
{
	/// <summary>
	/// Контроллер сцены редактора уровней.
	/// </summary>
	public class LevelEditorInstaller : MonoInstaller<LevelEditorInstaller>
	{
		private Coroutine _coroutine;

#pragma warning disable 649
		[Inject] private readonly SignalBus _signalBus;
		[Inject] private readonly ZenjectSceneLoader _sceneLoader;
		[InjectOptional] private EnvironmentModel _environmentModel;
#pragma warning restore 649

		public override void InstallBindings()
		{
			if (Container.HasBinding(typeof(EnvironmentModel)))
			{
				// Сцена была перегружена с инъкцией модели сцены (загрузка из файла).
				return;
			}

			// Создать исходную модель сцены.
			_environmentModel = new EnvironmentModel
			{
				Size = new Vector2Int(32, 22),
				Items = new List<EnvironmentItem>()
			};

			Container.BindInterfacesAndSelfTo<EnvironmentModel>().FromInstance(_environmentModel).AsSingle();
		}

		private void Awake()
		{
			_signalBus.DeclareSignal<GridCellSelectSignal>();
			_signalBus.DeclareSignal<SelectEnvironmentItemSignal>();
			_signalBus.DeclareSignal<ShowMessageSignal>();
		}

		public void OnSaveLevel()
		{
			// Сохранить текущую сцену в файл.
			Assert.IsNull(_coroutine);
			FileBrowser.SetFilters(true, ".bytes");
			FileBrowser.SetDefaultFilter(".bytes");
			_coroutine = StartCoroutine(ShowSaveDialogCoroutine());
		}

		private IEnumerator ShowSaveDialogCoroutine()
		{
			yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.FilesAndFolders,
				initialFilename: "level");
			if (FileBrowser.Success)
			{
				var path = FileBrowser.Result[0];
				var raw = Serializer.Serialize(_environmentModel);
				if (File.Exists(path)) File.Delete(path);
				File.WriteAllText(path, raw, Encoding.UTF8);
			}

			_coroutine = null;
		}

		public void OnLoadLevel()
		{
			// Загрузить сцену из файла.
			Assert.IsNull(_coroutine);
			FileBrowser.SetFilters(true, ".bytes");
			FileBrowser.SetDefaultFilter(".bytes");
			_coroutine = StartCoroutine(ShowLoadDialogCoroutine());
		}

		private IEnumerator ShowLoadDialogCoroutine()
		{
			yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders);
			if (FileBrowser.Success)
			{
				var path = FileBrowser.Result[0];
				if (File.Exists(path))
				{
					// Перегрузить сцену редактора уровней с инъекцией модели загруженного уровня.
					var raw = File.ReadAllText(path, Encoding.UTF8);
					var environmentModel = Serializer.Deserialize<EnvironmentModel>(raw);
					_sceneLoader.LoadSceneAsync("EditorScene",
						extraBindings: container => container.BindInterfacesAndSelfTo<EnvironmentModel>()
								.FromInstance(environmentModel).AsSingle());
				}
			}

			_coroutine = null;
		}

		public void OnPlay()
		{
			// Играть редактируемый уровень.
			const string id = "test_player";
			var input = new LocalInput();
			var player = new PlayerCharacter(input, id);
			_sceneLoader.LoadSceneAsync(Const.GameSceneID, extraBindings: container =>
			{
				container.Bind<string>().FromInstance("EditorScene").AsSingle();
				container.Bind<IEnvironment>().FromInstance(_environmentModel).AsSingle();
				container.BindInterfacesAndSelfTo<PlayerCharacter>().FromInstance(player).AsCached();
			});
		}

		private void OnDestroy()
		{
			if (_coroutine != null) StopCoroutine(_coroutine);
		}

		public void OnCLoseEditor()
		{
			// Нажата кнопка выхода из редактора (для standalone сборок редактора).
			_signalBus.Fire(new ShowMessageSignal
			{
				Message = "Are you really want to exit the Editor?",
				Buttons = MessageBoxButton.Yes | MessageBoxButton.No,
				Callback = result =>
				{
					if (result == MessageBoxButton.Yes)
					{
						Application.Quit();
					}
				}
			});
		}
	}
}