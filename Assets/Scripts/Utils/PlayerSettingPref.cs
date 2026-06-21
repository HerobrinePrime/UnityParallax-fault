using System;
using Enum;
using UI;
using UnityEngine;

namespace Utils
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

        [SerializeField] private TimeSettings timeSettings;

        public TimeSettings TimeSettings
        {
            get => timeSettings;
            set => timeSettings = value;
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
                        Debug.Log("Setting not found, creating new setting using current value");
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
        [SerializeField] private bool useParallax;
        [SerializeField] private bool useFullBackground;
        [SerializeField] private float parallaxScale;
        [SerializeField] private float basicScale;
        [SerializeField] private float damping;
        [SerializeField] private float xConstraint;
        [SerializeField] private float yConstraint;

        #region MyRegion

        public BGControllerSettings(
            bool reverse, bool useParallax, bool useFullBackground, float parallaxScale,
            float basicScale, float damping, float xConstraint, float yConstraint)
        {
            this.reverse = reverse;
            this.useParallax = useParallax;
            this.useFullBackground = useFullBackground;
            this.parallaxScale = parallaxScale;
            this.basicScale = basicScale;
            this.damping = damping;
            this.xConstraint = xConstraint;
            this.yConstraint = yConstraint;
        }

        public bool Reverse
        {
            get => reverse;
            set => reverse = value;
        }

        public bool UseParallax
        {
            get
            {
                // Debug.Log("getting useParallax");
                return useParallax;
            }
            set => useParallax = value;
        }

        public bool UseFullBackground
        {
            get => useFullBackground;
            set => useFullBackground = value;
        }

        public float ParallaxScale
        {
            get => parallaxScale;
            set => parallaxScale = value;
        }

        public float BasicScale
        {
            get => basicScale;
            set => basicScale = value;
        }

        public float Damping
        {
            get => damping;
            set => damping = value;
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

    [Serializable]
    public class TimeSettings
    {
        [SerializeField] private bool forceTime;
        [SerializeField] private bool forceSeason;
        [SerializeField] private Season season;
        [SerializeField] private TimeOfDay timeOfDay;
        [SerializeField] private bool autoCalculateSunPosition;
        [SerializeField] private int dawnStartHour;
        [SerializeField] private int dawnStartMinute;
        [SerializeField] private int dayStartHour;
        [SerializeField] private int dayStartMinute;
        [SerializeField] private int duskStartHour;
        [SerializeField] private int duskStartMinute;
        [SerializeField] private int nightStartHour;
        [SerializeField] private int nightStartMinute;
        [SerializeField] private float transitionDuration;

        #region MyRegion

        public TimeSettings(bool forceTime, bool forceSeason, Season season, TimeOfDay timeOfDay,
            bool autoCalculateSunPosition, int dawnStartHour, int dawnStartMinute, int dayStartHour, int dayStartMinute,
            int duskStartHour, int duskStartMinute, int nightStartHour, int nightStartMinute, float transitionDuration)
        {
            this.forceTime = forceTime;
            this.forceSeason = forceSeason;
            this.season = season;
            this.timeOfDay = timeOfDay;
            this.autoCalculateSunPosition = autoCalculateSunPosition;
            this.dawnStartHour = dawnStartHour;
            this.dawnStartMinute = dawnStartMinute;
            this.dayStartHour = dayStartHour;
            this.dayStartMinute = dayStartMinute;
            this.duskStartHour = duskStartHour;
            this.duskStartMinute = duskStartMinute;
            this.nightStartHour = nightStartHour;
            this.nightStartMinute = nightStartMinute;
            this.transitionDuration = transitionDuration;
        }

        public bool ForceTime
        {
            get => forceTime;
            set => forceTime = value;
        }

        public bool ForceSeason
        {
            get => forceSeason;
            set => forceSeason = value;
        }

        public Season Season
        {
            get => season;
            set => season = value;
        }

        public TimeOfDay TimeOfDay
        {
            get => timeOfDay;
            set => timeOfDay = value;
        }

        public bool AutoCalculateSunPosition
        {
            get => autoCalculateSunPosition;
            set => autoCalculateSunPosition = value;
        }

        public int DawnStartHour
        {
            get => dawnStartHour;
            set => dawnStartHour = value;
        }

        public int DawnStartMinute
        {
            get => dawnStartMinute;
            set => dawnStartMinute = value;
        }

        public int DayStartHour
        {
            get => dayStartHour;
            set => dayStartHour = value;
        }

        public int DayStartMinute
        {
            get => dayStartMinute;
            set => dayStartMinute = value;
        }

        public int DuskStartHour
        {
            get => duskStartHour;
            set => duskStartHour = value;
        }

        public int DuskStartMinute
        {
            get => duskStartMinute;
            set => duskStartMinute = value;
        }

        public int NightStartHour
        {
            get => nightStartHour;
            set => nightStartHour = value;
        }

        public int NightStartMinute
        {
            get => nightStartMinute;
            set => nightStartMinute = value;
        }

        public float TransitionDuration
        {
            get => transitionDuration;
            set => transitionDuration = value;
        }

        #endregion
    }
}