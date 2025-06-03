using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace.Utils
{
    [Serializable]
    public class PlayerSettingPref
    {
        private const string Key = "PLAYER_SETTINGS";

        // [SerializeField] private bool isInitialized = false;

        [SerializeField] public OtherSettings otherSettings;

        public OtherSettings OtherSettings
        {
            get => otherSettings;
            set => otherSettings = value;
        }
        [SerializeField] private BGControllerSettings bgControllerSettings;
        public BGControllerSettings BGControllerSettings
        {
            get => bgControllerSettings;
            set => bgControllerSettings = value;
        }

        [SerializeField] private ApplicationSettings applicationSettings;
        public ApplicationSettings ApplicationSettings
        {
            get => applicationSettings;
            set => applicationSettings = value;
        }

        private static PlayerSettingPref _instance;

        public static PlayerSettingPref Instance
        {
            get
            {
                if (_instance == null)
                {
                    if (PlayerPrefs.HasKey(Key))
                    {
                        _instance = JsonUtility.FromJson<PlayerSettingPref>(PlayerPrefs.GetString(Key));
                    }
                    else
                    {
                        _instance = new PlayerSettingPref();
                        _instance.InitializeNewSetting();
                    }
                }

                return _instance;
            }
        }

        public void Save()
        {
            string json = JsonUtility.ToJson(this);
            PlayerPrefs.SetString(Key, json);
            PlayerPrefs.Save();
        }

        public void InitializeNewSetting()
        {
            /*
             * TODO: Get default values from BGController | ApplicationSetting | UIController
             */
            this.ApplicationSettings = ApplicationSetting.Instance.GetMetaSettings();
            this.OtherSettings = UIController.Instance.GetMetaSettings();
            this.BGControllerSettings = BGController.Instance.GetMetaSettings();

            // this.isInitialized = true;
        }
    }
}

[Serializable]
public class OtherSettings
{
    [SerializeField] private float volume;
    [SerializeField] private bool muted;
    [SerializeField] private float menuTransparency;

    #region MyRegion

    public OtherSettings(float volume, bool muted, float menuTransparency)
    {
        this.volume = volume;
        this.muted = muted;
        this.menuTransparency = menuTransparency;
    }

    public float Volume
    {
        get => volume;
        set => volume = value;
    }

    public bool Muted
    {
        get => muted;
        set => muted = value;
    }

    public float MenuTransparency
    {
        get => menuTransparency;
        set => menuTransparency = value;
    }

    #endregion
}

[Serializable]
public class BGControllerSettings
{
    [SerializeField] private bool reverse;
    [SerializeField] private float parallaxScale;
    [SerializeField] private float xConstraint;
    [SerializeField] private float yConstraint;

    #region MyRegion

    public BGControllerSettings(bool reverse, float parallaxScale, float xConstraint, float yConstraint)
    {
        this.reverse = reverse;
        this.parallaxScale = parallaxScale;
        this.xConstraint = xConstraint;
        this.yConstraint = yConstraint;
    }

    public bool Reverse
    {
        get => reverse;
        set => reverse = value;
    }

    public float ParallaxScale
    {
        get => parallaxScale;
        set => parallaxScale = value;
    }

    public float XConstraint
    {
        get => xConstraint;
        set => xConstraint = value;
    }

    public float YConstraint
    {
        get => yConstraint;
        set => yConstraint = value;
    }

    #endregion
}

[Serializable]
public class ApplicationSettings
{
    [SerializeField] private int targetFrameRate;
    [SerializeField] private BackgroundRunningType backgroundRunningType;

    #region MyRegion

    public ApplicationSettings(int targetFrameRate, BackgroundRunningType backgroundRunningType)
    {
        this.targetFrameRate = targetFrameRate;
        this.backgroundRunningType = backgroundRunningType;
    }

    public int TargetFrameRate
    {
        get => targetFrameRate;
        set => targetFrameRate = value;
    }

    public BackgroundRunningType BackgroundRunningType
    {
        get => backgroundRunningType;
        set => backgroundRunningType = value;
    }

    #endregion
}