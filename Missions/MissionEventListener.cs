using MacSalad.Core;
using MacSalad.Core.Events;
using System.Collections;
using System.Collections.Generic;
using TalkBox.Core;
using UnityEngine;
using UnityEngine.Events;
using static TalkBox.Missions.MissionManager;

namespace TalkBox.Missions
{
    public class MissionEventListener : MSBehaviour
    {
        public Mission Mission;
        public MissionState State;
        public UnityEvent OnState;
        public bool WaitForState = false;

        public DialogueManager.GameState StateToWaitFor;

        protected override void AddEventListeners()
        {
            base.AddEventListeners();
            EventDispatcher.AddListener<MissionEvent>(Mission_Event);
        }

        protected override void RemoveEventListeners()
        {
            base.RemoveEventListeners();
            EventDispatcher.RemoveListener<MissionEvent>(Mission_Event);
        }

        private void Mission_Event(MissionEvent e)
        {
	        if (!e.Mission.Equals(Mission) || e.State != State) return;
	        
	        if (WaitForState)
	        {
		        StartCoroutine(WaitForGameState());
	        }
	        else
	        {
		        OnState.Invoke();
	        }
        }

        IEnumerator WaitForGameState()
        {
            while (DialogueManager.Instance.State != StateToWaitFor)
            {
                yield return null;
            }
            OnState.Invoke();
        }
    }
}