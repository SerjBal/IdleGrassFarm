using UnityEngine;

namespace Serjbal.UI
{
    public class Page : MonoBehaviour
    {
        public GameObject GameObject => gameObject;

        public virtual void Show(bool isTrue) => gameObject.SetActive(isTrue);
        public virtual void Show() => Show(true);
        public virtual void Hide() => Show(false);
        public virtual void Close() => Destroy(gameObject);
    }
}