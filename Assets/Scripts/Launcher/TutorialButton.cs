using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Launcher
{
    public class TutorialButton : MonoBehaviour
    {

        public void GoToTutorial()
        {
            SceneManager.LoadScene("Tutorial");
        }
    }
}