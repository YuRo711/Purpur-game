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
        UpdateType();
        UpdateRotation();
    }

    private void UpdateType()
    {
        if (parentButton.ButtonType == null)
            return;

        image.sprite = parentButton.ButtonType.BaseSprite;
        
    }

    private void UpdateRotation()
    {
        if (parentButton.ControlPanel.PlayerShip == null)
            return;

        transform.rotation = parentButton.ControlPanel.PlayerShip.transform.rotation;
    }
}
