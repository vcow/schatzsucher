using System.Collections.Generic;
using GameScene.Game;
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
		}

		public override void Start()
		{
			Container.InstantiateComponentOnNewGameObject<GameSceneEnvironmentController>();
			Container.Resolve<IGame>().Start();
		}
	}
}