using System.Collections;
using System.Collections.Generic;
using TalkBox.Core;
using UnityEngine;

namespace TalkBox.Cinematics
{
    public class BlackBars : MonoBehaviour
    {
        public RectTransform upper;
        public RectTransform lower;
        public float Easing = 1;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (DialogueManager.Instance.State == DialogueManager.GameState.Gameplay)
            {
                var scale = upper.localScale;
                scale.y = Mathf.Lerp(scale.y, 0, Time.deltaTime * Easing);
                upper.localScale = scale;

                scale = lower.localScale;
                scale.y = Mathf.Lerp(scale.y, 0, Time.deltaTime * Easing);
                lower.localScale = scale;
            }
            else
            {
                var scale = upper.localScale;
                scale.y = Mathf.Lerp(scale.y, 1, Time.deltaTime * Easing);
                upper.localScale = scale;

                scale = lower.localScale;
                scale.y = Mathf.Lerp(scale.y, 1, Time.deltaTime * Easing);
                lower.localScale = scale;
            }
        }
    }
}