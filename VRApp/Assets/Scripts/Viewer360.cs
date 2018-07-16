using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using SimpleFileBrowser;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class Viewer360 : MonoBehaviour
{
    private int _maxSize = 8000;
    private bool _stereo;

    // Simple viewer
    public Material SphereMaterial;
    private static Texture _currentTexture;
    private Viewer _simpleViewer;
    
    // Stereo viewer
    public Material LeftSphereMaterial;
    public Material RightSphereMaterial;
    public Toggle TopBottomToggle;
    public Toggle LeftRightToggle;
//    public Toggle MpoToggle;
    
    private static StereoToggle _activeToggle = StereoToggle.TopBottom;
    private static Texture _currentStereoTexture;
    public static bool Mpo;
    private static Texture[] _stereoTextures;
    private Viewer _stereoViewer;

    public enum StereoToggle
    {
        LeftRight,
        TopBottom,
        Mpo
    };
    
    public void Start()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"));
        FileBrowser.HideDialog();
        
         _simpleViewer = new Viewer("360Viewer", new List<Material>() { SphereMaterial }, "360Viewer/default360", _maxSize);
        _stereoViewer = new Viewer("360StereoViewer", new List<Material>() { LeftSphereMaterial, RightSphereMaterial}, "360Viewer/stereoPanorama", _maxSize);
        
        InitTextures();
    }

    /**
     * Updates the toggle value and adjusts the settings of the left and right spheres according to the image format
     */
    public void Update()
    {
        // Update toggles 
        TopBottomToggle.isOn = (_activeToggle == StereoToggle.TopBottom);
        LeftRightToggle.isOn = (_activeToggle == StereoToggle.LeftRight);
//        MpoToggle.isOn = (_activeToggle == StereoToggle.Mpo);
        
        UpdateTextureSettings();
    }

    public static void OnToggleChange(StereoToggle toggle)
    {
        _activeToggle = toggle;
//        Mpo = (_activeToggle == StereoToggle.Mpo);
        Debug.Log("active toggle = " + toggle);
    }
    
    /**
     * Initializes viewers textures
     */
    private void InitTextures()
    {
        // Simple viewer
        if (_currentTexture == null)
        {
            _currentTexture = _simpleViewer.InitTexture();
        }

        if (_currentStereoTexture == null && _stereoTextures == null)
        {
            Debug.Log("init simple current stereo texture");
            _currentStereoTexture = _stereoViewer.InitTexture();
        }
    }

    /**
     * Modifies offset and tiling properties for the left and right sphere materials
     * The tiling property is the same for both spheres,
     * whereas the offset is only modified for the left sphere
     */
    private void SetTextureSettings(float offsetX, float offsetY, float tilingX, float tilingY)
    {
        // Vertical/horizontal offset
        LeftSphereMaterial.mainTextureOffset = new Vector2(offsetX, offsetY);
        
        // Tiling property: texture is repeated half a time in the vertical/horizontal direction
        LeftSphereMaterial.mainTextureScale = new Vector2(tilingX, tilingY);
        RightSphereMaterial.mainTextureScale = new Vector2(tilingX, tilingY);
    }

    /**
     * Updates sphere texture settings according to the selected format (top/bottom, left/right, mpo)
     */
    private void UpdateTextureSettings()
    {
        // Settings for Up/Down pair images
        if (_activeToggle == StereoToggle.TopBottom)
        {
            SetTextureSettings(0, 0.5f, 1, 0.5f);
        }
        
        // Settings for Left/Right pair images
        else if (_activeToggle == StereoToggle.LeftRight)
        {
            SetTextureSettings(0.5f, 0, 0.5f, 1);  
        }

        // Settings for MPO file
//        else if (_activeToggle == StereoToggle.Mpo)
//        {
//            SetTextureSettings(0, 0, 1, 1);
//        }
    }

    /**
     * Called when clicking on "Select image" button
     * Opens a file browser allowing to choose the image set in the viewer
     */
    public void OpenFileBrowser(bool stereo)
    {
        _stereo = stereo;
        StartCoroutine(OpenBrowserCoroutine());
    }
    
    
    /**
     * Shows a load file dialog and waits for a response from user
     * Loads file/folder: file, Initial path: default (Documents), Title: "Load File", submit button text: "Load"in
     */
    IEnumerator OpenBrowserCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(false, null, "Load Image", "OK");
        Debug.Log(FileBrowser.Success + " " + FileBrowser.Result);
        if (!FileBrowser.Success) yield break;

        var viewer = _stereo ? _stereoViewer : _simpleViewer;
        if (_stereo)
        {
            string path = FileBrowser.Result;
            Debug.Log(path);
//            string extension = Path.GetExtension(path);
//            Debug.Log("extension: " + extension);
//            if (extension == ".MPO" || extension == ".mpo")
//            {
//                Mpo = true;
//                var textures = _stereoViewer.ReplaceMpoTextures(path);
//                _stereoTextures = new Texture[2];
//                _stereoTextures[0] = textures[0];
//                _stereoTextures[1] = textures[1];
//            }
//
//            else
//            {
//                Mpo = false;
                _currentStereoTexture = viewer.ReplaceTexture(FileBrowser.Result);    
//            }
        }

        else
        {
            _currentTexture = viewer.ReplaceTexture(FileBrowser.Result);
        }
        ScenesManager.Load(viewer.SceneName);
    }


}
