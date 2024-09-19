using Libraries;
using UnityEngine;

namespace Gameplay
{
    /// <summary>
    /// Controlling ingredient spawning by clicking on a container
    /// </summary>
    public class SpawnContainer : MonoBehaviour
    {
        /// <summary>
        /// This manager will find a product of the same type in the IngredientsLibrary and spawn it
        /// </summary>
        [SerializeField] private IngredientType ingredientType;

        /// <summary>
        /// Reacts when a player clicked on the sprite (actually its collider)
        /// </summary>
        private void OnMouseDown()
        {
            SpawnItem();
        }

        /// <summary>
        /// Spawn new item based on its type
        /// </summary>
        private void SpawnItem()
        {
            var productToSpawn = KitchenManager.Instance.GetIngredientByType(ingredientType);
            var placeholderToSpawn = KitchenManager.Instance.GetIngredientPlaceholder();
        
            if(productToSpawn == null || placeholderToSpawn == null) return;

            var pointerPosition = InputHandler.PointerPosition();
            var instantiatedItem = Instantiate(placeholderToSpawn, new Vector3(pointerPosition.x, pointerPosition.y, -1), Quaternion.identity);
            instantiatedItem.Initialize(productToSpawn);
            
            InputHandler.Instance.BeginDrag(instantiatedItem);
        }
    }
}
