using UnityEngine;
using UnityEngine.UI;

namespace PacMan.UI
{
    /// <summary>
    /// Optimizes UI elements for VR viewing by adjusting scale, position, and readability
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class VRUIOptimizer : MonoBehaviour
    {
        [Header("VR UI Settings")]
        [SerializeField] private float vrScaleFactor = 0.001f;
        [SerializeField] private float vrDistance = 1.5f;
        [SerializeField] private float vrHeightOffset = 0f;
        [SerializeField] private bool autoAdjustForVR = true;
        
        [Header("Text Optimization")]
        [SerializeField] private int vrFontSizeMultiplier = 2;
        [SerializeField] private float vrLineSpacing = 1.2f;
        [SerializeField] private bool useOutlineEffect = true;
        [SerializeField] private Color outlineColor = Color.black;
        [SerializeField] private float outlineWidth = 2f;
        
        [Header("Panel Optimization")]
        [SerializeField] private float panelDepthOffset = 0.1f;
        [SerializeField] private bool useWorldSpace = true;
        
        private Canvas _canvas;
        private Camera _vrCamera;
        
        private void Start()
        {
            _canvas = GetComponent<Canvas>();
            _vrCamera = Camera.main;
            
            if (autoAdjustForVR)
            {
                OptimizeForVR();
            }
        }
        
        /// <summary>
        /// Optimize all UI elements for VR viewing
        /// </summary>
        public void OptimizeForVR()
        {
            if (_canvas == null) return;
            
            // Adjust canvas for VR
            AdjustCanvasForVR();
            
            // Adjust all text elements
            AdjustTextElements();
            
            // Adjust all panels
            AdjustPanels();
            
            // Position UI in front of player
            PositionUIInFrontOfPlayer();
        }
        
        /// <summary>
        /// Adjust canvas settings for VR
        /// </summary>
        private void AdjustCanvasForVR()
        {
            if (_canvas == null) return;
            
            // Set canvas to world space for VR
            if (useWorldSpace)
            {
                _canvas.renderMode = RenderMode.WorldSpace;
                _canvas.worldCamera = _vrCamera;
            }
            
            // Scale the canvas for proper VR sizing
            transform.localScale = Vector3.one * vrScaleFactor;
        }
        
        /// <summary>
        /// Adjust all text elements for better VR readability
        /// </summary>
        private void AdjustTextElements()
        {
            // Find all Text components in children
            Text[] textElements = GetComponentsInChildren<Text>(true);
            
            foreach (Text text in textElements)
            {
                // Increase font size for VR
                text.fontSize *= vrFontSizeMultiplier;
                
                // Adjust line spacing
                text.lineSpacing = vrLineSpacing;
                
                // Add outline effect for better visibility
                if (useOutlineEffect)
                {
                    AddOutlineEffect(text);
                }
            }
        }
        
        /// <summary>
        /// Add outline effect to text for better visibility in VR
        /// </summary>
        /// <param name="text">Text component to add outline to</param>
        private void AddOutlineEffect(Text text)
        {
            // Check if outline effect already exists
            Outline outline = text.GetComponent<Outline>();
            if (outline == null)
            {
                outline = text.gameObject.AddComponent<Outline>();
            }
            
            outline.effectColor = outlineColor;
            outline.effectDistance = new Vector2(outlineWidth, outlineWidth);
        }
        
        /// <summary>
        /// Adjust panels for better VR depth perception
        /// </summary>
        private void AdjustPanels()
        {
            // Find all UI panels (GameObject with Image components)
            Image[] panels = GetComponentsInChildren<Image>(true);
            
            // We'll adjust the z-position of panels to create depth
            float currentDepth = 0f;
            
            foreach (Image panel in panels)
            {
                // Skip the main canvas background
                if (panel == GetComponent<Image>()) continue;
                
                // Adjust z-position for depth
                RectTransform rectTransform = panel.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    Vector3 localPosition = rectTransform.localPosition;
                    localPosition.z = currentDepth;
                    rectTransform.localPosition = localPosition;
                    
                    currentDepth -= panelDepthOffset;
                }
            }
        }
        
        /// <summary>
        /// Position UI in front of the player
        /// </summary>
        private void PositionUIInFrontOfPlayer()
        {
            if (_vrCamera == null) return;
            
            // Position the UI in front of the player at the specified distance
            Vector3 forward = _vrCamera.transform.forward;
            forward.y = 0f; // Keep UI level
            forward.Normalize();
            
            Vector3 targetPosition = _vrCamera.transform.position + forward * vrDistance;
            targetPosition.y += vrHeightOffset;
            
            transform.position = targetPosition;
            
            // Rotate UI to face the player
            transform.LookAt(_vrCamera.transform);
            transform.Rotate(0, 180, 0); // Correct orientation
        }
        
        /// <summary>
        /// Set the VR scale factor
        /// </summary>
        /// <param name="scale">Scale factor for VR UI</param>
        public void SetVRScaleFactor(float scale)
        {
            vrScaleFactor = scale;
            if (autoAdjustForVR)
            {
                AdjustCanvasForVR();
            }
        }
        
        /// <summary>
        /// Set the VR distance from player
        /// </summary>
        /// <param name="distance">Distance from player</param>
        public void SetVRDistance(float distance)
        {
            vrDistance = distance;
            if (autoAdjustForVR)
            {
                PositionUIInFrontOfPlayer();
            }
        }
        
        /// <summary>
        /// Set the VR height offset
        /// </summary>
        /// <param name="offset">Height offset</param>
        public void SetVRHeightOffset(float offset)
        {
            vrHeightOffset = offset;
            if (autoAdjustForVR)
            {
                PositionUIInFrontOfPlayer();
            }
        }
        
        /// <summary>
        /// Get whether auto-adjust for VR is enabled
        /// </summary>
        /// <returns>True if auto-adjust is enabled</returns>
        public bool IsAutoAdjustEnabled()
        {
            return autoAdjustForVR;
        }
        
        /// <summary>
        /// Set whether to auto-adjust for VR
        /// </summary>
        /// <param name="autoAdjust">True to auto-adjust</param>
        public void SetAutoAdjust(bool autoAdjust)
        {
            autoAdjustForVR = autoAdjust;
        }
    }
}