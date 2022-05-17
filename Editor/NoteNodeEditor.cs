using TalkBox.Nodes;
using UnityEngine;
using XNodeEditor;

namespace TalkBox.Editor
{
	[CustomNodeEditor(typeof(NoteNode))]
	public class NoteNodeEditor : NodeEditor
	{
		public override Color GetTint()
		{
			var node = (NoteNode)target;
			
			// force alpha value to keep things visible
			var c = node.Color;
			c.a = 0.5f;
			
			return c;
		}
	}
}