using System.Collections.Generic;
using System.Text;
using Google.ProtocolBuffers;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * This class is use to manage the main menu
 */
public class MenuManager : MonoBehaviour
{
    public Canvas MainCanvas;
    public Canvas Scenes3DCanvas;
    public Canvas ViewersCanvas;
    public Canvas Viewer360Canvas;
    public Canvas StereoViewer360Canvas;
    public Canvas VideoPlayerCanvas;
    private List<Canvas> _canvasList;

    public void Start()
    {
        _canvasList = new List<Canvas> {MainCanvas, Scenes3DCanvas, ViewersCanvas, Viewer360Canvas, StereoViewer360Canvas, VideoPlayerCanvas };
        UpdateCanvas();
    }

    /**
     * Displays a menu canvas
     */
    public void ShowCanvas(string canvasName)
    {
        foreach (var c in _canvasList)
        {
            if (canvasName == c.name)
            {
                ScenesManager.ShowCanvas(c);
                ScenesManager.CurrentMenuCanvas = c.name;
            }

            else
            {
                ScenesManager.HideCanvas(c);
            }   
        }    
    }

    /**
     * Shows the correct current menu canvas
     */
    private void UpdateCanvas()
    {
        if (ScenesManager.CurrentMenuCanvas == null)
        {
            ScenesManager.CurrentMenuCanvas = MainCanvas.name;
        }
        ShowCanvas(ScenesManager.CurrentMenuCanvas);
    }
    
    /*
     * Loads a new scene using its name
     * The scene name is given in parameter in Unity 
     */
    public void LoadByName(string sceneName)
    {
        ScenesManager.Load(sceneName);
    }
    
    public void QuitScene()
    {
        ScenesManager.LoadMenu();
    }

    public void QuitApp()
    {
        ScenesManager.Quit();
    }

}
