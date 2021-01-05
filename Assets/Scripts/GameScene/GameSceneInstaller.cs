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
			var gameResult = new GameResult();
			Container.Bind(typeof(GameResult), typeof(IGameResult))
				.FromInstance(gameResult).AsSingle();
		}

		public override void Start()
		{
			Container.InstantiateComponentOnNewGameObject<GameSceneEnvironmentController>();
		}
	}
}