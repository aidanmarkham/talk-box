using TalkBox.Nodes;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace TalkBox.Editor
{

	[CustomNodeEditor(typeof(DialogueNode))]
	public class DialogueNodeEditor : NodeEditor
	{
		private DialogueNode dialogueNode;

		public override void OnBodyGUI()
		{
			if (dialogueNode == null) dialogueNode = target as DialogueNode;
			Conversation conversation = dialogueNode.graph as Conversation;

			GUIStyle s = new GUIStyle(EditorStyles.label);

			if (dialogueNode == conversation.Entry)
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
					conversation.Entry = dialogueNode;
				}
			}
			else if (dialogueNode != conversation.Entry)
			{
				if (GUILayout.Button("Make Entry"))
				{
					conversation.Entry = dialogueNode;
				}
			}

			base.OnBodyGUI();

		}
	}
}