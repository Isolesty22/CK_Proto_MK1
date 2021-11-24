using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingManager : MonoBehaviour
{
    private UIMovieScreen movieScreen;
    private UIEndingCredit endingCredit;
    private IEnumerator Start()
    {
        //�ε�â�� ����������� ���
        yield return new WaitWhile(() => SceneChanger.Instance.isLoading);
        
        //����ũ�� ��������
        movieScreen = UIManager.Instance.GetUI("UIMovieScreen") as UIMovieScreen;

        //���� ������ ���� ����
        movieScreen.onMovieEnded += MovieScreen_onMovieEnded;
    }

    private void MovieScreen_onMovieEnded()
    {
        endingCredit = UIManager.Instance.GetUI("UIEndingCredit") as UIEndingCredit;

    }
}
