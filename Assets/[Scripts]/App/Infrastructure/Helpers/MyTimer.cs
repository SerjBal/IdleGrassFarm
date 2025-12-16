using UnityEngine;
using System;

public class MyTimer : MonoBehaviour
{
    public Action OnStopAction { get; set; }
    // public Action<float> OnUpdateAction { get; set; }

    private float time;
    private float _tSpeed;

    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime * _tSpeed;
        } else
        {
            OnStopAction?.Invoke();
            Destroy();
        }
    }

    public void Init(float t, float tSpeed = 1)
    {
        time = t;
        _tSpeed = tSpeed;
    }

    public float AddTime(float t) => time += t;

    public void Destroy() => Destroy(gameObject);

    public static MyTimer Create()
    {
        var go = new GameObject();
        return go.AddComponent<MyTimer>();
    }
}