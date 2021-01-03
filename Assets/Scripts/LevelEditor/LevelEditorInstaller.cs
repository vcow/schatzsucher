using System;
using System.Collections.Generic;
using Common;
using Common.JsonConverter;
using LevelEditor.Signals;
using Model.Environment;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;
using Zenject;

namespace LevelEditor
{
	public class LevelEditorInstaller : MonoInstaller<LevelEditorInstaller>
	{
		private EnvironmentModel _environmentModel;

#pragma warning disable 649
		[Inject] private readonly SignalBus _signalBus;
#pragma warning restore 649

		public override void InstallBindings()
		{
			_environmentModel = new EnvironmentModel
			{
				Size = Vector2Int.one * 10,
				Items = new List<EnvironmentItem>()
			};
			Container.Bind(typeof(EnvironmentModel), typeof(IEnvironment))
				.FromInstance(_environmentModel).AsTransient();
		}

		private void Awake()
		{
			_signalBus.DeclareSignal<GridCellSelectSignal>();
			_signalBus.DeclareSignal<SelectEnvironmentItemSignal>();
			_signalBus.DeclareSignal<SaveLevelSignal>();
			_signalBus.DeclareSignal<ShowMessageSignal>();
		}

		public override void Start()
		{
			_signalBus.Subscribe<SaveLevelSignal>(OnSaveLevel);
		}

		private void OnSaveLevel(SaveLevelSignal signal)
		{
			var txt = Serializer.Serialize(_environmentModel);

			var e = Serializer.Deserialize<EnvironmentModel>(txt);
		}

		public void OnPlay()
		{
			throw new NotImplementedException();
		}

		private void OnDestroy()
		{
			_signalBus.Unsubscribe<SaveLevelSignal>(OnSaveLevel);
		}
	}
}