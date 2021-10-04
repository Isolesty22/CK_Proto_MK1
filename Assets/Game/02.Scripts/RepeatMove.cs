using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RepeatMove : MonoBehaviour
{
    public float patrolTime;
    public Vector3 patrolPos1;
    public Vector3 patrolPos2;

    Tween tween;

    private void Start()
    {
        patrolPos1 = transform.position;
    }

    private void Update()
    {
        if (transform.position == patrolPos1)
        {
            Utility.KillTween(tween);
            tween = transform.DOMove(patrolPos2, patrolTime).SetEase(Ease.Linear);
            tween.Play();
        }
        else if (transform.position == patrolPos2)
        {
            Utility.KillTween(tween);
            tween = transform.DOMove(patrolPos1, patrolTime).SetEase(Ease.Linear);
            tween.Play();
        }
    }
}
