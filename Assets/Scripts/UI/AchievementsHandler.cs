using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementsHandler : MonoBehaviour
{
    [SerializeField] private Transform achievementsList;
    [SerializeField] private GameObject achievementSlotPrefab;

    private void PopulateAchievementsList()
    {
        RectTransform achievementSlotRect = achievementSlotPrefab.GetComponent<RectTransform>();
        Achievement[] achievements = Achievements.instance.GetAchievements();
        if (achievements == null) return;
        for (int i = 0; i < achievements.Length; i++)
        {
            GameObject achievementSlot = Instantiate(achievementSlotPrefab);
            Transform achievementSlotTransform = achievementSlot.transform;
            GameObject achievementNameObj = achievementSlotTransform.Find("Name").gameObject;
            GameObject achievementDescriptionObj = achievementSlotTransform.Find("Description").gameObject;
            GameObject achievementIconObj = achievementSlotTransform.Find("Icon").gameObject;
            TMP_Text achievementName = achievementNameObj.GetComponent<TMP_Text>();
            TMP_Text achievementDescription = achievementDescriptionObj.GetComponent<TMP_Text>();
            Image achievementIcon = achievementIconObj.GetComponent<Image>();
            achievementName.text = achievements[i].GetName();
            achievementDescription.text = achievements[i].GetDescription();
            achievementIcon.sprite = achievements[i].GetIcon();
            achievementSlotTransform.SetParent(achievementsList, false);
            RectTransform slotRect = achievementSlot.GetComponent<RectTransform>();
            slotRect.sizeDelta = new Vector2(achievementSlotRect.sizeDelta.x, achievementSlotRect.sizeDelta.y);
        }
    }

    private void Start()
    {
        PopulateAchievementsList();
    }
}
