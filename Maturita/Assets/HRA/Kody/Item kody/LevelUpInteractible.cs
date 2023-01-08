using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{

    public class LevelUpInteractible : Interactable
    {
        public override void Interact(PlayerManager playerManager)
        {
            playerManager.uIManager.levelUpWindow.SetActive(true);
        }
    }

}
