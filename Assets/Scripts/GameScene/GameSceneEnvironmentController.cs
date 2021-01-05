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
			}
		}
	}
}