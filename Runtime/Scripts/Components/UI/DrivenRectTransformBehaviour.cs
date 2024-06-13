using UnityEngine;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    public class DrivenRectTransformBehaviour : LCHMonoBehaviour
    {
        protected DrivenRectTransformTracker Tracker { get; } = new();
        
        
        
        protected override void OnDisable()
        {
            base.OnDisable();
            
            Tracker.Clear();
            LayoutRebuilder.MarkLayoutForRebuild(RectTransformOrNull);
        }
    }
}
