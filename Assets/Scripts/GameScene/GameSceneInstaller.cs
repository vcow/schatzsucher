using System.Collections.Generic;
using GameScene.Game;
using GameScene.Signals;
using Model.Game;
using Zenject;

namespace GameScene
{
	/// <summary>
	/// Контроллер игровой сцены.
	/// </summary>
	public class GameSceneInstaller : MonoInstaller<GameSceneInstaller>
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesTo<Game.Game>().AsSingle();

			var gameResult = new GameResult();
			Container.BindInterfacesAndSelfTo<GameResult>().FromInstance(gameResult).AsSingle();

			Container.Bind<List<IEnter>>().AsSingle();
			Container.Bind<IReadOnlyList<IEnter>>().To<List<IEnter>>().FromResolve();

			Container.Bind<List<IStair>>().AsSingle();
			Container.Bind<IReadOnlyList<IStair>>().To<List<IStair>>().FromResolve();

			Container.Bind<List<ICharacter>>().AsSingle();
			Container.Bind<IReadOnlyList<ICharacter>>().To<List<ICharacter>>().FromResolve();
			
			Container.DeclareSignal<EnterToStairSignal>();
			Container.DeclareSignal<ExitFromStairSignal>();
		}

		public override void Start()
		{
			Container.InstantiateComponentOnNewGameObject<GameSceneEnvironmentController>();
			Container.Resolve<IGame>().Start();
		}
	}
}