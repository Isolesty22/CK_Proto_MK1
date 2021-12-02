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
    
    [SerializeField]
    private Transform tr_parent;

    [Tooltip("�Ǻ��� �����Դϴ�. ������, ���� �� �ϳ������մϴ�.")]
    public eDirection pivotDir;
    public void UpdatePosition(Vector2 _pos)
    {
        tr.position = _pos;
    }
    public void UpdateText(string _str)
    {
        textMesh.text = _str;
    }

    public void SetSprite(Sprite _sprite)
    {
        boxSprite.sprite = _sprite;
    }
}
