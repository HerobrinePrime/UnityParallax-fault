using System;
using DefaultNamespace.Utils;
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
            /*
             * TODO: Should be initialized in InitializeFromSettings
             */
            Application.targetFrameRate = targetFrameRate;
            SetBackgroundRunningType(backgroundRunningType);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            PauseOrResume(!hasFocus);
        }

        private void OnApplicationQuit()
        {
            PlayerSettingPref.Instance.Save();
        }

        private void PauseOrResume(bool goToPause)
        {
            switch (backgroundRunningType)
            {
                case BackgroundRunningType.Running:
                    Time.timeScale = 1;
                    Application.targetFrameRate = targetFrameRate;
                    break;
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

        public void SetTargetFrameRate(int frameRate)
        {
            targetFrameRate = frameRate;
        }
        
        /*
         * TODO: Band this to Dropdown menu in Unity Editor
         */
        public void SetBackgroundRunningType(BackgroundRunningType type)
        {
            backgroundRunningType = type;
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
        
        /*
         * TODO: Load settings from PlayerSettingPref
         */
        public void InitializeFromSettings()
        {
            
        }
        
        /*
         * TODO:
         */
        public void GetMetaSettings()
        {
        
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            SetBackgroundRunningType(backgroundRunningType);
        }
#endif
    }
}

public enum BackgroundRunningType
{
    Running,
    Muted,
    Paused,
    Stopped,
};