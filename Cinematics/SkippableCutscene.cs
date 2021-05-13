using MacSalad.Core.Events;
using System.Collections;
using System.Collections.Generic;
using TalkBox.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace TalkBox.Cinematics
{
    public class SkippableCutscene : MonoBehaviour
    {
        public PlayableDirector director;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (DialogueManager.Instance.State == DialogueManager.GameState.Cutscene)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    director.time = director.playableAsset.duration;
                }
            }
        }
    }
}