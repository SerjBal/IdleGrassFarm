using UnityEngine;

namespace Serjbal
{
    public class RotationAnim : MonoBehaviour
    {
        [SerializeField] private Vector3 _direction;
        [SerializeField] private float _speed;
        
        void Update()
        {
            transform.RotateAroundLocal(_direction, _speed);
        }
    }
}
