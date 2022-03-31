using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProfileSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text highscoreText;

    public static event System.Action<string> OnProfileSelected;

    private void Awake()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(300, 300);
    }

    public void SetName(string newName)
    {
        nameText.text = newName;
    }

    public void SetHighscore(int newHighscore)
    {
        highscoreText.text = "Highscore: " + newHighscore.ToString("N0");
    }

    public void SelectProfileButtonClicked()
    {
        OnProfileSelected?.Invoke(nameText.text);
    }
}
