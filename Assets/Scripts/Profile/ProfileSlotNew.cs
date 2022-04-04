using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProfileSlotNew : MonoBehaviour
{
    [SerializeField] private TMP_Text nameInput;

    public static event System.Action<string> OnNewProfile;

    private void Awake()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(300, 300);
    }

    public void NewProfileButtonClicked()
    {
        OnNewProfile?.Invoke(nameInput.text);
    }
}
