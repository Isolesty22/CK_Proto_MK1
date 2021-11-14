using UnityEditor;
using UnityEditor.SceneManagement;

public class StartLoadingScene : EditorWindow
{

	//��ܹ� �޴� ����
	[MenuItem("�׽�Ʈ �÷���/Ÿ��Ʋ������ ����")]

	public static void PlayLoadingScene()
	{
		//���������� ������ �����Ұųİ� ���
		if (EditorSceneManager.GetActiveScene().isDirty)
		{
			EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		}

		EditorSceneManager.OpenScene("Assets/Game/01.Scenes/InGame/ETC/Title.unity");
		EditorApplication.isPlaying = true;
	}
}
