using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using static TalkBox.Nodes.DialogueNode;


namespace TalkBox.Nodes
{
	public abstract class ActionNode : Node
	{

		[Input] public Empty enter;
		// Use this for initialization
		protected override void Init()
		{
			base.Init();

		}

		// Return the correct value of an output port when requested
		public override object GetValue(NodePort port)
		{
			return null; // Replace this
		}

		public abstract void Action();
	}
}