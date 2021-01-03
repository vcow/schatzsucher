using System.Collections.Generic;
using UnityEngine;

namespace Model.Environment
{
	public class EnvironmentModel : IEnvironment
	{
		public Vector2Int Size { set; get; }
		public List<EnvironmentItem> Items { set; get; }

		// IEnvironment
		Vector2Int IEnvironment.Size => Size;

		IReadOnlyList<IEnvironmentItem> IEnvironment.Items => Items;
		// \IEnvironment
	}
}