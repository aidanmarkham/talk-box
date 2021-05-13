using MacSalad.Core;
using MacSalad.Core.Events;
using TalkBox.Core;
using static TalkBox.Core.Events;


namespace TalkBox.Utility
{
    public class CharacterCamera : MSBehaviour
    {
#if CINEMACHINE_PRESENT
        public int ActivePriority = 100;
        public int InactivePriority = 0;
        public CharacterData Character;
        private Cinemachine.CinemachineVirtualCamera Camera;


        protected override void SafeInitialize()
        {
            base.SafeInitialize();
            Camera = GetComponent<Cinemachine.CinemachineVirtualCamera>();
        }

        protected override void AddEventListeners()
        {
            base.AddEventListeners();
            EventDispatcher.AddListener<DialogueEvent>(DialogueStarted);
        }

        protected override void RemoveEventListeners()
        {
            base.RemoveEventListeners();

            EventDispatcher.RemoveListener<DialogueEvent>(DialogueStarted);
        }

        private void DialogueStarted(DialogueEvent e)

        {
            if (e.Started)
            {
                if (e.Source.Character == Character)
                {
                    Camera.Priority = ActivePriority;
                }
                else
                {
                    Camera.Priority = InactivePriority;
                }
            }
            else
            {
                Camera.Priority = InactivePriority;
            }
        }
#endif
    }
}