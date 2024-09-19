using UnityEngine;

namespace Effects
{
    /// <summary>
    /// Changes the material of an object when the mouse is hovered over it
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class MaterialController : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        private bool _isOutlineEnabled;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnMouseEnter()
        {
            // If the player is still holding down the mouse button (i.e. holding the object), then do not select
            if(Input.GetMouseButton(0)) return;
            
            UpdateOutline(true);
        }

        private void OnMouseExit()
        {
            UpdateOutline(false);
        }

        private void OnMouseOver()
        {
            if(!_isOutlineEnabled && !Input.GetMouseButton(0))
                UpdateOutline(true);
        }

        private void OnMouseDrag()
        {
            if(_isOutlineEnabled)
                UpdateOutline(false);
        }

        /// <summary>
        /// Changes material parameters
        /// </summary>
        /// <param name="outline"></param>
        private void UpdateOutline(bool outline)
        {
            _isOutlineEnabled = outline;
            
            var mpb = new MaterialPropertyBlock();
            _spriteRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat("_Outline", outline ? 1f : 0);
            _spriteRenderer.SetPropertyBlock(mpb);
        }
    }
}
