using System.Collections.Generic;
using SimpleFileBrowser;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace Viewers
{
    public class Viewer360 : Viewer
    {
        // Simple viewer
        public Material SphereMaterial;

        public RawImage ImagePreview;
        private static Texture _currentTexture;

    
        public void Update()
        {
            ImagePreview.texture = _currentTexture;
        }
    
        /**
     * Initializes viewers textures
     */
        protected override void InitTexture()
        {
            if (_currentTexture == null)
            {
                Texture2D texture = (Texture2D) Resources.Load("360Viewer/default360");
                SphereMaterial.mainTexture = texture;
                _currentTexture = texture;
            }
        }

        protected override void ActionResult()
        {
            _currentTexture = LoadTexture(FileBrowser.Result);
            SphereMaterial.mainTexture = _currentTexture;
        }
    }
}
