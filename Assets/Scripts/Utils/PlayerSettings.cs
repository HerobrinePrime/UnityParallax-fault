using UnityEngine;

namespace DefaultNamespace.Utils
{
    public class PlayerSettings
    {
        private static PlayerSettings _instance;

        public static PlayerSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PlayerSettings();
                }
            }
        }
    }
}