using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ImagePicker {
    
    private static int _maxSize = 2000;

    public static void Test(string path)
    {
            Debug.Log( "Image path: " + path );
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, _maxSize);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                }
            }
//        }
    }

    public static void Select(NativeGallery.MediaPickCallback callback)
    {
        // Checks that another media pick operation is not already in progress
        if (!NativeGallery.IsMediaPickerBusy())
        {
            NativeGallery.Permission permission = NativeGallery.GetImageFromGallery(callback, "Select a PNG image", "image/png", _maxSize);

            Debug.Log("Permission result: " + permission);
        }
    }
}
