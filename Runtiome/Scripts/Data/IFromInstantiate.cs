using UnityEngine;

namespace LCHFramework.Data
{
    public interface IFromInstantiate
    {
        public (Transform, Vector3) FromInstantiate { get; set; }
    }
}