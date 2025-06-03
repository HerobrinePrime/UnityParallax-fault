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

        public bool Reverse { get => reverse; set => reverse = value; }
        
        public float Volume { get => volume; set => volume = value; }
        
        public bool Muted { get => muted; set => muted = value; }
        
        public float MenuTransparency { get => menuTransparency; set => menuTransparency = value; }
        
        public float XConstraint { get => xConstraint; set => xConstraint = value; }
        
        public float YConstraint { get => yConstraint; set => yConstraint = value; }
    }
}