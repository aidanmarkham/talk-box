using System.Collections.Generic;
using MacSalad.Core;
using MacSalad.Core.Events;
using MacSalad.Core.Utility;
using TMPro;

namespace TalkBox.Missions
{
	public class MissionManager : SingletonMB<MissionManager>
	{
		private List<Mission> missions = new List<Mission>();
		public TMP_Text MissionDisplay;

		public IReadOnlyList<Mission> Missions => missions;

		public enum MissionState
		{
			NotStarted,
			InProgress,
			Complete
		}

		public bool HasMission(Mission mission)
		{
			mission = RuntimeInstanceManager.GetRuntimeInstance(mission);
			return missions.Contains(mission);
		}

		public void AddMission(Mission mission)
		{
			missions.Add(RuntimeInstanceManager.GetRuntimeInstance(mission));
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
			for (int i = 0; i < missions.Count; i++)
			{
				if (missions[i].ID == e.Mission.ID && missions[i].Completion != e.State)
				{
					missions[i].Completion = e.State;
					missions[i].OnStateChanged();
				}
			}
		}

		private void InstantiateMissions()
		{
			for (int i = 0; i < missions.Count; i++)
			{
				missions[i] = Instantiate(missions[i]);
			}
		}

		public MissionState GetMissionState(Mission m)
		{
			for (int i = 0; i < missions.Count; i++)
			{
				if (missions[i].ID == m.ID)
				{
					return missions[i].Completion;
				}
			}

			return MissionState.NotStarted;
		}

		public void SetMissionState(Mission m, MissionState state)
		{
			if (GetMissionState(m) == state) return;

			EventDispatcher.Dispatch(new MissionEvent { Mission = m, State = state });
		}
	}
}