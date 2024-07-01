using UnityEngine;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    [RequireComponent(typeof(Image))]
    public class SpriteLooper : LCHMonoBehaviour
    {
        [SerializeField] private float speed = 1;
        [SerializeField] private Sprite[] sprites;


        private float _startTime;
        
        
        private Image Image => _image == null ? _image = GetComponent<Image>() : _image;
        private Image _image;


        protected override void OnEnable()
        {
            base.OnEnable();

            _startTime = Time.time;
        }
        
        private void Update()
        {
            var frame = sprites.Length * speed;
            var framePerSecond = 1f / frame;
            var spriteIndex = (int)((Time.time - _startTime) / framePerSecond) % sprites.Length;
            Image.sprite = sprites[spriteIndex];
        }
    }
}
