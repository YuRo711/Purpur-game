using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Launcher
{
    public class MainMenuGoer : MonoBehaviour
    {
        public void GoToMainMenu()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}