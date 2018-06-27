using UnityEngine;

/**
 * This class allows to use a native Android plugin to pick up a picture from the smartphone gallery
 * 
 * The plugin works well in 2D, but the open gallery pop-up window cannot be rendered in VR
 * See SimpleFileBrowser plugin instead with OpenFileBrowser function
 *
 * If you still wish to use ImagePicker, create a MonoBehaviour script attached to your game object,
 * and use "ImagePicker.Select(callback)" where @callback (or name it what you want) is another function in your script: "public void callback(string path) { ... }"
 * This callback function will be called after the image is selected 
 */
public static class ImagePicker {
    
    /*private static int _maxSize = 2000;

    public static void Select(NativeGallery.MediaPickCallback callback)
    {
        // Checks that another media pick operation is not already in progress
        if (!NativeGallery.IsMediaPickerBusy())
        {
            NativeGallery.Permission permission = NativeGallery.GetImageFromGallery(callback, "Select a PNG image", "image/png", _maxSize);

            Debug.Log("Permission result: " + permission);
        }
    }*/
}
