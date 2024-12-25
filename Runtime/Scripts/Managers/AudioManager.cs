using LCHFramework.Data;

namespace LCHFramework.Managers
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        public ReactiveProperty<float> MasterVolume;
        
        protected override bool IsDontDestroyOnLoad => true;
        
                
        
        public virtual void Play(string type, float volume)
        {
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
