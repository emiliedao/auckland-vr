using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * This class is use to manage the VR Menu scene
 */
public class MenuManager : MonoBehaviour
{
    /*
     * Loads a new scene using its name
     * The scene name is given in parameter in Unity 
     */
    public void LoadByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /*
     * Quits the application
     */
    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Debug.Log("Quit");
            Application.Quit();
            #endif
    }
}
