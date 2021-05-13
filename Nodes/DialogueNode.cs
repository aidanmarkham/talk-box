using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using NaughtyAttributes;
using System;
using TalkBox.Core;

namespace TalkBox.Nodes
{
    public class DialogueNode : Node
    {
        public CharacterData Character;

        [TextArea]
        public string Text;

        public bool WaitForInput = true;

        [Input]
        public Empty enter;

        [Output]
        public Empty exit;

        // Use this for initialization
        protected override void Init()
        {
            base.Init();

        }

        // Return the correct value of an output port when requested
        public override object GetValue(NodePort port)
        {
            if (port.IsConnected)
            {
                return port.Connection.node;
            }
            else
            {
                return null;
            }
        }
        public void OnEnter()
        {
            // Get a reference to teh graph
            Conversation conversation = graph as Conversation;

            // Set ourself to current
            conversation.current = this;
        }

        public void NextDialogue()
        {
            // get a reference to the graph
            Conversation conversation = graph as Conversation;

            // Make sure we're actually the current node
            if (conversation.current != this)
            {
                Debug.LogWarning("Node isn't active");
            }

            // Get exit port
            NodePort exitPort = GetOutputPort("exit");

            DialogueNode nextDialogue = null;

            Conversation.CallActionsOnPort(exitPort);

            var nextConnections = exitPort.GetConnections();

            for (int i = 0; i < nextConnections.Count; i++)
            {
                var currentConnection = nextConnections[i].node;


                // otherwise, get the exit node
                DialogueNode dialogueNode = currentConnection as DialogueNode;

                LogicNode logicNode = currentConnection as LogicNode;

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
            if (nextDialogue)
            {
                nextDialogue.OnEnter();
            }
            else
            {
                Debug.LogWarning("Node isn't connected to another dialogue");

                // if it's not, set current to null so we know we've reached an endpoint
                conversation.current = null;
            }
        }

        [Serializable]
        public class Empty { }
    }
}