using System.Collections;
using System.Collections.Generic;
using TalkBox.Core;
using UnityEngine;
using XNode;

namespace TalkBox.Nodes
{

	[CreateAssetMenu(fileName = "Conversation", menuName = "TalkBox/Conversation", order = 1)]
	public class Conversation : NodeGraph
	{

		public Node Entry;

		public DialogueNode current;

		private List<string> participantIDs;
		private List<CharacterData> participants;

		public void Init()
		{
			DialogueNode dialogueNode = Entry as DialogueNode;
			LogicNode logicNode = Entry as LogicNode;

			if (dialogueNode)
			{
				current = dialogueNode;
			}
			else if (logicNode)
			{
				current = logicNode.RecurseToDialogue();
			}
		}

		// Attempts to move current to the next dialogue, and returns if we were successful or not
		public bool Next()
		{
			current.NextDialogue();

			if (current == null)
			{
				return false;
			}
			else
			{
				return true;
			}
		}


		public bool Next(string option)
		{
			current.NextDialogue(option);

			if (current == null)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
		public static void CallActionsOnPort(NodePort exitPort)
		{
			var nextConnections = exitPort.GetConnections();

			for (int i = 0; i < nextConnections.Count; i++)
			{
				var currentConnection = nextConnections[i].node;

				ActionNode actionNode = currentConnection as ActionNode;

				if (actionNode)
				{
					actionNode.Action();
				}
			}
		}

		public List<string> GetParticipantIDs()
		{

			// todo: rebuilding the list every time this is called is really expensive
			participantIDs.Clear();

			foreach (Node node in nodes)
			{
				DialogueNode dNode = node as DialogueNode;

				if (dNode != null)
				{
					if (!participantIDs.Contains(dNode.Character.ID))
					{
						participantIDs.Add(dNode.Character.ID);
					}
				}
			}


			return participantIDs;
		}

		public List<CharacterData> GetParticipants()
		{

			// todo: rebuilding the list every time this is called is really expensive
			participants.Clear();

			foreach (Node node in nodes)
			{
				DialogueNode dNode = node as DialogueNode;

				if (dNode != null)
				{
					if (!participants.Contains(dNode.Character))
					{
						participants.Add(dNode.Character);
					}
				}
			}


			return participants;
		}
	}
}