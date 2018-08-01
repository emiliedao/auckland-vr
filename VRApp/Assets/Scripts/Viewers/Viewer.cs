using System;
using System.Collections.Generic;
using System.Runtime;
using SimpleFileBrowser;
using UnityEngine;
using UnityEngine.UI;

namespace Viewers
{
    public abstract class Viewer : OpenBrowser
    {
//        private string _defaultTexture;
        private const int MaxSize = 8000;
       
        void Start()
        {
            InitFileBrowser(FileType.Image);
            InitTexture();
        }

        protected abstract void InitTexture();
        
        /// <summary>
        /// Called when clicking on "Select image" button
        /// Opens a file browser allowing to choose the image set in the viewer
        /// </summary>
        public void OpenFileBrowser()
        {
            StartCoroutine(OpenBrowserCoroutine("Load Image"));
        }

        public void ValidatePopup(Canvas popup)
        {
            Debug.Log("Validate");
            popup.gameObject.SetActive(false);
            ImagePicker.Select(ActionResult);
        }

        /// <summary>
        /// Loads a texture from an image stored on the device
        /// </summary>
        /// <param name="path">Path of the image</param>
        /// <returns></returns>
        protected Texture LoadTexture(string path)
        {
            Texture texture = null;
            try
            {
                texture = NativeGallery.LoadImageAtPath(path, MaxSize);
            }

            catch (Exception e)
            {
                Debug.Log(e.Message + " Couldn't load texture from " + FileBrowser.Result);
            }
            return texture;
        }
    }
}