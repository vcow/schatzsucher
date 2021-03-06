using System.Collections.Generic;
using GameScene.Game;
using Zenject;

namespace GameScene.Controller
{
	/// <summary>
	/// Контроллер лестницы.
	/// </summary>
	public class StairController : BoundedItemController<IStair>, IStair
	{
#pragma warning disable 649
		[Inject] private readonly List<IStair> _stairs;
#pragma warning restore 649
		protected override bool IsDynamic => false;
		protected override List<IStair> ElementsCollection => _stairs;
	}
}