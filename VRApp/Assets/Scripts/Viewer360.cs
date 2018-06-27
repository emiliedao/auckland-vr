using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using SimpleFileBrowser;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class Viewer360 : MonoBehaviour
{
    private int _maxSize = 2000;
    private bool _stereo;

    // Simple viewer
    public Material SphereMaterial;
    private static Texture _currentTexture;
    private Viewer _simpleViewer;
    
    // Stereo viewer
    public Material LeftSphereMaterial;
    public Material RightSphereMaterial;
    public Toggle UpDownToggle;
    public Toggle LeftRightToggle;
    private static bool _upDownToggle = true;
    private static Texture _currentStereoTexture;
    private Viewer _stereoViewer;
    
    public void Start()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"));
        FileBrowser.HideDialog();
        
         _simpleViewer = new Viewer("360Viewer", new List<Material>() { SphereMaterial }, "360Viewer/default360", 2000);
        _stereoViewer = new Viewer("360StereoViewer", new List<Material>() { LeftSphereMaterial, RightSphereMaterial}, "360Viewer/stereoPanorama", 4000);
        
        UpdateTextures();
    }

    public void Update()
    {
        UpdateToggle();
    }

    public static void OnToggleChange(bool upDown)
    {
        _upDownToggle = upDown;
    }
    
    /**
     * Updates viewers textures
     */
    private void UpdateTextures()
    {
        if (_currentTexture == null)
        {
            _currentTexture = _simpleViewer.InitTexture();
        }

        if (_currentStereoTexture == null)
        {
            _currentStereoTexture = _stereoViewer.InitTexture();
        }
    }

    /**
     * Updates the toggle value and adjusts the settings of the left and right spheres according to the image format
     */
    private void UpdateToggle()
    {
        UpDownToggle.isOn = _upDownToggle;
        LeftRightToggle.isOn = !_upDownToggle;
        
        // Settings for Up/Down pair images
        if (_upDownToggle)
        {
            SetTextureSettings(0, 0.5f, 1, 0.5f);
        }

        // Settings for Left/Right pair images
        else
        {
            SetTextureSettings(0.5f, 0, 0.5f, 1);
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
            _currentStereoTexture = viewer.ReplaceImage(FileBrowser.Result);
        }

        else
        {
            _currentTexture = viewer.ReplaceImage(FileBrowser.Result);
        }
        ScenesManager.Load(viewer.SceneName);
    }


}
