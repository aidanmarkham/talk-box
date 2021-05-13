using MacSalad.Core.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TalkBox.Missions
{
    public class MissionEvent : MSEvent
    {
        public static MissionEvent Prepare(
            Mission m,
            MissionManager.MissionState s
            )
        {
            var e = new MissionEvent();

            e.Mission = m;
            e.State = s;

            return e;
        }

        public Mission Mission;
        public MissionManager.MissionState State;
    }

}