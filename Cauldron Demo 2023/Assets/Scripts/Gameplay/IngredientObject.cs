using Libraries;
using UnityEngine;

namespace Gameplay
{
    /// <summary>
    /// A placeholder suitable for any product. When appearing in the game - initialized depending on the incoming class instance
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class IngredientObject : DraggableObject
    {
        private SpriteRenderer _spriteRenderer;

        /// <summary>
        /// Reference to an instance of the ingredient class in the Ingredients Library
        /// </summary>
        [HideInInspector] public IngredientItem linkedItem;

        public void Initialize(IngredientItem ingredientItem)
        {
            // Link a class instance to this object in the scene
            linkedItem = ingredientItem;
            
            // Set the sprite specified in the library 
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = linkedItem.GetSprite();

            // Rename the object itself, for convenience
            name = "ProductObject (" + linkedItem.name + ")";

            // Add the collider manually so that it takes the correct shape of the sprite
            gameObject.AddComponent<PolygonCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.CompareTag("ProductsHandler"))
                AddProduct();
        }
        
        private void AddProduct()
        {
            KitchenManager.Instance.AddProduct(this);
            Destroy(gameObject);
        }
    }
}