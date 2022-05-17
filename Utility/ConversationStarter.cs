using MacSalad.Core.Events;
using System.Collections;
using System.Collections.Generic;
using TalkBox.Core;
using TalkBox.Nodes;
using UnityEngine;
using static TalkBox.Core.Events;

namespace TalkBox.Utility
{
    public class ConversationStarter : MonoBehaviour
    {
        public Conversation Conversation;

        public void StartConversation()
        {
            // EventDispatcher.Dispatch(ConversationEvent.Prepare(Conversation, true));
            DialogueManager.Instance.StartConversation(Conversation);
        }
    }
}