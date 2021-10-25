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
        StartCoroutine(StartMove(reverse, duration));
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
                (moveDistance / time) * Time.deltaTime);

            yield return null;
        }

        onMoveDone.Invoke();
    }
}
