using System.Collections.Generic;
using UnityEngine;

namespace Model.Environment
{
	public class EnvironmentModel : IEnvironment
	{
		public Vector2Int Size;
		public readonly List<IEnvironmentItem> Items = new List<IEnvironmentItem>();

		Vector2Int IEnvironment.Size => Size;

		IReadOnlyList<IEnvironmentItem> IEnvironment.Items => Items;
	}
}