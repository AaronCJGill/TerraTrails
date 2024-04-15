using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform Transform;

    private float Time1;
    private float Time2;

    private float AvgTime;
    private bool FinishShakeFirst;
    private float Duration;
    private bool IsUpdate;

    public float Bound1;
    public float Bound2;

    // Start is called before the first frame update
    void Start()
    {
        IsUpdate = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsUpdate)
        {
            return;
        }

        Time2 += Time.deltaTime;

        float shakeCam = 0;

        if (FinishShakeFirst)
        {
            shakeCam = Lerp(Bound1, Bound2, Time2 / AvgTime);
        }
        else
        {
            shakeCam = Lerp(Bound2, 0, Time2 / AvgTime);
        }

        if (FinishShakeFirst && Time2 >= AvgTime)
        {
            Time2 = 0;
            FinishShakeFirst = false;
        }

        Transform.localEulerAngles = new Vector3(0, 0, shakeCam);

        Time1 += Time.deltaTime;

        if (Time1 >= Duration)
        {
            IsUpdate = false;
        }
    }

    public void Shake(float duration)
    {
        IsUpdate = true;
        Time1 = 0;
        Duration = duration;
        AvgTime = duration / 2.0f;
        FinishShakeFirst = true;
    }

    private float Lerp(float v1, float v2, float t)
    {
        if (t >= 1)
        {
            return v2;
        }

        float result = v1 + (v2 - v1) * t;

        return result;
    }
}
