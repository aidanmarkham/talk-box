using MacSalad.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TalkBox.Core
{
    public class Character : MSBehaviour
    {
        public CharacterData CharacterData;

        protected override void SafeInitialize()
        {
            base.SafeInitialize();

            CharacterData = Instantiate(CharacterData);
        }
    }
}