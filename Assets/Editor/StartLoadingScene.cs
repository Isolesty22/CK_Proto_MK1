using UnityEditor;
using UnityEditor.SceneManagement;

public class StartLoadingScene : EditorWindow
{

	//상단바 메뉴 생성
	[MenuItem("테스트 플레이/타이틀씬부터 시작")]

	public static void PlayLoadingScene()
	{
		//수정사항이 있으면 저장할거냐고 물어봄
		if (EditorSceneManager.GetActiveScene().isDirty)
		{
			EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		}

		EditorSceneManager.OpenScene("Assets/Game/01.Scenes/InGame/ETC/Title.unity");
		EditorApplication.isPlaying = true;
	}
}
