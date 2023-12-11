using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonStateImage : MonoBehaviour
{
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite chargedSprite;
    [SerializeField] private Sprite brokenSprite;
    [SerializeField] private PanelButton parentButton;

    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (!parentButton.IsFunctional)
            image.sprite = brokenSprite;

        else if (parentButton.IsFullyCharged)
            image.sprite = chargedSprite;

        else
            image.sprite = activeSprite;
    }
}
