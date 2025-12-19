using System;
using UnityEngine;

public class ScaleAnim : MonoBehaviour
{
    [SerializeField] private AnimationCurve _scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private float _duration = 0.5f;
    
    private Vector3 _originalScale;
    private float _startTime;
    private Transform _target;

    private void Awake() => _target = transform.parent;

    private void OnEnable()
    {
        _originalScale = _target.localScale;
        _startTime = Time.time;
    }

    private void Update()
    {
        float elapsed = Time.time - _startTime;
        float t = Mathf.Clamp01(elapsed / _duration);

        float curveValue = _scaleCurve.Evaluate(t);
        _target.localScale = _originalScale + Vector3.one * curveValue;

        if (t >= 1f)
        {
            _target.localScale = _originalScale;
            Destroy(gameObject);
        }
    }
}