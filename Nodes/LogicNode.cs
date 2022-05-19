using System;
using UnityEngine;
using XNode;
using static TalkBox.Nodes.DialogueNode;

namespace TalkBox.Nodes
{
	public abstract class LogicNode : Node
	{
		[Input] public Empty enter;

		[Output] public Empty True;
		[Output] public Empty False;

		protected override void Init()
		{
			base.Init();
		}

		public virtual Node Evaluate()
		{
			bool logicResult = Logic();

			NodePort exit;

			if (logicResult == true)
			{
				exit = GetOutputPort("True");
			}
			else
			{
				exit = GetOutputPort("False");
			}

			Conversation.CallActionsOnPort(exit);

			if (exit.Connection.node == null)
			{
				return null;
			}
			return exit.Connection.node;
		}

		public DialogueNode RecurseToDialogue()
		{
			// get a refrence to the graph
			Conversation conversation = graph as Conversation;

			DialogueNode nextDialogue = null;
			Node node = this;
			int maxLoops = 100;
			int counter = 0;

			while (node && nextDialogue == null)
			{
				counter++;
				if (counter > maxLoops)
				{
					break;
				}

				switch (node)
				{
					case DialogueNode d:
						nextDialogue = d;
						break;
					case LogicNode l:
						node = l.Evaluate();
						break;
					case GoToNode goTo:
						node = goTo.GetDestinationNode();
						break;
					case ActionNode a:
						conversation.current = null;
						node = null;
						break;
				}
			}

			return nextDialogue;
		}

		public abstract bool Logic();
	}
}