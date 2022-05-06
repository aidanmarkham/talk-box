using Cinemachine;
using MacSalad.Core;
using MacSalad.Core.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace TalkBox.Core
{
	public class DialogueManager : SingletonMB<DialogueManager>
	{
		public CharacterData ProtagonistData;
		public Character Protagonist => GetCharacter(ProtagonistData);

		private List<Character> Characters = new List<Character>();

		public bool ManageCursor = true;

		public enum GameState { Menu, Gameplay, Dialogue, Cutscene, Startup, Gameplay_Cinematic }

		public GameState State = GameState.Gameplay;

		public GameObject CharacterPrefab;

		private Keyboard keyboard;
		private Mouse mouse;

		protected override void SafeInitialize()
		{
			base.SafeInitialize();

			keyboard = Keyboard.current;
			mouse = Mouse.current;

			Characters.Clear();
			Characters.AddRange(FindObjectsOfType<Character>());
		}

		protected override void SetInstance()
		{
			SetInstance(
				dontDestroyOnLoad: false, // disable DDOL
				destroyGameObject: true // destroy entire object if more than 1
			);
		}

		private void EnterState()
		{
			switch (State)
			{
				case GameState.Startup:
					break;
				case GameState.Menu:
					if (ManageCursor)
					{
						Cursor.lockState = CursorLockMode.Confined;
						Cursor.visible = true;
					}
					break;
				case GameState.Gameplay:
					if (ManageCursor)
					{
						Cursor.lockState = CursorLockMode.Locked;
						Cursor.visible = false;
					}
					break;
				case GameState.Dialogue:
					if (ManageCursor)
					{
						Cursor.lockState = CursorLockMode.Locked;
						Cursor.visible = false;
					}
					break;
				case GameState.Cutscene:
					if (ManageCursor)
					{
						Cursor.lockState = CursorLockMode.Locked;
						Cursor.visible = false;
					}
					break;
				default:
					break;
			}
		}

		private void Update()
		{
			switch (State)
			{
				case GameState.Startup:
					SetState(GameState.Menu);
					break;
				case GameState.Menu:
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;
					break;
				case GameState.Gameplay:
					if (ManageCursor)
					{
						// Cursor Locking 
						if (keyboard.escapeKey.wasPressedThisFrame)
						{
							Cursor.lockState = CursorLockMode.None;
							Cursor.visible = true;
						}
						if (mouse.leftButton.wasPressedThisFrame)
						{
							Cursor.lockState = CursorLockMode.Locked;
							Cursor.visible = false;
						}
					}
					break;
				case GameState.Dialogue:
					break;
				case GameState.Cutscene:
					break;
				default:
					break;
			}
		}

		private void ExitState()
		{
			switch (State)
			{
				case GameState.Startup:
					break;
				case GameState.Menu:
					break;
				case GameState.Gameplay:
					break;
				case GameState.Dialogue:
					break;
				case GameState.Cutscene:
					break;
				default:
					break;
			}
		}

		public void SetState(GameState state)
		{
			ExitState();
			var lastState = State;
			State = state;

			EventDispatcher.Dispatch(GameStateChanged.Prepare(State, lastState));

			EnterState();
		}

		public void SetState(string state)
		{
			GameState to;
			if (Enum.TryParse(state, out to))
			{
				SetState(to);
			}
			else
			{
				Debug.LogError("Tried to go to bad state!");
			}
		}

		public class GameStateChanged : MSEvent
		{
			public static GameStateChanged Prepare(
				GameState state,
				GameState lastState
				)
			{
				var e = new GameStateChanged();
				e.State = state;
				e.LastState = lastState;
				return e;
			}
			public GameState State;
			public GameState LastState;
		}
		public void ReloadScene()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		public Character GetCharacter(CharacterData data)
		{
			Characters.Clear();
			Characters.AddRange(FindObjectsOfType<Character>());

			for (int i = 0; i < Characters.Count; i++)
			{
				if (Characters[i].CharacterData.ID == data.ID) return Characters[i];
			}

			Debug.LogError("Character doesn't exist!");
			return null;
		}
	}
}