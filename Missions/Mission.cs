using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace TalkBox.Missions
{
    [CreateAssetMenu(fileName = "New Mission", menuName = "TalkBox/Mission", order = 1)]
    public class Mission : ScriptableObject
    {
        public Mission MissionTag;
        
        [TextArea]
        [FormerlySerializedAs("DisplayText")]
        [SerializeField] protected string displayText;

        public virtual string DisplayText
        {
            get => displayText;
            set => displayText = value;
        }

        public string ID = Guid.NewGuid().ToString();

        public MissionManager.MissionState Completion = MissionManager.MissionState.NotStarted;

        /// <summary>
        /// Called when the mission state has changed
        /// </summary>
        public virtual void OnStateChanged()
        {
            
        }
        
        public static bool operator ==(Mission lhs, Mission rhs)
        {
            if (!lhs && !rhs) return true;
            if (!lhs || !rhs) return false;
            return lhs.ID == rhs.ID;
        }

        public static bool operator !=(Mission lhs, Mission rhs)
        {
            return !(lhs == rhs);
        }
    }
}