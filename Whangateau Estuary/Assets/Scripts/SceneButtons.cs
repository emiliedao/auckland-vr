using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class SceneButtons : MonoBehaviour
{
    // VR disabled
    private const int ZOOM_OFFSET = 10;
    private const int ZOOM_MAX = 10;
    private static int zoom = 0;
    
    // VR enabled
    private const float VR_ZOOM = 0.1f;
    private const float VR_ZOOM_MAX = 3f;
	
    void Update() {
        // Android back button
        if (Input.GetKeyDown(KeyCode.Escape)) {
            LoadMenu();
        }
    }

    /**
     * Handles click on back button
     */
    public void BackButton()
    {
        LoadMenu();
    }
    
    void LoadMenu()
    {
        ResetZoom();
        SceneManager.LoadScene("VRMenuScene");
    }

    /**
     * Handles zooming buttons (in and out)
     */
    public void ZoomButton(int sign)
    {       
        // Changing camera's transform won't work when VR is enabled, thus fovZoomFactor must be used (note that it cannot be set under 1.0)
        if (XRSettings.enabled)
        {
            if (ZoomAllowed(XRDevice.fovZoomFactor, VR_ZOOM_MAX, sign))
            {
                XRDevice.fovZoomFactor += sign * VR_ZOOM;    
            }  	
        }

        else
        {
            if (ZoomAllowed(zoom, ZOOM_MAX, sign))
            {
                zoom += sign;
                zoomObject(Camera.main.transform, 0, 0, sign * ZOOM_OFFSET);
                zoomObject(transform.parent, 0, 0, sign * ZOOM_OFFSET);
            }
        }
    }

    /**
     * Returns true if zoom is allowed
     */
    bool ZoomAllowed(float zoomValue, float zoomMax, int sign)
    {
        return ((sign == 1 && zoomValue < zoomMax) || (sign == -1 && zoomValue > -zoomMax));
    }


    /**
     * Operates a zoom on a game object by changing its transform
     */
    private void zoomObject(Transform transform, int offsetX, int offsetY, int offsetZ)
    {
        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x + offsetX, pos.y + offsetY, pos.z + offsetZ);
    }

    /**
     * Resets VR zoom
     */
    private void ResetZoom()
    {
        if (XRSettings.enabled)
        {
            XRDevice.fovZoomFactor = 1;
        }
    }

}