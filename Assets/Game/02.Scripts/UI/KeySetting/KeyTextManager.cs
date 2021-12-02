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
    private TutorialKeyBox keyLookUp;
    private TutorialKeyBox keyCrouch;
    private TutorialKeyBox keyMoveRight;
    private TutorialKeyBox keyMoveLeft;
    private TutorialKeyBox keyJump;
    private TutorialKeyBox keyParry;
    private TutorialKeyBox keyParry_Jump;
    private TutorialKeyBox keyAttack;
    private TutorialKeyBox keyAttack_Up;
    private TutorialKeyBox keyLifeLight;
    private TutorialKeyBox keyLightFes;

    private KeyOption keyOption
    {
        get
        {
            return DataManager.Instance.currentData_settings.keySetting;
        }
    }

    private void Start()
    {
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
        }
        else
        {
            _keyBox.SetSprite(boxShortSprite);
        }

        _keyBox.UpdateText(curString);
    }
}
