using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] TextMeshProUGUI textMesh;

    void Update()
    {
        textMesh.text = "Score: "+levelManager.Score.ToString();
    }
}
