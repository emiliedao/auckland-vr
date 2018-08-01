using SimpleFileBrowser;
using UnityEngine;
using UnityEngine.UI;

namespace Viewers
{
    public class StereoViewer360 : Viewer
    {
        public Material LeftSphereMaterial;
        public Material RightSphereMaterial;
        
        public Toggle TopBottomToggle;
        public Toggle LeftRightToggle;

        public RawImage ImagePreview;
        
        private static Texture _currentTexture;
        
        public enum StereoToggle
        {
            LeftRight,
            TopBottom,
        };
        
        private static StereoToggle _activeToggle = StereoToggle.TopBottom;
        
        protected override void InitTexture()
        {
            if (_currentTexture == null)
            {
                Texture2D texture = (Texture2D) Resources.Load("360Viewer/stereoPanorama");
                LeftSphereMaterial.mainTexture = texture;
                RightSphereMaterial.mainTexture = texture;
                _currentTexture = texture;
            }
        }
        
        protected override void ActionResult(string path)
        {
            Debug.Log(path);
            _currentTexture = LoadTexture(path);
            LeftSphereMaterial.mainTexture = _currentTexture;
            RightSphereMaterial.mainTexture = _currentTexture;
        }


        /// <summary>
        /// Updates the toggle value and adjusts the settings of the left and right spheres according to the image format
        /// </summary>
        void Update()
        {
            TopBottomToggle.isOn = (_activeToggle == StereoToggle.TopBottom);
            LeftRightToggle.isOn = (_activeToggle == StereoToggle.LeftRight);
            
            UpdateTextureSettings();
            ImagePreview.texture = _currentTexture;
        }
        
        /// <summary>
        /// Updates sphere texture settings according to the selected format (top/bottom, left/right)
        /// </summary>
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
        }
        
        /// <summary>
        /// Modifies offset and tiling properties for the left and right sphere materials
        /// The tiling property is the same for both spheres, whereas the offset is only modified for the left sphere
        /// </summary>
        /// <param name="offsetX">Horizontal offset</param>
        /// <param name="offsetY">Vertical offset</param>
        /// <param name="tilingX">Number of times the texture is repeated in the horizontal direction</param>
        /// <param name="tilingY">Number of time the texture is repeated in the vertical direction</param>
        private void SetTextureSettings(float offsetX, float offsetY, float tilingX, float tilingY)
        {
            // Vertical/horizontal offset
            LeftSphereMaterial.mainTextureOffset = new Vector2(offsetX, offsetY);
        
            // Tiling property: texture is repeated half a time in the vertical/horizontal direction
            LeftSphereMaterial.mainTextureScale = new Vector2(tilingX, tilingY);
            RightSphereMaterial.mainTextureScale = new Vector2(tilingX, tilingY);
        }

        public static void OnToggleChange(StereoToggle toggle)
        {
            _activeToggle = toggle;
        }
    }
}