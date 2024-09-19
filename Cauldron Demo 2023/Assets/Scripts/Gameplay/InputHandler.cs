using DataManagers;
using SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay
{
    /// <summary>
    /// Handles the player input
    /// </summary>
    public class InputHandler : MonoBehaviour
    {
        public static InputHandler Instance;

        /// <summary>
        /// The main game camera
        /// </summary>
        [SerializeField] private Camera mainCamera;

        /// <summary>
        /// The speed at which the item follows the pointer (low values - more smoothness)
        /// </summary>
        [Space] [SerializeField] private float itemFollowSpeed = 10;
    
        /// <summary>
        /// The item speed cannot be greater than this value (when a player releases the pointer)
        /// </summary>
        [SerializeField] private float maxVelocity = 50;

        /// <summary>
        /// A reference to the object currently held by the player
        /// </summary>
        private DraggableObject _currentDraggableObject;
        
        private Vector2 _currentItemVelocity;

        private void Awake()
        {
            Instance = this;
        
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = 1000;
        }

        public void BeginDrag(DraggableObject newObject)
        {
            _currentDraggableObject = newObject;
            _currentDraggableObject.BeginDrag();
        }

        private void EndDrag()
        {
            if(_currentDraggableObject == null) return;
        
            _currentDraggableObject.EndDrag(maxVelocity);
            _currentDraggableObject = null;
        }

        private void FixedUpdate()
        {
            if(_currentDraggableObject == null) 
                return;
        
            // The process of following the object after the pointer
            _currentDraggableObject.Drag(itemFollowSpeed);
        }

        private void Update()
        {
            if(Input.GetMouseButtonUp(0)) 
                EndDrag();
        
            if(Input.GetKeyDown(KeyCode.R))
                RestartGame();

            if (Input.GetKeyDown(KeyCode.L))
                PlayerStats.Instance.LoadAllData();

            if (Input.GetKeyDown(KeyCode.T))
                KitchenManager.Instance.GenerateIngredientCombinations();
        }
        
        /// <summary>
        /// Returns the current position of the pointer 
        /// </summary>
        /// <returns></returns>
        public static Vector2 PointerPosition()
        {
            if(Instance.mainCamera) return Instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
            return Vector2.zero;
        }

        /// <summary>
        /// Restart the scene
        /// </summary>
        public void RestartGame()
        {
            PlayerStats.Instance.ResetData();
            LevelLoader.Instance.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
