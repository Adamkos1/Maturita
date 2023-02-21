using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AH
{

    public class AmmoToNumber : MonoBehaviour
    {
        public Text itemToText;

        public void SetItemCountText(int itemCount)
        {
            itemToText.text = itemCount.ToString();
        }
    }
}