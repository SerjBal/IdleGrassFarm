using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _stoppingDistance = 0;
    [SerializeField] private Transform _target;
    
    private void Update()
    {
        if (_target == null) return;
        
        Vector3 direction = _target.position - transform.position;
        direction.y = 0;
        
        if (direction.magnitude > _stoppingDistance)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                targetRotation, 
                Time.deltaTime * 10f
            );
            
            Vector3 moveDirection = transform.forward * _speed * Time.deltaTime;
            _controller.Move(moveDirection);
        }
    }
    
    public void SetTarget(Transform target)
    {
        _target = target;
    }
}