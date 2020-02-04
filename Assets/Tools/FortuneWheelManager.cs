using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class FortuneWheelManager : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public int[] RewardChance;
    int NumberOfReward
    {
        get
        {
            return RewardChance.Length;
        }
    }
    public bool _isStarted;
    public GameObject Circle;
    private float[] _sectorsAngles;
    private float _finalAngle;
    private float _startAngle = 0;
    private float _currentLerpRotationTime;
    private int Result = 0;
    public delegate void TrackDelegate(int Result);
    public event TrackDelegate Complete;

    private void Start()
    {
        _isStarted = false;
    }
    public void TurnWheel()
    {
        _currentLerpRotationTime = 0f;
        float step = 360f / NumberOfReward;
        _sectorsAngles = new float[NumberOfReward];
        for (int i = 0; i < NumberOfReward; i++)
        {
            _sectorsAngles[i] = (i + 1) * step;
        }
        int fullCircles = 5;
        int rand = UnityEngine.Random.Range(1, 101);
        for (int i = 0; i < NumberOfReward; i++)
        {
            if (rand <= RewardChance[i])
            {
                Result = i;
                break;
            }
        }
        float randomFinalAngle = _sectorsAngles[Result];
        _finalAngle = (fullCircles * 360 + randomFinalAngle);
        _isStarted = true;
    }

    void Update()
    {
        if (!_isStarted)
            return;
        float maxLerpRotationTime = 4f;
        _currentLerpRotationTime += Time.deltaTime;
        if (_currentLerpRotationTime > maxLerpRotationTime || Circle.transform.eulerAngles.z == _finalAngle)
        {
            _currentLerpRotationTime = maxLerpRotationTime;
            _isStarted = false;
            _startAngle = _finalAngle % 360;
            Complete(Result);
        }
        float t = _currentLerpRotationTime / maxLerpRotationTime;
        t = t * t * t * (t * (6f * t - 15f) + 10f);
        float angle = Mathf.Lerp(_startAngle, _finalAngle, t);
        Circle.transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
