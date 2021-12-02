using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialKeyBox : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer boxSprite;
    [SerializeField]
    private TextMesh textMesh;
    [SerializeField]
    private Transform tr;
    
    [Tooltip("�Ǻ��� �����Դϴ�. ������, ����, ���� �� �ϳ������մϴ�.")]
    public eDirection pivotDir;
    public void UpdatePosition(Vector2 _pos) => tr.localPosition = _pos;

    public void UpdateText(string _str) => textMesh.text = _str;

    public void SetSprite(Sprite _sprite) => boxSprite.sprite = _sprite;

}
