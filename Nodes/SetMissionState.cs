using MacSalad.Core.Events;
using System.Collections;
using System.Collections.Generic;
using TalkBox.Missions;
using TalkBox.Nodes;
using UnityEngine;
using static TalkBox.Missions.MissionManager;

public class SetMissionState : ActionNode
{
    public Mission Mission;
    public MissionState StateToSet;

    public override void Action()
    {
        MissionManager.Instance.AddMission(Mission);

        EventDispatcher.Dispatch(MissionEvent.Prepare(Mission, StateToSet));
    }

}
