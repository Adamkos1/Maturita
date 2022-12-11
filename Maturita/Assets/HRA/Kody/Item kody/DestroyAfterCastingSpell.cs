using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AH
{

    public class DestroyAfterCastingSpell : MonoBehaviour
    {
        CharacterManager characterManager;

        private void Awake()
        {
            characterManager = GetComponentInParent<CharacterManager>();
        }

        private void Update()
        {
            if(characterManager.isFiringSpell)
            {
                Destroy(gameObject);
            }
        }
    }

}
