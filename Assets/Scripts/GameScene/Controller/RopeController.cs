using System.Collections.Generic;
using GameScene.Game;
using Zenject;

namespace GameScene.Controller
{
	/// <summary>
	/// Контроллер веревки.
	/// </summary>
	public class RopeController : BoundedItemController<IRope>, IRope
	{
#pragma warning disable 649
		[Inject] private readonly List<IRope> _rops;
#pragma warning restore 649
		protected override bool IsDynamic => false;
		protected override List<IRope> ElementsCollection => _rops;
	}
}