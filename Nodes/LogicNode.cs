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
			LogicNode logicNode = this;
			int maxLoops = 100;
			int counter = 0;

			while (nextDialogue == null)
			{
				counter++;
				if (counter > maxLoops)
				{
					break;
				}

				var nextNode = logicNode.Evaluate();

				DialogueNode d = nextNode as DialogueNode;
				LogicNode l = nextNode as LogicNode;
				ActionNode a = nextNode as ActionNode;

				if (d)
				{
					nextDialogue = d;
					break;
				}

				if (l)
				{
					logicNode = l;
				}

				if (nextNode == null)
				{
					conversation.current = null;
					break;
				}

			}

			return nextDialogue;
		}

		public abstract bool Logic();
	}
}