using System;
using UnityEngine;

namespace DefaultNamespace.Utils
{
    [Serializable]
    public class PlayerSettings
    {
        private const string Key = "PLAYER_SETTINGS";
        
        [SerializeField] private bool reverse;
        [SerializeField] private float volume;
        [SerializeField] private bool muted;
        [SerializeField] private float menuTransparency;
        [SerializeField] private float xConstraint;
        [SerializeField] private float yConstraint;
        
        
        private static PlayerSettings _instance;

        public static PlayerSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PlayerSettings();
                }
                return _instance;
            }
        }
    }
}