using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace TalkBox.Core
{

    [CreateAssetMenu(fileName = "Character", menuName = "TalkBox/Character", order = 1)]
    public class CharacterData : ScriptableObject
    {
        public string Name;

        public string ID = Guid.NewGuid().ToString();
        [Button]
        public void GenerateNewID()
        {
            ID = Guid.NewGuid().ToString();
        }
    }
}