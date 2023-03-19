using UnityEngine;

namespace LCHFramework.Modules
{
    public interface IFromInstantiate
    {
        public (Transform, Vector3) FromInstantiate { get; set; }
    }
}