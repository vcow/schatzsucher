using UnityEngine;

namespace Model.Environment
{
	public interface IEnvironmentItem
	{
		EnvironmentItemType Type { get; }
		Vector2Int Position { get; }
	}
}