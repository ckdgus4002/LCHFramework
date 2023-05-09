using UnityEngine;
using UnityEngine.Windows;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LCHFramework
{
    public class LCHFrameworkSettings : ScriptableObject
    {
        private static readonly string DirectoryPath = $"Assets/{nameof(Resources)}";
        private static readonly string BaseName = $"{nameof(LCHFrameworkSettings)}";
        private static readonly string FileName = $"{BaseName}.asset";
        private static readonly string FileFullName = $"{DirectoryPath}/{FileName}";


        public static LCHFrameworkSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    CreateAssetIfEmpty();
                    _instance = Resources.Load<LCHFrameworkSettings>(FileName);
                }
                
                return _instance;
            }
        }
        private static LCHFrameworkSettings _instance;



#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void InitializeOnLoadMethod()
        {
            CreateAssetIfEmpty();   
        }

        private static void CreateAssetIfEmpty()
        {
            if (AssetDatabase.LoadAssetAtPath<LCHFrameworkSettings>(FileFullName)) return;
            
            if (!Directory.Exists(DirectoryPath)) Directory.CreateDirectory(DirectoryPath);

            AssetDatabase.CreateAsset(CreateInstance<LCHFrameworkSettings>(), $"{DirectoryPath}/{FileName}");
        }
#endif
        
        
        
        public Vector2 canvasSize = new Vector2(1920, 1080);
    }
}

