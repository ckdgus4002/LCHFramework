using UnityEngine;

namespace LCHFramework.Components
{
    public class Bullet : LCHMonoBehaviour
    {
        private static float Gravity
            // => -(1.0f * Time.deltaTime * Time.deltaTime / 2.0f);
            => 0.98888f;
        
        
        
        [SerializeField] private float angle = 45.0f;
        
        
        private float _elapsedTime;
        
        
        
        protected override void OnEnable()
        {
            base.OnEnable();

            _elapsedTime = 0;
        }
        
        protected void Update()
        {
            _elapsedTime += Time.deltaTime;
            transform.Translate(new Vector3(
                0,
                Mathf.Cos(angle * Mathf.PI / 180.0f) * _elapsedTime, 
                Mathf.Cos(angle * Mathf.PI / 180.0f) * _elapsedTime * Gravity
            ));
            transform.Rotate(new Vector3(
                Mathf.Cos(angle * Mathf.PI / 180.0f), 
                0, 
                0
            ));
        }
        
        private void OnTriggerEnter(Collider col) => Destroy(gameObject);
    }
}

