using System.Collections.Generic;
using GameScene.Game;
using UnityEngine;
using Zenject;

namespace GameScene.Controller.Utility
{
	/// <summary>
	/// Вспомогательный компонент, определяющий отношение объекта к
	/// элементу сцены "веревка".
	/// </summary>
	[DisallowMultipleComponent]
	public class RopeAttitude : BoundedItemAttitude<IRope>
	{
#pragma warning disable 649
		[Inject] private readonly IReadOnlyList<IRope> _rops;
#pragma warning restore 649

		protected override IReadOnlyList<IRope> Items => _rops;
	}
}