﻿using MacSalad.Core;
using MacSalad.Core.Events;
using TMPro;

namespace TalkBox.Missions
{
    public class MissionManager : SingletonMB<MissionManager>
    {
        public Mission[] Missions;
        public TMP_Text MissionDisplay;
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
            UpdateText();
        }
        public void UpdateText()
        {
            string text = "";
            for (int i = 0; i < Missions.Length; i++)
            {
                if (Missions[i].Completion != MissionState.NotStarted)
                {
                    string prefix = "";

                    if (Missions[i].Completion == MissionState.InProgress)
                    {
                        prefix += "☐ ";
                    }
                    else
                    {
                        prefix += "☑ ";
                    }

                    text += prefix + Missions[i].DisplayText + "\n";
                }
            }
            MissionDisplay.text = text;
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