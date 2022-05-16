using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TalkBox.Missions
{
    [CreateAssetMenu(fileName = "New Mission", menuName = "TalkBox/Mission", order = 1)]
    public class Mission : ScriptableObject
    {
        public Mission MissionTag;
        public string DisplayText;

        public string ID = Guid.NewGuid().ToString();

        public MissionManager.MissionState Completion = MissionManager.MissionState.NotStarted;
        
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