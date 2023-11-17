using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonImage : MonoBehaviour
{
    private PanelButton parentButton;
    private Image image;

    void Start()
    {
        parentButton = GetComponentInParent<PanelButton>();
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (!parentButton.IsFunctional)
            image.sprite = parentButton.ButtonType.BrokenSprite;

        else if (parentButton.IsFullyCharged)
            image.sprite = parentButton.ButtonType.FullyChargedSprite;

        else
            image.sprite = parentButton.ButtonType.BaseSprite;
    }
}
