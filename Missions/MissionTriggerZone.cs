using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using MacSalad.Core.Events;
using UnityEngine.Events;

namespace TalkBox.Missions
{
    public class MissionTriggerZone : MonoBehaviour
    {

        public Mission Mission;
        public MissionManager.MissionState StateToSet;

        [Tag]
        public string Tag;

        public UnityEvent OnTrigger;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tag))
            {
                EventDispatcher.Dispatch(MissionEvent.Prepare(Mission, StateToSet));
                OnTrigger.Invoke();
                gameObject.SetActive(false);
            }
        }
    }
}
