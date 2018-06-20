using System.Collections;
using System.IO;
using UnityEngine;
using SimpleFileBrowser;

public class Viewer360 : MonoBehaviour
{
    private static int _maxSize = 2000;
    public Material SphereMaterial;
    private static Texture _currentTexture;
    private string _defaultTexture = "360Viewer/stereo_left";

    public void Start()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"));
        FileBrowser.HideDialog();
        InitTexture();
    }

    public void InitTexture()
    {
        if (_currentTexture == null)
        {
            SphereMaterial.mainTexture = (Texture2D) Resources.Load(_defaultTexture);
            _currentTexture = SphereMaterial.mainTexture;    
        }
    }
    
    public void PickImageFromGallery()
    {
        ImagePicker.Select(ReplaceViewerImg);
    }

    public void OpenFileBrowser()
    {
        StartCoroutine(ShowLoadDialogCoroutine());
    }
    
    
    IEnumerator ShowLoadDialogCoroutine()
    {
        // Show a load file dialog and wait for a response from user
        // Load file/folder: file, Initial path: default (Documents), Title: "Load File", submit button text: "Load"
        yield return FileBrowser.WaitForLoadDialog(false, null, "Load File", "Load");

        // Dialog is closed
        // Print whether a file is chosen (FileBrowser.Success)
        // and the path to the selected file (FileBrowser.Result) (null, if FileBrowser.Success is false)
        Debug.Log( FileBrowser.Success + " " + FileBrowser.Result );
        if (FileBrowser.Success)
        {
            ReplaceViewerImg(FileBrowser.Result);
            ScenesManager.Load("360Viewer");
        }
    }

    private void ReplaceViewerImg(string path)
    {
        if (path != null)
        {
            // Create Texture from selected image
            Texture2D texture = NativeGallery.LoadImageAtPath(path, _maxSize);
            
            if (texture == null)
            {
                Debug.Log("Couldn't load texture from " + path);
            }

            else
            {
                SphereMaterial.mainTexture = texture;
                ScenesManager.Load("360Viewer");
            }
        }
    }

}
