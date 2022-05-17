using UnityEngine;

namespace TalkBox.Missions
{
	/// <summary>
	/// Used to create tags that can be shared between missions (i.e. "Hidden")
	/// </summary>
    [CreateAssetMenu(fileName = "New Mission Tag", menuName = "TalkBox/Mission Tag", order = 1)]
	public class MissionTag : ScriptableObject
	{
	}
}