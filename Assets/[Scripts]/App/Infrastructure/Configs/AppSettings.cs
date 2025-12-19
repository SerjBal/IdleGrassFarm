using UnityEngine;

namespace Serjbal
{
    [CreateAssetMenu]
    public class AppSettings : ScriptableObject
    {
        public AppSettingsModel model;
        
        private void OnEnable()
        {
            model.InitializeDictionarys();
        }
    }
}