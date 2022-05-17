using UnityEngine;
using XNode;

namespace TalkBox.Nodes
{
	public class NoteNode : Node
	{
		[TextArea] public string Text;
	}
}