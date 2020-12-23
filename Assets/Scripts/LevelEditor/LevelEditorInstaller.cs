using LevelEditor.Signals;
using Zenject;

namespace LevelEditor
{
	public class LevelEditorInstaller : MonoInstaller<LevelEditorInstaller>
	{
#pragma warning disable 649
		[Inject] private readonly SignalBus _signalBus;
#pragma warning restore 649

		public override void InstallBindings()
		{
		}

		public override void Start()
		{
			_signalBus.DeclareSignal<GridCellSelectSignal>();
			_signalBus.DeclareSignal<SelectEnvironmentItemSignal>();
		}
	}
}