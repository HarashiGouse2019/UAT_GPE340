using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForDestroy : MonoBehaviour
{
    [SerializeField, Range(1, 10)] private float durationTime;

    public enum Method
    {
        DEACTIVATE,
        DESTROY_COMPLETELY
    }

    [SerializeField] private Method destroyMethod;

    private float time;

    private const float reset = 0;

    void Update()
    {
        if (enabled) RunClock();
    }

    void RunClock()
    {
        time += Time.deltaTime;

        if (time >= durationTime)
        {

            switch (destroyMethod)
            {
                case Method.DEACTIVATE:
                    gameObject.SetActive(false);
                    time = reset;
                    break;

                case Method.DESTROY_COMPLETELY:
                    Destroy(gameObject);
                    break;
            }
        }
    }
}
