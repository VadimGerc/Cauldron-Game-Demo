using UnityEngine;

namespace UI
{
    /// <summary>
    /// Keeps the aspect ratio constant
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class AspectRatioManager : MonoBehaviour
    {
        private Camera _camera;
    
        private void Start()
        {
            _camera = GetComponent<Camera>();
            CalculateAspectRatio();
        }

        private void Update()
        {
#if UNITY_EDITOR
            CalculateAspectRatio();
#endif
        }

        private void CalculateAspectRatio()
        {
            const float targetAspectRatio = 16f / 9f;
            var currentAspectRatio = Screen.width / (float) Screen.height;
            var scaleHeight = currentAspectRatio / targetAspectRatio;

            if (scaleHeight < 1.0f)
            {
                var rect = _camera.rect;

                rect.width = 1.0f;
                rect.height = scaleHeight;
                rect.x = 0;
                rect.y = (1.0f - scaleHeight) / 2.0f;

                _camera.rect = rect;
            }
            else
            {
                var scaleWidth = 1.0f / scaleHeight;

                var rect = _camera.rect;

                rect.width = scaleWidth;
                rect.height = 1.0f;
                rect.x = (1.0f - scaleWidth) / 2.0f;
                rect.y = 0;

                _camera.rect = rect;
            }
        }
    }
}
