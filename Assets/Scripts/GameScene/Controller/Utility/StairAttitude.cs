using System.Collections.Generic;
using GameScene.Game;
using UnityEngine;
using Zenject;

namespace GameScene.Controller.Utility
{
	/// <summary>
	/// Вспомогательный компонент, определяющий отношение объекта к
	/// элементу сцены "лестница".
	/// </summary>
	[DisallowMultipleComponent]
	public class StairAttitude : BoundedItemAttitude<IStair>
	{
#pragma warning disable 649
		[Inject] private readonly IReadOnlyList<IStair> _stairs;
#pragma warning restore 649

		protected override IReadOnlyList<IStair> Items => _stairs;
	}
}