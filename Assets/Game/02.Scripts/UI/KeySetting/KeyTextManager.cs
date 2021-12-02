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
    private TextMesh keyLookUp;
    [SerializeField]
    private TextMesh keyCrouch;
    [SerializeField]
    private TextMesh keyMoveRight;
    [SerializeField]
    private TextMesh keyMoveLeft;
    [SerializeField]
    private TextMesh keyJump;

    [SerializeField]
    private TextMesh keyAttack;

    [SerializeField]
    private TextMesh keyLifeLight;
    [SerializeField]
    private TextMesh keyLightFes;

    private KeyOption keyOption
    {
        get
        {
            return DataManager.Instance.currentData_settings.keySetting;
        }
    }

    private void Start()
    {
        uiKeySetting.onSave += UpdateKeyText;
    }

    /// <summary>
    /// 등록된 키 텍스트를 업데이트합니다.
    /// </summary>
    private void UpdateKeyText()
    {
        ChangeKeyText(keyAttack, keyOption.attack);

        ChangeKeyText(keyLookUp, keyOption.lookUp);
        ChangeKeyText(keyCrouch, keyOption.crouch);

        ChangeKeyText(keyMoveLeft, keyOption.moveLeft);
        ChangeKeyText(keyMoveRight, keyOption.moveRight);
        ChangeKeyText(keyJump, keyOption.jump);

        ChangeKeyText(keyLifeLight, keyOption.counter);
        ChangeKeyText(keyLightFes, keyOption.ult);
    }

    private void ChangeKeyText(TextMesh _text, KeyCode _changeKeyCode)
    {
        _text.text = KeyUtil.TryConvertString(_changeKeyCode);
    }
}
