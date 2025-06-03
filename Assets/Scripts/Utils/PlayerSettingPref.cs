using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace.Utils
{
    [Serializable]
    public class PlayerSettingPref
    {
        private const string Key = "PLAYER_SETTINGS";

        [SerializeField] private bool isInitialized = false;

        [SerializeField] public OtherSettings otherSettings;
        public OtherSettings OtherSettings => otherSettings;
        [SerializeField] private BGControllerSettings bgControllerSettings;
        public BGControllerSettings BGControllerSettings => bgControllerSettings;


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
    }
}

[Serializable]
public class OtherSettings
{
    [SerializeField] private float volume;
    [SerializeField] private bool muted;
    [SerializeField] private float menuTransparency;

    #region MyRegion

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