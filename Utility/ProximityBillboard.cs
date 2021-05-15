using Cinemachine;
using MacSalad.Core;
using System.Collections;
using System.Collections.Generic;
using TalkBox.Core;
using UnityEngine;
using UnityEngine.Events;

namespace TalkBox.Utility
{
    public class ProximityBillboard : MSBehaviour
    {
        public UnityEvent OnInteract;

        [Header("Parameters")]
        public float Distance = 3f;
        public float Angle = .8f;
        public bool Enabled = true;

        [Header("Events")]
        public UnityEvent Show;
        public UnityEvent Hide;

        private Transform view;
        private bool shown = false;


        // Start is called before the first frame update
        void Start()
        {
#if CINEMACHINE_PRESENT
            view = Camera.main.GetComponent<CinemachineBrain>().transform;

#else
            view = Camera.main.transform;
#endif
        }

        private void Update()
        {
            if (Enabled && DialogueManager.Instance.State == DialogueManager.GameState.Gameplay)
            {
                transform.forward = view.position - transform.position;

                var distance = Vector3.Distance(view.position, transform.position);
                var angle = Vector3.Dot(view.forward, (transform.position - view.position).normalized);

                if (shown)
                {
                    if (distance > Distance || angle < Angle)
                    {
                        shown = false;
                        Hide.Invoke();
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        OnInteraction();
                    }
                }
                else
                {
                    if (distance < Distance && angle > Angle)
                    {
                        shown = true;
                        Show.Invoke();
                    }
                }
            }
        }

        protected void Enable()
        {
            Enabled = true;
        }

        protected void Disable()
        {
            Enabled = false;
            shown = false;
            Hide.Invoke();
        }

        public virtual void OnInteraction()
        {
            OnInteract.Invoke();
        }
    }

}