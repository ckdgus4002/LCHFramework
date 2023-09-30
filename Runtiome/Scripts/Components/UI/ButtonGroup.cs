using System.Collections.Generic;
using System.Linq;
using LCHFramework.Extensions;
using LCHFramework.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LCHFramework
{
    public class ButtonGroup : MonoBehaviour
    {
        public ButtonClickWhenIsOnType buttonClickWhenIsOn;
        
        
        
        public UnityEvent<ButtonInfo> onValueChanged;
        
        
        
        public virtual List<ButtonInfo> ButtonInfos
        {
            get
            {
                if (_buttonInfos.IsEmpty())
                {
                    _buttonInfos = new List<ButtonInfo>();
                    foreach (var button in GetComponentsInChildren<Button>(true))
                    {
                        var buttonInfo = new ButtonInfo(this, button);
                        UnityEventUtil.AddPersistentListener(button.onClick, () => OnButtonClick(button));
                        _buttonInfos.Add(buttonInfo);
                    }
                }
                
                return _buttonInfos;
            }
        }
        protected List<ButtonInfo> _buttonInfos;
        

        
        private void OnButtonClick(Button button)
        {
            var buttonInfo = ButtonInfos.FirstOrDefault(item => item.Button == button);
            bool isOn;
            bool isNotify;
            if (!buttonInfo.GetIsOn())
            {
                isOn = true;
                isNotify = true;
            }
            else if (buttonClickWhenIsOn == ButtonClickWhenIsOnType.None)
            {
                isOn = true;
                isNotify = false;
            }
            else if (buttonClickWhenIsOn == ButtonClickWhenIsOnType.Off)
            {
                isOn = false;
                isNotify = true;
            }
            else // if (buttonClickWhenIsOn == ButtonClickWhenIsOnType.Relaunch)
            {
                isOn = true;
                isNotify = true;
            }
            
            buttonInfo.SetIsOn(isOn, isNotify);
        }
        
        
        
        public enum ButtonClickWhenIsOnType
        {
            None = 0,
            Off,
            Relaunch
        }
        
        public class ButtonInfo
        {
            public ButtonInfo(ButtonGroup buttonGroup, Button button)
            {
                _buttonGroup = buttonGroup;
                Button = button;
            }
            
            
            
            private readonly ButtonGroup _buttonGroup;
            private bool _isOn;
            
            
            public Button Button { get; }
            
            
            
            public bool GetIsOn() => _isOn;
            
            public void SetIsOn(bool value, bool sendCallback = true)
            {
                _isOn = value;
                if (sendCallback) _buttonGroup.onValueChanged?.Invoke(this);
            }
        }
    }
}
