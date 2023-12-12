using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTypeImage : MonoBehaviour
{
    [SerializeField]
    private PanelButton parentButton;

    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        image.sprite = parentButton.ButtonType.BaseSprite;

        transform.rotation = parentButton.ControlPanel.PlayerShip.transform.rotation;
    }
}