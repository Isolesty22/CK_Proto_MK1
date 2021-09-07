using UnityEditor;
using UnityEditor.SceneManagement;

public class StartLoadingScene : EditorWindow
{

	//��ܹ� �޴� ����
	[MenuItem("�׽�Ʈ �÷���/Loading������ ����")]

	public static void PlayLoadingScene()
	{
		//���������� ������ �����Ұųİ� ���
		if (EditorSceneManager.GetActiveScene().isDirty)
		{
			EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		}

		EditorSceneManager.OpenScene("Assets/Game/01.Scenes/InGame/ETC/Loading.unity");
		EditorApplication.isPlaying = true;
	}
}
