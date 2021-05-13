using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace TalkBox.Core
{

    [CreateAssetMenu(fileName = "Character", menuName = "Data/Character", order = 1)]
    public class CharacterData : ScriptableObject
    {
        public string Name;

        [HideInInspector] public string ID = Guid.NewGuid().ToString();
    }
}