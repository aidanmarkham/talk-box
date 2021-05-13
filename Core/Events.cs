using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MacSalad.Core.Events;
using TalkBox.Nodes;

namespace TalkBox.Core
{
    public static class Events
    {
        // Called when a conversation is started or ended
        public class ConversationEvent : MSEvent
        {
            public static ConversationEvent Prepare(
                Conversation conversation,
                bool started
                )
            {
                var e = new ConversationEvent();
                e.Conversation = conversation;
                e.Started = started;
                return e;
            }

            public Conversation Conversation { get; private set; }
            public bool Started { get; private set; }
        }

        // Called when a dialogue is started or ended
        public class DialogueEvent : MSEvent
        {
            public static DialogueEvent Prepare(
                DialogueNode source,
                bool started
            )
            {
                var e = new DialogueEvent();

                e.Source = source;
                e.Started = started;

                return e;
            }
            public DialogueNode Source { get; private set; }
            public bool Started { get; private set; }
        }

        // Called when a letter is spoken
        public class SpeakingEvent : MSEvent
        {
            public static SpeakingEvent Prepare(char letter)
            {
                var e = new SpeakingEvent();
                e.Letter = letter;
                return e;
            }

            public char Letter { get; private set; }
        }

    }
}