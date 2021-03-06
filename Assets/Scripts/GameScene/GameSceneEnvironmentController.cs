using Common.Controller;
using GameScene.Controller;
using Model.Environment;

namespace GameScene
{
	public class GameSceneEnvironmentController : EnvironmentController
	{
		protected override void ProcessITemView(EnvironmentItemController itemView)
		{
			switch (itemView.ItemModel.Type)
			{
				case EnvironmentItemType.Enter:
					Container.InstantiateComponent<EnterController>(itemView.gameObject);
					break;
				case EnvironmentItemType.Stair:
					Container.InstantiateComponent<StairController>(itemView.gameObject);
					break;
				case EnvironmentItemType.Rope:
					Container.InstantiateComponent<RopeController>(itemView.gameObject);
					break;
			}
		}
	}
}