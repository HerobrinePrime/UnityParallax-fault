using System;
using Enum;
using UnityEngine;

namespace Entity
{
    [Serializable]
    public class LayerTexture
    {
        public LayerType layerType;
        public Material material;
        public SeasonTexture[] seasonTextures;
    }


    [Serializable]
    public class SeasonTexture
    {
        public Season season;
        public TimeTexture[] timeTextures;
    }

    [Serializable]
    public class TimeTexture
    {
        public TimeOfDay time;
        public Texture2D timeTexture;
    }

    [Serializable]
    public class Layer
    {
        public Transform transform;
    }
}