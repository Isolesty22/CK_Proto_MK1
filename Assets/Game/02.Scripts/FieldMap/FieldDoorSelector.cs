using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldDoorSelector : MonoBehaviour
{
    [Tooltip("�ڱ� �ڽ��� Ʈ������")]
    public RectTransform rectTransform;
    private Vector3 centerPosition;

    [Tooltip("���Ʒ��� ������ �̹����� Ʈ������")]
    public RectTransform selectorImageTransform;

    [Space(5)]

    [Tooltip("�ӵ�")]
    public float frequency = 1f;
    [Tooltip("����")]
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
