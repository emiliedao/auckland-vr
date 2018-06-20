using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * This class is use to manage the VR Menu scene
 */
public class MenuManager : MonoBehaviour
{
    public Canvas MainCanvas;
    public Canvas Scenes3DCanvas;
    public Canvas Viewer360Canvas;
    public Canvas ImagePickerCanvas;
    private List<Canvas> _canvasList;

    public void Start()
    {
        _canvasList = new List<Canvas> {MainCanvas, Scenes3DCanvas, Viewer360Canvas, ImagePickerCanvas}; 
        ShowCanvas(MainCanvas.name);
    }

    public void ShowCanvas(string canvasName)
    {
        foreach (var c in _canvasList)
        {
            if (canvasName == c.name)
            {
                ScenesManager.ShowCanvas(c);
            }

            else
            {
                ScenesManager.HideCanvas(c);
            }   
        }
         
    }
    
    
    /*
     * Loads a new scene using its name
     * The scene name is given in parameter in Unity 
     */
    public void LoadByName(string sceneName)
    {
        ScenesManager.Load(sceneName);
    }

    public void LoadMenu()
    {
        ScenesManager.LoadMenu();
    }
    
#region SettingsMenu
    public void LoadSettingsMenu()
    {
        ScenesManager.LoadSettingsMenu();
    }

    public void QuitSettingsMenu()
    {
        ScenesManager.QuitSettingsMenu();
    }
#endregion
    
    public void QuitScene()
    {
        ScenesManager.LoadMenu();
    }

    public void QuitApp()
    {
        ScenesManager.Quit();
    }

}
