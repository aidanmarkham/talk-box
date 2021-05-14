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
        EventDispatcher.Dispatch(MissionEvent.Prepare(Mission, StateToSet));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
