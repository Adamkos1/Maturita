using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AH
{

    public class LevelSelector : MonoBehaviour
    {
        public bool isSelectMenu;
        public bool isLevel1;

        public void OpenScene()
        {
            if(isSelectMenu)
            {
                SceneManager.LoadScene("Level 1");
            }
            else if(isLevel1)
            {
                SceneManager.LoadScene("Level Select");
            }
        }

    }
}
