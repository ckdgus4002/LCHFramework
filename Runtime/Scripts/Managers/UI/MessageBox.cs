using LCHFramework.Extensions;
using LCHFramework.Managers.UI.MessageBoxItems;
using UnityEngine;
using UnityEngine.UI;

namespace LCHFramework.Managers.UI
{
    [RequireComponent(typeof(CanvasScaler))]
    public class MessageBox : MonoSingleton<MessageBox>
    {
        private GameObject wrapper;
        
        
        public CanvasScaler CanvasScaler => _canvasScaler == null ? _canvasScaler = GetComponent<CanvasScaler>() : _canvasScaler;
        private CanvasScaler _canvasScaler;
        
        internal MessageBoxItem[] Items => _items ??= GetComponentsInChildren<MessageBoxItem>(true);
        private MessageBoxItem[] _items;
        
        
        
        protected override void Awake()
        {
            base.Awake();
            
            wrapper = transform.GetChild(0).gameObject;
        }
        
        
        
        public bool IsShownFromKey(string key) => Items.TryFirstOrDefault(t => t.mKey == key, out var result) && result.IsShown;
        
        public MessageBoxItem Show(string key, params object[] objects)
        {
            wrapper.SetActive(true);
            
            MessageBoxItem result = null;
            Items.ForEach(t =>
            {
                if (t.mKey == key)
                {
                    t.Show(objects);
                    result = t;
                }
                else if (t.IsShown)
                    t.Hide();
            });
            return result;
        }
        
        public MessageBoxItem Hide(string key)
        {
            if (Items.TryFirstOrDefault(t => t.mKey == key, out var result)) result.Hide();
            return result;
        }
        
        public void Hide() => wrapper.SetActive(false);
    }
}
