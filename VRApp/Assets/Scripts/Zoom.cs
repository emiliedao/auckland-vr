using UnityEngine;
using UnityEngine.XR;

/**
 * This class is used to manage zoom in VR or 2D environments
 */
public static class Zoom {
    private static float _factor = XRDevice.fovZoomFactor;
    public static float Factor
    {
        get { return _factor; }
        set { _factor = value; }
    }

    private static int _2DZoomOffset = 50;

    private static float _settingsMenuScale;
    public static float SettingsMenuScale
    {
        get { return _settingsMenuScale; }
        set { _settingsMenuScale = value; }
    }

    /**
     * Resets VR zoom
     */
    public static void ResetZoom()
    {
        _factor = 1;
        XRDevice.fovZoomFactor = 1f;
        _settingsMenuScale = SettingsMenu.DefaultScale;
    }
	    	
    /**
     * Updates 2D zoom if VR is disabled
     */
    public static void Update2DZoom()
    {
        if (!XRSettings.enabled)
        {
            Vector3 pos = Camera.main.transform.position;
//            Debug.Log("vr zoom factor" + _factor);
//            Debug.Log("cam pos before" + pos.z);

            Camera.main.transform.position = new Vector3(pos.x, pos.y, Get2DZoom());
//            Debug.Log("cam pos afer " + Camera.main.transform.position.z);
        }
    }
    

    /**
     * Returns a z-axis value corresponding to the current zoom factor
     * This is used to zoom in non-VR scene
     */
    private static float Get2DZoom()
    {
        Vector3 pos = Camera.main.transform.position;
        float z = pos.z;
		
        if ((int) pos.z == 0 && _factor != 1)
        {
            z = _2DZoomOffset * _factor;
        }
		
        else
        {
            z = z * _2DZoomOffset * _factor;
        }

        return z;
    }
	
}
