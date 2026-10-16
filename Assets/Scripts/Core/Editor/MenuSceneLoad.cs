using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MenuSceneLoad : MonoBehaviour
{

    [MenuItem("Scenes/Start Scene")]
    static void OpenStartScene()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/_StartScreen.unity");
    }

    [MenuItem("Scenes/Main Menu")]
    static void OpenMainMenu()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
    }

    [MenuItem("Scenes/Level Scene")]
    static void OpenGameScene()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/GameScene.unity");
    }
}
