using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsHelperResetScroll : MonoBehaviour
{
    Scrollbar vertical;

    private void Start()
    {
        vertical = GetComponent<ScrollRect>().verticalScrollbar;
        Invoke("Reset", 1f);
    }

    private void Reset()
    {
        vertical.value = 1;
    }


}
