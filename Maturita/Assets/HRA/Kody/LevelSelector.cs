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
        public bool isLevelspecialny;

        public void OpenScene()
        {
            if(isLevel1)
            {
                SceneManager.LoadScene("Level 1");
            }
            else if(isSelectMenu)
            {
                SceneManager.LoadScene("Level Select");
            }
            else if (isLevelspecialny)
            {
                SceneManager.LoadScene("Level 2");
            }
        }

    }
}
