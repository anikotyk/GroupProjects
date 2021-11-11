using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutoMove : MonoBehaviour
{
    public Vector3 moveOffset;
    public bool onStart, reverse;
    public float duration;
    public UnityEvent onStartMove, onMoveDone;

    private Vector3 targetPos, initialPos;
    private float moveDistance;

    public Coroutine Moving;

    public float timecoef=1;

    public bool isStoping = false;

    // Use this for initialization
    void Start ()
    {
        initialPos = transform.localPosition;
        moveDistance = moveOffset.magnitude;

        if (onStart)
        {
            Move(reverse);
        }
	}

    public void Move(bool reverse)
    {
        Moving = StartCoroutine(StartMove(reverse, duration));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator StartMove(bool reverse, float time)
    {
        if (reverse)
        {
            targetPos = initialPos;
            transform.localPosition += moveOffset;
        }
        else
            targetPos = transform.localPosition + moveOffset;

        onStartMove.Invoke();

        while (transform.localPosition != targetPos)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, 
                (moveDistance / time) * Time.deltaTime/ timecoef);

            yield return null;
        }

        onMoveDone.Invoke();
    }

    public void ChangeTimeCoef(float newcoef)
    {
        if (newcoef != 0 && !isStoping)
        {
            timecoef = newcoef;
        }
    }

    public void SlowlyStopF(float time)
    {
        StartCoroutine(SlowlyStop(time));
    }

    public void SlowlyStartF(float time)
    {
        StartCoroutine(SlowlyStart(time));
    }

    public IEnumerator SlowlyStop(float time)
    {
        isStoping = true;
        int cnt = (int)((10 - timecoef) / 0.1f);
        WaitForSeconds time1 = new WaitForSeconds(time*1.0f / cnt);
        for(int i=0; i < cnt; i++) {
            timecoef += 0.1f;
            yield return time1;
            Debug.Log("Slowing");
        }
        StopCoroutine(Moving);
    }

    public IEnumerator SlowlyStart(float time)
    {
        Moving = StartCoroutine(StartMove(reverse, duration));
        int cnt = (int)((timecoef - 1) / 0.1f);
        WaitForSeconds time1 = new WaitForSeconds(time * 1.0f / cnt);
        for (int i = 0; i < cnt; i++)
        {
            timecoef -= 0.1f;
            yield return time1;
            Debug.Log("Fasting");
        }
        timecoef = 1f;
        isStoping = false;
    }
}
