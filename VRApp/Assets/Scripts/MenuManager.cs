using System.Collections.Generic;
using System.Text;
using Google.ProtocolBuffers;
using SimpleFileBrowser;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * This class is use to manage the main menu
 */
public class MenuManager : MonoBehaviour
{
    public Canvas MenuCanvas;
    private CanvasManager _menuCanvasManager;
   
    public Canvas MainCanvas;
    public Canvas Scenes3DCanvas;
    public Canvas ViewersCanvas;
    public Canvas Viewer360Canvas;
    public Canvas StereoViewer360Canvas;
    public Canvas MpoViewerCanvas;
    public Canvas VideoPlayerCanvas;
    private List<Canvas> _canvasList;

    public Canvas WarningPopupCanvas;

    public void Start()
    {
        _canvasList = new List<Canvas> { MainCanvas, Scenes3DCanvas, ViewersCanvas, Viewer360Canvas, StereoViewer360Canvas, MpoViewerCanvas, VideoPlayerCanvas };
        UpdateCanvas();
        _menuCanvasManager = MenuCanvas.GetComponent<CanvasManager>();
    }

    void Update()
    {
        // Click outside of the parent menu canvas (and no file browser open)
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && !_menuCanvasManager.PointerInside && !FileBrowser.IsOpen)
        {
            CanvasManager.FaceCamera(MenuCanvas, 700);            
        }
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
                CanvasManager.ShowCanvas(c);
                ScenesManager.CurrentMenuCanvas = c.name;
            }

            else
            {
                CanvasManager.HideCanvas(c);
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
    
    public void OpenPopup(Canvas popup)
    {
        popup.gameObject.SetActive(true);
    }
    
    public void ClosePopup(Canvas popup)
    {
        popup.gameObject.SetActive(false);
    }

}
