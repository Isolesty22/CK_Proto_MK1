using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyTextManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("키세팅 UI. 이벤트 설정 용으로 사용함")]
    private UIKeySetting uiKeySetting;

    [SerializeField]
    private Sprite boxLongSprite;
    [SerializeField]
    private Sprite boxShortSprite;

    [Space(5)]

    [SerializeField]
    private TutorialKeyBox keyLookUp;
    [SerializeField]
    private TutorialKeyBox keyCrouch;
    [SerializeField]
    private TutorialKeyBox keyMoveRight;
    [SerializeField]
    private TutorialKeyBox keyMoveLeft;
    [SerializeField]
    private TutorialKeyBox keyJump;
    [SerializeField]
    private TutorialKeyBox keyParry;
    [SerializeField]
    private TutorialKeyBox keyParry_Jump;
    [SerializeField]
    private TutorialKeyBox keyAttack;
    [SerializeField]
    private TutorialKeyBox keyAttack_Up;
    [SerializeField]
    private TutorialKeyBox keyLifeLight;
    [SerializeField]
    private TutorialKeyBox keyLightFes;

    private KeyOption keyOption
    {
        get
        {
            return DataManager.Instance.currentData_settings.keySetting;
        }
    }

    private readonly Vector2 boxLongPos_right = new Vector2(-0.85f, 0f);
    private readonly Vector2 boxLongPos_left = new Vector2(-0.85f, 0f);
    private readonly Vector2 boxShortPos_right = new Vector2(-0.3f, 0f);
    private readonly Vector2 boxShortPos_left = new Vector2(-0.3f, 0f);


    private void Start()
    {
        UpdateAllKeyBox();
        uiKeySetting.onSave += UpdateAllKeyBox;
    }

    /// <summary>
    /// 등록된 키박스를 전부 업데이트합니다.
    /// </summary>
    private void UpdateAllKeyBox()
    {
        UpdateKeyBox(keyAttack, keyOption.attack);
        UpdateKeyBox(keyAttack_Up, keyOption.attack);

        UpdateKeyBox(keyLookUp, keyOption.lookUp);
        UpdateKeyBox(keyCrouch, keyOption.crouch);

        UpdateKeyBox(keyMoveLeft, keyOption.moveLeft);
        UpdateKeyBox(keyMoveRight, keyOption.moveRight);
        UpdateKeyBox(keyJump, keyOption.jump);
        UpdateKeyBox(keyParry, keyOption.jump);
        UpdateKeyBox(keyParry_Jump, keyOption.jump);

        UpdateKeyBox(keyLifeLight, keyOption.counter);
        UpdateKeyBox(keyLightFes, keyOption.ult);
    }

    private void UpdateKeyBox(TutorialKeyBox _keyBox, KeyCode _changeKeyCode)
    {
        string curString = KeyUtil.TryConvertString(_changeKeyCode);

        if (curString.Length > 1)
        {
            _keyBox.SetSprite(boxLongSprite);
            if (_keyBox.pivotDir == eDirection.Left)
            {
                _keyBox.UpdatePosition(boxLongPos_left);
            }
            else
            {
                _keyBox.UpdatePosition(boxLongPos_right);
            }
        }
        else
        {
            _keyBox.SetSprite(boxShortSprite);
            if (_keyBox.pivotDir == eDirection.Left)
            {
                _keyBox.UpdatePosition(boxShortPos_left);
            }
            else
            {
                _keyBox.UpdatePosition(boxShortPos_right);
            }
        }

        _keyBox.UpdateText(curString);
    }
}
