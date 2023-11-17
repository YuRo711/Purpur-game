using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonChargeText : MonoBehaviour
{
    private PanelButton parentButton;
    private TextMeshProUGUI textMesh;

    void Start()
    {
        parentButton = GetComponentInParent<PanelButton>();
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        textMesh.text = ((int)(parentButton.CurrentCharge * 100)).ToString() + "%";

        if (!parentButton.IsFunctional)
            textMesh.color = Color.red;

        else if(parentButton.IsFullyCharged)
            textMesh.color = Color.green;

        else if (parentButton.IsCharging)
            textMesh.color = Color.yellow;

        else
            textMesh.color = Color.white;
    }
}
