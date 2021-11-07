using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomObstructSign : MonoBehaviour
{
    public ParticleSystem particle;

    private WaitForSeconds waitSec = null;
    private void Awake()
    {
        waitSec = new WaitForSeconds(1f);
    }
    public void Init(GloomController _gloom, Vector3 _startPos, Vector3 _endPos)
    {

        particle.Play();
        StartCoroutine(ProcessSummonBullet(_gloom, _startPos, _endPos));

    }

    private IEnumerator ProcessSummonBullet(GloomController _gloom, Vector3 _startPos, Vector3 _endPos)
    {
        //절반씩 나눠서 기다리기

        yield return waitSec;
        GloomObstructBullet bullet = _gloom.Pool.obstructBullet.SpawnThis(_startPos);
        bullet.Init(_gloom, _startPos, _endPos);
        bullet.Move();


        yield return waitSec;

        _gloom.Pool.obstructSign.ReleaseThis(this);

    }
}
