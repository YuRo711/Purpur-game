using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonChargeText : MonoBehaviour
{
    private PanelButton parentButton;
    private TextMeshProUGUI textMesh;

    // Start is called before the first frame update
    void Start()
    {
        parentButton = GetComponentInParent<PanelButton>();
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        textMesh.text = ((int)(parentButton.CurrentCharge * 100)).ToString() + "%";
    }
}
