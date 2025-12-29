using System;
using UnityEngine;

namespace LCHFramework.Components
{
    public class Particle : MonoBehaviour
    {
        public Action onStopped;
        
        
        public ParticleSystem ParticleSystem => _particleSystem == null ? _particleSystem = GetComponentInChildren<ParticleSystem>() : _particleSystem;
        private ParticleSystem _particleSystem;
        
        
        
        public virtual void OnParticleSystemStopped() => onStopped?.Invoke();
        
        
        
        public virtual void Show(RectTransform parent, Vector3 position)
        {
            transform.position = position;
            transform.SetParent(parent, true);
            Show();
        }
        
        public virtual void Show(RectTransform parent)
        {
            transform.SetParent(parent);
            Show();
        }
        
        public virtual void Show(Vector3 position)
        {
            transform.position = position;
            Show();
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
            ParticleSystem.Play();
        }
        
        public virtual void Hide() => gameObject.SetActive(false);
    }
}