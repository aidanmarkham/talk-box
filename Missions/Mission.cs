using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TalkBox.Missions
{
    [CreateAssetMenu(fileName = "New Mission", menuName = "Data/Mission", order = 1)]
    public class Mission : ScriptableObject
    {
        public string DisplayText;

        public string ID = Guid.NewGuid().ToString();

        public MissionManager.MissionState Completion = MissionManager.MissionState.NotStarted;

        [Tooltip("A mission to be set as in progress after this mission is complete")]
        public Mission FollowingMission = null;

        public override bool Equals(object other)
        {
	        // compare ID for missions so runtime instances are equal to originals
	        if (other is Mission mission) return mission.ID.Equals(ID);
	        
	        return base.Equals(other);
        }
    }
}