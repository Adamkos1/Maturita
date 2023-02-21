using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AH
{

    public class UdiedPopUpUI : MonoBehaviour
    {
        CanvasGroup canvas;
        public Image image;
        public GameObject button;
        public GameObject backround;

        private void Awake()
        {
            canvas = GetComponent<CanvasGroup>();
        }

        public void DisplayUdiedPopUp()
        {
            StartCoroutine(FadeInPopUp());
            button.SetActive(true);
            backround.SetActive(true);
        }

        IEnumerator FadeInPopUp()
        {
            image.enabled = true;
            gameObject.SetActive(true);

            for (float fade = 0.05f; fade < 1; fade = fade + 0.05f)
            {
                canvas.alpha = fade;

               // if (fade > 0.9f)
              //  {
               //     StartCoroutine(FadeOutPopUp());
               // }

                yield return new WaitForSeconds(0.05f);
            }
        }

        IEnumerator FadeOutPopUp()
        {
            //2 sekudny az potom to zmizne
            yield return new WaitForSeconds(2);

            for (float fade = 1f; fade > 0; fade = fade - 0.05f)
            {
                canvas.alpha = fade;

                if (fade <= 0.05f)
                {
                    image.enabled = false;
                    gameObject.SetActive(false);
                }

                yield return new WaitForSeconds(0.05f);
            }
        }
    }

}