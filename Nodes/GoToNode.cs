using System;
using UnityEngine;
using XNode;
using static TalkBox.Nodes.DialogueNode;

namespace TalkBox.Nodes
{
	public class GoToNode : Node
	{
		public string ID;
		public string Destination;

		[Input] public Empty enter;
		[Output] public Empty exit;

		public DialogueNode GetDestinationNode()
		{
			// get a refrence to the graph
			Conversation conversation = graph as Conversation;

			if (Destination == "Entry")
			{
				DialogueNode dialogueNode = conversation.Entry as DialogueNode;
				LogicNode logicNode = conversation.Entry as LogicNode;

				if (dialogueNode)
				{
					return dialogueNode;
				}
				else if (logicNode)
				{
					return logicNode.RecurseToDialogue();
				}
			}
			else
			{
				// Find the GoToNode that matches the destination
				GoToNode destination = null;
				for (int i = 0; i < conversation.nodes.Count; i++)
				{
					GoToNode node = conversation.nodes[i] as GoToNode;

					if (node != null)
					{
						if (node.ID == Destination)
						{
							destination = node;
						}
					}
				}

				DialogueNode nextDialogue = null;

				// If we found one
				if(destination != null)
				{
					NodePort exitPort = destination.GetOutputPort("exit");

					var nextConnections = exitPort.GetConnections();

					for (int i = 0; i < nextConnections.Count; i++)
					{
						var currentConnection = nextConnections[i].node;


						// otherwise, get the exit node
						DialogueNode dialogueNode = currentConnection as DialogueNode;

						LogicNode logicNode = currentConnection as LogicNode;

						GoToNode entryNode = currentConnection as GoToNode;

						if (dialogueNode)
						{
							if (nextDialogue != null)
							{
								Debug.LogWarning("Multiple dialogues connected to this exit.");
							}
							// and tell it to set itself to current
							nextDialogue = dialogueNode;
						}

						if (logicNode)
						{
							if (nextDialogue != null)
							{
								Debug.LogWarning("Multiple dialogues connected to this exit.");
							}

							nextDialogue = logicNode.RecurseToDialogue();
						}
					}
				}

				return nextDialogue;
			}
			return null;
		}

	}
}