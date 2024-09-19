using UnityEngine;

namespace Gameplay
{
    /// <summary>
    /// Allows moving an object with the mouse
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class DraggableObject : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// Reacts when a player clicked on the sprite (when the item is already spawned)
        /// </summary>
        private void OnMouseDown()
        {
            InputHandler.Instance.BeginDrag(this);
        }
    
        /// <summary>
        /// Called when a player starts dragging (called from InputHandler script)
        /// </summary>
        public void BeginDrag()
        {
            // Reset the current movement (in case the item is still in the air)
            _rigidbody.velocity = Vector2.zero;
        }

        /// <summary>
        /// Called when a player releases the item (called from InputHandler script)
        /// </summary>
        /// <param name="maxVelocity"></param>
        public void EndDrag(float maxVelocity)
        {
            // Limit the flight speed (so that players can't throw items very far)
            if (_rigidbody.velocity.magnitude > maxVelocity)
                _rigidbody.velocity = _rigidbody.velocity.normalized * maxVelocity;
        }

        /// <summary>
        /// The process of following the pointer cursor
        /// </summary>
        /// <param name="followSpeed">The speed at which the item follows the pointer (low values - more smoothness)</param>
        public void Drag(float followSpeed)
        {
            var position = transform.position;
            _rigidbody.velocity = (InputHandler.PointerPosition() - new Vector2(position.x, position.y)) * followSpeed;
            _rigidbody.angularVelocity = 0;
        }
    }
}
