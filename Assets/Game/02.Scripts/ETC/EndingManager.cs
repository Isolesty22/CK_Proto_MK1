using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EndingManager : MonoBehaviour
{
    private UIMovieScreen movieScreen;
    private UIEndingCredit endingCredit;

    public PlayableDirector director;
    private IEnumerator Start()
    {
        //�ε�â�� ����������� ���
        yield return new WaitWhile(() => SceneChanger.Instance.isLoading);

        yield return null;
        //����ũ�� ��������
        movieScreen = UIManager.Instance.GetUI("UIMovieScreen") as UIMovieScreen;
        if (AudioManager.Instance.Audios.audioSource_BGM.isPlaying)
            AudioManager.Instance.Audios.audioSource_BGM.Stop();
        movieScreen.onMovieEnded += MovieScreen_onMovieEnded;
        StartCoroutine(movieScreen.playMovie);
    }

    private void MovieScreen_onMovieEnded()
    {
        movieScreen.onMovieEnded -= MovieScreen_onMovieEnded;
        movieScreen.waitingPanel.SetActive(false);
        //��������
        movieScreen.Close();

        endingCredit = UIManager.Instance.GetUI("UIEndingCredit") as UIEndingCredit;
        endingCredit.StartCoroutine(endingCredit.Up());

        AudioClip tempClip = null;

        if (AudioManager.Instance.clipDict_BGM.TryGetValue("Stage1BGM", out tempClip))
        {
            AudioManager.Instance.Audios.audioSource_BGM.clip = tempClip;
            AudioManager.Instance.Audios.audioSource_BGM.Play();
        }

        StartCoroutine(CoStartTimeline());

        endingCredit.onCreditMoveEnded += EndingCredit_onCreditMoveEnded;
    }

    private IEnumerator CoStartTimeline()
    {
        yield return new WaitForSeconds(3f);

        director.Play();
    }
    private IEnumerator OnCreditMoveEnded()
    {
        yield return new WaitForSeconds(2f);
        SceneChanger.Instance.LoadThisScene(SceneNames.mainMenu);
    }
    private void EndingCredit_onCreditMoveEnded()
    {
        endingCredit.onCreditMoveEnded -= EndingCredit_onCreditMoveEnded;
        StartCoroutine(OnCreditMoveEnded());
    }
}
