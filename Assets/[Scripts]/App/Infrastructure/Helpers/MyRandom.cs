using System.Collections.Generic;
using UnityEngine;


public class MyRandom
{
    public MyRandom(int min, int max)
    {
        _min = min;
        _max = max;
        Initialize();
    }

    private List<int> _lastList = new List<int>();
    private int _min;
    private int _max;

    private void Initialize()
    {
        _lastList = new List<int>();
        for (int i = _min; i < _max; i++)
        {
            _lastList.Add(i);
        }
    }

    public int Generate()
    {
        if (_min == _max) return 0;
        if (_lastList.Count == 0) Initialize();

        var i = Random.Range(0, _lastList.Count);
        var x = _lastList[i];
        _lastList.RemoveAt(i);
        return x;
    }
}