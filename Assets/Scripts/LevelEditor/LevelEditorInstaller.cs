using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Common;
using LevelEditor.Signals;
using Model.Environment;
using SimpleFileBrowser;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace LevelEditor
{
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
			if (Container.HasBinding(typeof(EnvironmentModel))) return;

			_environmentModel = new EnvironmentModel
			{
				Size = Vector2Int.one * 10,
				Items = new List<EnvironmentItem>()
			};

			Container.Bind(typeof(EnvironmentModel), typeof(IEnvironment))
				.FromInstance(_environmentModel).AsSingle();
		}

		private void Awake()
		{
			_signalBus.DeclareSignal<GridCellSelectSignal>();
			_signalBus.DeclareSignal<SelectEnvironmentItemSignal>();
			_signalBus.DeclareSignal<ShowMessageSignal>();
		}

		public void OnSaveLevel()
		{
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
					var raw = File.ReadAllText(path, Encoding.UTF8);
					var environmentModel = Serializer.Deserialize<EnvironmentModel>(raw);
					_sceneLoader.LoadSceneAsync("EditorScene",
						extraBindings: container =>
							container.Bind(typeof(EnvironmentModel), typeof(IEnvironment))
								.FromInstance(environmentModel).AsSingle());
				}
			}

			_coroutine = null;
		}

		public void OnPlay()
		{
			throw new NotImplementedException();
		}

		private void OnDestroy()
		{
			if (_coroutine != null) StopCoroutine(_coroutine);
		}

		public void OnCLoseEditor()
		{
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