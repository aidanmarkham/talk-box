using TalkBox.Nodes;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace TalkBox.Editor
{
	[CustomNodeEditor(typeof(LogicNode))]
	public class LogicNodeEditor : NodeEditor
	{
		private LogicNode logicNode;

		public override void OnBodyGUI()
		{
			if (logicNode == null) logicNode = target as LogicNode;
			Conversation conversation = logicNode.graph as Conversation;

			GUIStyle s = new GUIStyle(EditorStyles.label);

			if (logicNode == conversation.Entry)
			{
				s.normal.textColor = Color.green;
				UnityEditor.EditorGUILayout.LabelField("Entry", s);
			}
			else if (conversation.Entry == null)
			{
				s.normal.textColor = Color.red;
				UnityEditor.EditorGUILayout.LabelField("No Entry Set", s);
				if (GUILayout.Button("Make Entry"))
				{
					conversation.Entry = logicNode;
				}
			}
			else if (logicNode != conversation.Entry)
			{
				if (GUILayout.Button("Make Entry"))
				{
					conversation.Entry = logicNode;
				}
			}

			base.OnBodyGUI();
		}
	}
}