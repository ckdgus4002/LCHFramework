using UnityEngine;

namespace LCHFramework.Managers.UI
{
    public class MessageBoxItem : MonoBehaviour
    {
        public string Key => name;
        
        
        
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
