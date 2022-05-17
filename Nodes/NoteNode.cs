using UnityEngine;
using XNode;

namespace TalkBox.Nodes
{
	public class NoteNode : Node
	{
		public Color Color = Color.yellow;
		[TextArea] public string Text;
	}
}