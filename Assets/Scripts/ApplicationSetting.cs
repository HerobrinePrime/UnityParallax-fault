using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class ApplicationSetting : MonoBehaviour
    {
        public int targetFrameRate = 60;
        public BackgroundRunningType backgroundRunningType = BackgroundRunningType.Running;

        public static bool IsMuted = false;

        private void Start()
        {
            Application.targetFrameRate = targetFrameRate;
            SwitchBackgroundRunningType();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            PauseOrResume(!hasFocus);
        }

        private void PauseOrResume(bool goToPause)
        {
            switch (backgroundRunningType)
            {
                case BackgroundRunningType.Running:
                    Time.timeScale = 1;
                    Application.targetFrameRate = targetFrameRate;
                    break;
                /*
                 * TODO: Mute the game when it's paused or stopped
                 */
                case BackgroundRunningType.Muted:
                    IsMuted = goToPause;
                    TestAudio.Instance.GraduallyMuteAudio(goToPause);
                    Time.timeScale = 1;
                    Application.targetFrameRate = targetFrameRate;
                    break;
                case BackgroundRunningType.Paused:
                    Time.timeScale = goToPause ? 0 : 1;
                    Application.targetFrameRate = goToPause ? 1 : targetFrameRate;
                    break;
                case BackgroundRunningType.Stopped:
                    Time.timeScale = goToPause ? 0 : 1;
                    Application.targetFrameRate = goToPause ? 1 : targetFrameRate;
                    break;
            }
        }

        /*
         * TODO: Band this to Dropdown menu in Unity Editor
         */
        private void SwitchBackgroundRunningType()
        {
            switch (backgroundRunningType)
            {
                case BackgroundRunningType.Running:
                    Application.runInBackground = true;
                    break;
                case BackgroundRunningType.Muted:
                    Application.runInBackground = true;
                    break;
                case BackgroundRunningType.Paused:
                    Application.runInBackground = true;
                    break;
                case BackgroundRunningType.Stopped:
                    Application.runInBackground = false;
                    break;
            }

            Debug.Log("Background Running Type: " + backgroundRunningType + " - RunInBackground: " +
                      Application.runInBackground);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            SwitchBackgroundRunningType();
        }
#endif

        void Test()
        {
        }
    }
}

public enum BackgroundRunningType
{
    Running,
    Muted,
    Paused,
    Stopped,
};