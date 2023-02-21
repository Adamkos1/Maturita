using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AH
{
    public class LevelToNumber : MonoBehaviour
    {
        public Text itemToText;

        public void SetLevelCountText(int playerLevel)
        {
            itemToText.text = playerLevel.ToString();
        }
    }
}
