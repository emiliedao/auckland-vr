using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject CameraMover;
    public SettingsMenu SettingsMenu;
    
    public void MoveForward(bool forward)
    {
        MoveCamera(CameraMover, Camera.main, forward);
    }

    private void MoveCamera(GameObject cameraMover, Camera cam, bool forward)
    {
        int sign = forward ? 1 : -1;
        Vector3 pos = cam.transform.position + cam.transform.forward * sign;
        StartCoroutine(MoveToPosition(cameraMover, cam.transform.position, pos));
    }
    
    private IEnumerator MoveToPosition(GameObject cameraMover, Vector3 startPosition, Vector3 endPosition)
    {
        float elapsedTime = 0;
        float time = 1f;
        
        while (elapsedTime < time)
        {
            float a = elapsedTime / time;
            cameraMover.transform.position = Vector3.Lerp(startPosition, endPosition, a);
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }   
        SettingsMenu.Close();
    }

    public void CameraMoverForward(GameObject cameraMover)
    {
        MoveCamera(cameraMover, cameraMover.GetComponentInChildren<Camera>(), true);
    }

    public void CameraMoverBackward(GameObject cameraMover)
    {
        MoveCamera(cameraMover, cameraMover.GetComponentInChildren<Camera>(), false);
    }
}
