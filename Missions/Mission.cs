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

        

    }
}