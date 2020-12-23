using System.Collections.Generic;
using UnityEngine;

namespace Model.Environment
{
	public interface IEnvironment
	{
		Vector2Int Size { get; }
		IReadOnlyList<IEnvironmentItem> Items { get; }
	}
}