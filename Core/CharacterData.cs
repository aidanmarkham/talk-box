﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Serialization;
#pragma warning disable 660,661

namespace TalkBox.Core
{

    [CreateAssetMenu(fileName = "Character", menuName = "TalkBox/Character", order = 1)]
    public class CharacterData : ScriptableObject
    {
        [FormerlySerializedAs("Name")] public string FullName;
        public string ShortName;
        public Color CharacterColor = new Color(21f/255f, 26f/255f, 34f/255f);
        public string ID = Guid.NewGuid().ToString();
        
        [Button]
        public void GenerateNewID()
        {
            ID = Guid.NewGuid().ToString();
        }

        public static bool operator ==(CharacterData lhs, CharacterData rhs)
        {
            if (!lhs && !rhs) return true;
            if (!lhs || !rhs) return false;
            return lhs.ID == rhs.ID;
        }

        public static bool operator !=(CharacterData lhs, CharacterData rhs)
        {
            return !(lhs == rhs);
        }
    }
}