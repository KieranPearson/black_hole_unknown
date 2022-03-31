using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileSlotNew : MonoBehaviour
{
    private void Awake()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(300, 300);
    }
}
