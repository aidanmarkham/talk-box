using System.Collections.Generic;
using TalkBox.Nodes;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace TalkBox.Editor
{

	[CustomNodeEditor(typeof(GoToNode))]
	public class GoToNodeEditor : NodeEditor
	{
		private GoToNode goToNode;
		
		public override void OnBodyGUI()
		{
			if (goToNode == null) goToNode = target as GoToNode;
			Conversation conversation = goToNode.graph as Conversation;

			serializedObject.Update();

			GUIStyle s = new GUIStyle(EditorStyles.label);
			EditorGUILayout.LabelField("Incoming:", s);


			// Destination selector
			var destinations = GetDestinations(conversation);
			var selected = 0;
			for (int i = 0; i < destinations.Length; i++)
			{
				if (destinations[i] == goToNode.Destination)
				{
					selected = i;
					break;
				}
			}
			selected = EditorGUILayout.Popup("Destination: ", selected, destinations);
			goToNode.Destination = destinations[selected];

			NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("enter"));

			EditorGUILayout.Space();
			EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1f), Color.black);
			EditorGUILayout.Space();

			EditorGUILayout.LabelField("Outgoing:", s);
			NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("ID"));
			NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("exit"));


			serializedObject.ApplyModifiedProperties();
		}

		private string[] GetDestinations(Conversation c)
		{
			var destinations = new List<string>();

			destinations.Add("Entry");

			for(int i = 0; i < c.nodes.Count; i++)
			{
				GoToNode node = c.nodes[i] as GoToNode;

				if(node != null)
				{
					if (!string.IsNullOrEmpty(node.ID))
					{
						destinations.Add(node.ID);
					}
				}
			}
			return destinations.ToArray();
		}
	}
}