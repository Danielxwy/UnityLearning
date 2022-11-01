using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFlash : MonoBehaviour
{
    public static ScreenFlash instance;

    private Image img;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        img = GetComponent<Image>();
    }

    public void Flash()
    {
        img.color = new Color(1, 0.2f, 0.2f, 0.1f);
        StartCoroutine(FlashBack());
    }

    IEnumerator FlashBack()
    {
        yield return new WaitForSeconds(0.05f);
        img.color = new Color(1, 0.2f, 0.2f, 0);
    }
}
