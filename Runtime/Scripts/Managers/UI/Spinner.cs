using System;
using System.Collections.Generic;
using System.Linq;
using LCHFramework.Extensions;
using LCHFramework.Utilities;
using UniRx;
using UnityEngine;

namespace LCHFramework.Managers.UI
{
    public class Spinner : Spinner<Spinner>
    {
    }
    
    public class Spinner<T>: MonoSingleton<T> where T : Spinner<T>
    {
        [SerializeField] private GameObject spinnerImage;
        
        
        private readonly Dictionary<IDisposable, System.IDisposable> showDisposables = new();
        
        
        public override bool IsShown => Wrapper.activeInHierarchy;
        
        private GameObject Wrapper => _wrapper == null ? _wrapper = transform.GetChild(0).gameObject : _wrapper;
        [NonSerialized] private GameObject _wrapper;
        
        
        
        private void Reset() => spinnerImage = Wrapper.transform.GetChildren().Last().gameObject;
        
        protected override void Awake()
        {
            base.Awake();
            
            spinnerImage.SetActive(false);
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            Release();
        }
        
        
        
        public virtual IDisposable Show(float showImageDelay = 0)
        {
            Wrapper.SetActive(true);
            
            var disposable = new IDisposable { spinnerOrNull = this };
            showDisposables.Add(disposable, Observable.Timer(TimeSpan.FromSeconds(showImageDelay), Scheduler.MainThreadIgnoreTimeScale).Subscribe(_ => spinnerImage.SetActive(true)));
            return disposable;
        }

        private void Release() => Hide();
        
        public virtual void Hide() { while (showDisposables.Exists()) Hide(showDisposables.First().Key); }

        public virtual void Hide(IDisposable disposable)
        {
            if (!showDisposables.Remove(disposable, out var observable)) return;
            
            IDisposableUtility.DisposeAndSetDefault(ref observable);
            if (0 < showDisposables.Count) return;
            
            Wrapper.SetActive(false);
            spinnerImage.SetActive(false);
        }
        
        
        
        public class IDisposable : System.IDisposable
        {
            public Spinner<T> spinnerOrNull;
            
            public void Dispose() { if (spinnerOrNull != null) spinnerOrNull.Hide(this); }
        }
    }
}