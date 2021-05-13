using System.Collections;
using System.Collections.Generic;
using TalkBox.Nodes;
using UnityEngine;

namespace TalkBox.Missions
{
    public class MissionLogic : LogicNode
    {
        public Mission Mission;
        public MissionManager.MissionState State;

        public override bool Logic()
        {
            return MissionManager.Instance.GetMissionState(Mission) == State;
        }


    }
}