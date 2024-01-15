using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Launcher
{
    public class ExitButton : MonoBehaviour
    {

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}