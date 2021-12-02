using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialKeyBox : MonoBehaviour
{
    public SpriteRenderer boxSprite;
    public TextMesh textMesh;
    public void UpdateText(string _str)
    {
        textMesh.text = _str;
    }

    public void SetSprite(Sprite _sprite)
    {
        boxSprite.sprite = _sprite;
    }
}
