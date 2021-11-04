using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomObstructBullet : MonoBehaviour
{


    [Tooltip("투사체가 생성된 후 waitTime만큼 대기 후 이동을 시작합니다.")]
    public float waitTime;
    [Tooltip("투사체가 맵 끝으로 이동할 때까지 걸리는 시간입니다.")]
    public float moveTime;

    //[HideInInspector]
    //public GloomController gloom;
    private AnimationCurve curve;

    public Transform myTransform;

    private Vector3 startPos;
    private Vector3 endPos;

    private PlayerController player;

    private void Start()
    {
        player = GameManager.instance.playerController;
    }
    public void Init(GloomController _gloom ,Vector3 _startPos,Vector3 _endPos)
    {
        curve = _gloom.SkillVal.obstruct.curve;
        startPos = _startPos;
        endPos = _endPos;
    }

    public void Move()
    {
        StartCoroutine(ProcessMove());
    }

    private IEnumerator ProcessMove()
    {

        yield return new WaitForSeconds(waitTime);

        float timer = 0f;
        float progress = 0f;
        while (true)
        {
            timer += Time.deltaTime;
            progress = timer / moveTime;
            myTransform.position = Vector3.Lerp();
        }
    }
    public void Despawn()
    {

    }

    private IEnumerator ProcessDespawn()
    {

    }    

    
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag(TagName.Player))
        {

        }
    }
}
