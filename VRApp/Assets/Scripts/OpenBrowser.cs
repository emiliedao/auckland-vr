using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SimpleFileBrowser;
using UnityEngine;

/// <summary>
/// This class can be inherited from any class that needs to use a file browser 
/// </summary>
public abstract class OpenBrowser : MonoBehaviour
{
    /// <summary>
    /// This function must be overrided to implement a behaviour after loading a file through the file browser 
    /// </summary>
    protected abstract void ActionResult(string path);

    protected enum FileType
    {
        Image, Mpo, Object, Video
    }

    /// <summary>
    /// Initializes the file browser according to the type of file that must be selected 
    /// </summary>
    /// <param name="type"></param>
    protected void InitFileBrowser(FileType type)
    {
        string[] extensions = null;
        string typeName = "";
        
        switch (type)
        {
            case FileType.Image:
                typeName = "Images";
                extensions = new string[] { ".jpg", ".png" };
                break;
            
            case FileType.Mpo:
                typeName = "Mpo";
                extensions = new string[] { ".mpo" };
                break;
                
            case FileType.Object:
                typeName = "Object";
                extensions = new string[] { ".obj" };
                break;
         
            case FileType.Video:
                typeName = "Videos";
                extensions = new string[] { ".mp4" };
                break;
        }

        FileBrowser.SetFilters(true, new FileBrowser.Filter(typeName, extensions));
        FileBrowser.HideDialog();
    }

    /// <summary>
    /// Opens a file browser dialog and waits for a response from the user 
    /// </summary>
    /// <param name="title"></param>
    /// <returns></returns>
    protected IEnumerator OpenBrowserCoroutine(string title)
    {
        yield return FileBrowser.WaitForLoadDialog(false, null, title, "OK");
        Debug.Log(FileBrowser.Success + " " + FileBrowser.Result);
        
        if (!FileBrowser.Success) yield break;
        ActionResult(FileBrowser.Result);
    }
}
