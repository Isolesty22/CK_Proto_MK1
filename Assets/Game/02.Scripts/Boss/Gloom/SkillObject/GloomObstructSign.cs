using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomObstructSign : MonoBehaviour
{
    public ParticleSystem particle;

    private WaitForSeconds waitSec = null;

    private Vector3 bulletRotation = Vector3.zero;
    private void Awake()
    {
        waitSec = new WaitForSeconds(1f);
    }
    public void Init(GloomController _gloom, Vector3 _startPos, Vector3 _endPos, float _bulletMoveTime)
    {
        particle.Play();
        StartCoroutine(ProcessSummonBullet(_gloom, _startPos, _endPos, _bulletMoveTime));

    }
    public void SetBulletRotation(Vector3 _rot)
    {
        bulletRotation = _rot;
    }

    private IEnumerator ProcessSummonBullet(GloomController _gloom, Vector3 _startPos, Vector3 _endPos, float _bulletMoveTime)
    {
        //절반씩 나눠서 기다리기

        yield return waitSec;
        GloomObstructBullet bullet = _gloom.Pool.obstructBullet.SpawnThis(_startPos, bulletRotation, null);
        bullet.Init(_gloom, _startPos, _endPos, _bulletMoveTime);
        bullet.Move();


        particle.Stop();
        yield return waitSec;

        //각도 초기화
        bulletRotation = Vector3.zero;
        _gloom.Pool.obstructSign.ReleaseThis(this);

    }
}
