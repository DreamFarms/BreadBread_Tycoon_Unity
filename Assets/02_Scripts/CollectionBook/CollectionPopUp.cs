using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionPopUp : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI customerName;
    public TextMeshProUGUI customerDescription;
    public Slider slider;

    public Image bread1;
    public Image bread2;

    public bool unlocked;

    public List<Sprite> breadImages;
    public Sprite unKnwoBread;
    public Sprite unKnownCustomer;

    public void SetUpPopUp(string name, string intro, List<string> pb, bool pbv, int count, bool unlocked, Sprite sprite)
    {
        if (unlocked)
        {
            image.sprite = sprite;
            customerName.text = name;
            customerDescription.text = intro;

            slider.value = count / 100;

            bread1.sprite = GetBreadImageByKoreanName(pb[0]);
            bread2.sprite = GetBreadImageByKoreanName(pb[1]);
        }
        else
        {
            Debug.Log(sprite.name);
            image.sprite = unKnownCustomer;
            customerName.text = string.Empty;
            customerDescription.text = string.Empty;

            slider.value = 0;

            bread1.sprite = unKnwoBread;
            bread2.sprite = unKnwoBread;
        }
    }

    public Sprite GetBreadImageByKoreanName(string koName)
    {
        string englishName;

        if (InfoManager.Instance.enKoMappingDic.TryGetValue(koName, out englishName))
        {

            for (int i = 0; i < breadImages.Count; i++) {

                if (breadImages[i].name == englishName)
                {
                    return breadImages[i];
                }
            }
        }
        return unKnwoBread;
    }
}
