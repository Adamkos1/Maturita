using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AH
{

    public class LevelSelector : MonoBehaviour
    {
        public void OpenScene()
        {
            SceneManager.LoadScene("Level 1");
        }

    }
}
