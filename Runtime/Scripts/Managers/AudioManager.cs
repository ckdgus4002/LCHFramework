using System.Collections.Generic;
using LCHFramework.Data;

namespace LCHFramework.Managers
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        public const string Bgm = "Bgm";
        public const string Narration = "Narration";
        public const string Sfx = "Sfx";
        
        
        
        public ReactiveProperty<float> masterVolume;
        public Dictionary<string, AudioController> controllers = new()
        {
            { Bgm, new AudioController() },
            { Narration, new AudioController() },
            { Sfx, new AudioController() },
        };
        
        
        protected override bool IsDontDestroyOnLoad => true;
        
                
        
        public virtual void Play(string type, float volume)
        {
            
        }
    }
}
