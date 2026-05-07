using System;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace LCHFramework.Components
{
    [Serializable]
    public class ParticleObjectPool
    {
        [SerializeField] private Transform wrapper;
        [SerializeField] private Particle prefab;
        
        
        public ObjectPool<Particle> ObjectPool => _objectPool ??= new ObjectPool<Particle>
        (
            () =>
            {
                var t = Object.Instantiate(prefab, wrapper);
                t.onStopped += () => ObjectPool.Release(t);
                return t;
            },
            t => t.Show(),
            t => t.Hide(),
            t => Object.Destroy(t.gameObject)
        );
        private ObjectPool<Particle> _objectPool;
    }
}
