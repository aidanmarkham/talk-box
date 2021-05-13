using MacSalad.Core.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using TalkBox.Nodes;
using UnityEngine;

using static TalkBox.Core.Events;

namespace TalkBox.Utility
{
    public class ConversationBillboard : ProximityBillboard
    {
        public Conversation conversation;

        protected override void SafeInitialize()
        {
            base.SafeInitialize();
        }

        protected override void AddEventListeners()
        {
            base.AddEventListeners();
            EventDispatcher.AddListener<ConversationEvent>(Conversation_Event);
        }

        protected override void RemoveEventListeners()
        {
            base.RemoveEventListeners();
            EventDispatcher.RemoveListener<ConversationEvent>(Conversation_Event);
        }

        private void Conversation_Event(ConversationEvent e)
        {
            if (!e.Started && e.Conversation == conversation)
            {
                // enable if our conversation is over
                Enable();
            }

            if (e.Started && e.Conversation == conversation)
            {
                // disable to prevent multiple interactions
                Disable();
            }
        }

        public override void OnInteraction()
        {
            EventDispatcher.Dispatch(ConversationEvent.Prepare(conversation, true));

            base.OnInteraction();
        }


    }
}