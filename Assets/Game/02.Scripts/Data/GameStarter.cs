using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ó�� ������ ���� ù �ε��� �������ִ� �Ŵ���
/// </summary>
public class GameStarter : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return StartCoroutine(DataManager.Instance.Init_DataFiles());
        Debug.Log("������ ������ �ҷ��Խ��ϴ�.");

        yield return StartCoroutine(AudioManager.Instance.Init());
        Debug.Log("����� ������ �ҷ��Խ��ϴ�.");
    }

}
