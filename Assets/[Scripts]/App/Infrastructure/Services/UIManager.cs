using UnityEngine;
using UnityEngine.InputSystem;

namespace Serjbal
{
    public sealed class UIManager : MonoBehaviour, IUIManager
    {
        public void Init()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Debug.Log("UI Initialized");
        }
       
        private void Update()
        {
            var currentKey = Keyboard.current;
            if (currentKey != null)
            {
                if (currentKey.escapeKey.wasPressedThisFrame)
                {
                    // ShowPage(PageName.QuitGamePopup);
                }
            }
        }
    }
}
