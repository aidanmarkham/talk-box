using MacSalad.Core;
using MacSalad.Core.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using TalkBox.Nodes;
using UnityEngine;
using UnityEngine.SceneManagement;
using static TalkBox.Core.Events;

namespace TalkBox.Core
{
    public class DialogueManager : SingletonMB<DialogueManager>
    {
        public CharacterData ProtagonistData;
        public Character Protagonist => GetCharacter(ProtagonistData);

        public List<Character> Characters = new List<Character>();

        public bool ManageCursor = true;

        public enum GameState { Menu, Gameplay, Dialogue, Cutscene, Startup }

        public GameState State = GameState.Gameplay;

        // TEST
        public Conversation PollimeroConversation;

        protected override void SafeInitialize()
        {
            base.SafeInitialize();
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
                        Cursor.lockState = CursorLockMode.None;
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
                    SetState(GameState.Gameplay);
                    break;
                case GameState.Menu:
                    break;
                case GameState.Gameplay:
                    if(Input.GetKeyDown(KeyCode.P))
					{
                        EventDispatcher.Dispatch(ConversationEvent.Prepare(PollimeroConversation, true));
					}
                    if (ManageCursor)
                    {
                        // Cursor Locking 
                        if (Input.GetKey(KeyCode.Escape))
                        {
                            Cursor.lockState = CursorLockMode.None;
                            Cursor.visible = true;
                        }
                        if (Input.GetMouseButtonDown(0))
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
            for (int i = 0; i < Characters.Count; i++)
            {
                if(Characters[i].CharacterData == null)
				{
                    continue;
				}

                if (Characters[i].CharacterData.ID == data.ID) return Characters[i];
            }

            Debug.LogWarning("Character doesn't exist!");
            return null;
        }
    }
}