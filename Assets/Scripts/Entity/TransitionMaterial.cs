using UnityEngine;

namespace Entity
{
    public class TransitionMaterial
    {
        private Material _material;

        public TransitionMaterial(Material source)
        {
            _material = source;
            _material.SetFloat("_time", 0f);
        }

        public float time
        {
            get => _material.GetFloat("_time");
            set => _material.SetFloat("_time", value);
        }

        public Texture2D textureBefore
        {
            get => (Texture2D)_material.GetTexture("_Texture1");
            set => _material.SetTexture("_Texture1", value);
        }

        public Texture2D textureAfter
        {
            get => (Texture2D)_material.GetTexture("_Texture2");
            set => _material.SetTexture("_Texture2", value);
        }
    }
}