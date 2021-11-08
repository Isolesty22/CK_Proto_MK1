using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldDoorSelector : MonoBehaviour
{
    [Tooltip("자기 자신의 트랜스폼")]
    public RectTransform rectTransform;
    private Vector3 centerPosition;

    [Tooltip("위아래로 움직일 이미지의 트랜스폼")]
    public RectTransform selectorImageTransform;

    [Space(5)]

    [Tooltip("속도")]
    public float frequency = 1f;
    [Tooltip("정도")]
    public float magnitude = 1f;
    private void Start()
    {
        StartCoroutine(MoveUpDown());
    }
    private IEnumerator MoveUpDown()
    {
        while (true)
        {
            selectorImageTransform.position = new Vector3(
                centerPosition.x, 
                centerPosition.y * Mathf.Sin(Time.time * frequency) * magnitude, 
                centerPosition.z);

            yield return null;
        }
    }


    public void MovePosition(Vector3 _pos)
    {
        rectTransform.position = _pos;
        centerPosition = _pos;
    }

}
