using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Launcher
{
    public class BestScoreText : MonoBehaviour
    {
        void Start()
        {
            var scoreText = GetComponent<TextMeshProUGUI>();
            scoreText.text = "Best Score: "+PlayerPrefs.GetInt("highscore").ToString(); 
        }
    }
}