using MacSalad.Core;
using MacSalad.Core.Events;
using TMPro;

namespace TalkBox.Missions
{
    public class MissionManager : SingletonMB<MissionManager>
    {
        public Mission[] Missions;
        public TMP_Text MissionDisplay;
        private string missionDisplayText;

        public enum MissionState
        {
            NotStarted,
            InProgress,
            Complete
        }

        protected override void SetInstance()
        {
            SetInstance(
                dontDestroyOnLoad: false, // disable DDOL
                destroyGameObject: true // destroy entire object if more than 1
            );
        }

        protected override void SafeInitialize()
        {
            base.SafeInitialize();

            InstantiateMissions();
        }

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
            for (int i = 0; i < Missions.Length; i++)
            {
                if (Missions[i].ID == e.Mission.ID)
                {
                    Missions[i].Completion = e.State;

                    if(Missions[i].FollowingMission != null &&
                        GetMissionState(Missions[i].FollowingMission) == MissionState.NotStarted)
					{
                        EventDispatcher.Dispatch(MissionEvent.Prepare(Missions[i].FollowingMission, MissionState.InProgress));
					}
                }
            }
        }

        private void InstantiateMissions()
        {
            for (int i = 0; i < Missions.Length; i++)
            {
                Missions[i] = Instantiate(Missions[i]);
            }
        }


        private void Update()
        {
            if(MissionDisplay) UpdateText();
        }
        public void UpdateText()
        {
            string text = "";
            for (int i = 0; i < Missions.Length; i++)
            {
                if (Missions[i].Completion != MissionState.NotStarted && Missions[i].DisplayText != "")
                {
                    string prefix = "";
                    string postfix = "";
                    if (Missions[i].Completion == MissionState.InProgress)
                    {
                        prefix += "• ";                      
                    }
                    else
                    {
                        prefix += "• <s>";
                        postfix += "</s>";
                    }

                    text += prefix + Missions[i].DisplayText + postfix + "\n";
                }
            }
            missionDisplayText = text;

            UpdateTextDisplay();
        }

        public void UpdateTextDisplay()
		{
            if(MissionDisplay.text != missionDisplayText)
			{
                MissionDisplay.text = missionDisplayText;
                MissionDisplay.ForceMeshUpdate();
			}
		}

        public MissionState GetMissionState(Mission m)
        {
            for (int i = 0; i < Missions.Length; i++)
            {
                if (Missions[i].ID == m.ID)
                {
                    return Missions[i].Completion;
                }
            }

            return MissionState.NotStarted;
        }

    }
}