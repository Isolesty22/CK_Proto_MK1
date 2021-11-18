using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPEffector : MonoBehaviour
{

    public BoxCollider boxCollider;
    public Vector3 effectVector;

    private const float cameraShakeValue = 0.5f;

    private IEnumerator CoGrow()
    {
        GameManager.instance.cameraManager.AddShakeValue(cameraShakeValue);
        GameManager.instance.cameraManager.AddShakeValue(-cameraShakeValue);
        yield break;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagName.Player))
        {
            GameManager.instance.playerController.Val.knockBackVelocity += effectVector;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagName.Player))
        {
            GameManager.instance.playerController.Val.knockBackVelocity -= effectVector;
        }
    }
}
