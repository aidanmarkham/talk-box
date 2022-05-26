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

        public int AnimationIndex = 0;

        public Conversation Conversation => graph as Conversation;

        [Input]
        public Empty enter;

        [Output]
        public Empty exit;

        [Output(dynamicPortList = true)]
        public string[] dialogueOptions;

        public bool HasDialogueOptions => dialogueOptions.Length > 0;

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

        public void NextDialogue(string option = null)
        {
            // get a reference to the graph
            Conversation conversation = graph as Conversation;

            // Make sure we're actually the current node
            if (conversation.current != this)
            {
                Debug.LogWarning("Node isn't active");
            }
            
            DialogueNode nextDialogue = null;

            // Get exit port
            NodePort exitPort = null;

            // if an argument action was passed
            if(option != null)
			{
                // loop through the dialogue options to find the one that matches 
                for (int i = 0; i < dialogueOptions.Length; i++)
                {
                    if (dialogueOptions[i] == option)
                    {
                        // then grab the port that matches
                        exitPort = GetOutputPort("dialogueOptions " + i);
                    }
                }

                // confirm we actually have an exit port
                if (exitPort == null)
                {
                    Debug.LogError("Couldn't find dialogue option.");
                    return;
                }
            }
            // otherwise
            else
            {
                // use the exit port
                exitPort = GetOutputPort("exit");
            }

            Conversation.CallActionsOnPort(exitPort);

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

                if(entryNode)
				{
                    if (nextDialogue != null)
                    {
                        Debug.LogWarning("Multiple dialogues connected to this exit.");
                    }
                    nextDialogue = entryNode.GetDestinationNode();

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