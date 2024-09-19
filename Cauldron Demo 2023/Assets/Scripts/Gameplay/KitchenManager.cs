using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataManagers;
using Libraries;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    /// <summary>
    /// Main cooking control
    /// </summary>
    public class KitchenManager : MonoBehaviour
    {
        public static KitchenManager Instance;

        /// <summary>
        /// Link to the library with all products
        /// </summary>
        [SerializeField] private IngredientsLibrary ingredientsLibrary;
    
        /// <summary>
        /// Link to the library with all dishes
        /// </summary>
        [SerializeField] private DishesLibrary dishesLibrary;

        /// <summary>
        /// This placeholder will be spawned for any product and then initialized according to the product type
        /// </summary>
        [Space] [SerializeField] private IngredientObject ingredientPlaceholder;

        /// <summary>
        /// Amount of ingredients required for cooking
        /// </summary>
        [Space] [SerializeField] private int maxIngredientsAmount;

        /// <summary>
        /// All products that are currently in cauldron
        /// </summary>
        private readonly List<IngredientObject> _ingredientsInCauldron = new List<IngredientObject>();
        
        public delegate void CallbackFunction();
        
        /// <summary>
        /// The event is triggered when a new ingredient is added to the cauldron
        /// </summary>
        public event CallbackFunction IngredientAdded;
        
        /// <summary>
        /// The event is triggered when a dish is cooked
        /// </summary>
        public event CallbackFunction DishCooked;
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            // Load ingredients from JSON file and overwrite local products with them
            CompareTwoIngredientLists(SavesHelper.LoadIngredientsFromFile(), GetIngredientsLibrary().ingredients);

            // Create icons in the game corresponding to the maximum number of ingredients
            UIManager.Instance.ingredientsUI.Initialize(maxIngredientsAmount);
        }

        /// <summary>
        /// Returns the current Ingredients Library
        /// </summary>
        private IngredientsLibrary GetIngredientsLibrary()
        {
            if (ingredientsLibrary != null) return ingredientsLibrary;
        
            Debug.LogError("(Kitchen Manager) [Ingredients Library] not set.", gameObject);
            return null;
        }

        /// <summary>
        /// Search for a product with a certain type in the Ingredients Library
        /// </summary>
        public IngredientItem GetIngredientByType(IngredientType type)
        {
            var library = GetIngredientsLibrary();

            if (!library) return null;
        
            var foundProduct = library.ingredients.FindByType(type);

            if (foundProduct != null) return foundProduct;
        
            Debug.LogError($"Product type '{type}' is not specified in the [Product Library].");
            return null;
        }

        /// <summary>
        /// Returns the actual placeholder object
        /// </summary>
        public IngredientObject GetIngredientPlaceholder()
        {
            if (ingredientsLibrary != null) return ingredientPlaceholder;
        
            Debug.LogError("(Kitchen Manager) [Ingredient Object Placeholder] not set.", gameObject);
            return null;
        }

        /// <summary>
        /// Comparing 2 ingredient lists (loaded from JSON and the local one)
        /// </summary>
        private void CompareTwoIngredientLists(IngredientsList loadedList, IngredientsList localList)
        {
            localList?.OverrideWithLoadedValues(loadedList);
        }
 
        /// <summary>
        /// Generate and display in the console all possible combinations based on the available products and the amount of ingredients required for cooking
        /// </summary>
        public void GenerateIngredientCombinations()
        {
            var library = GetIngredientsLibrary();

            if (!library) return;

            var generatedCombinations = ingredientsLibrary.ingredients.GenerateCombinations(maxIngredientsAmount);
        
            // Output to console
            foreach (var combination in generatedCombinations)
            {
                var ingredients = CalculationsHelper.GetSortedIngredients(combination);
                var combinationString = string.Join(", ", ingredients);
                
                var totalScore = CalculationsHelper.CalculateScore(combination);
                var dishName = dishesLibrary.ChooseDish(combination);
                
                Debug.Log($"Score: {totalScore} | {dishName} | {combinationString}");
            }
        }

        /// <summary>
        /// Add a product to the cauldron
        /// </summary>
        /// <param name="ingredientObject"></param>
        public void AddProduct(IngredientObject ingredientObject)
        {
            _ingredientsInCauldron.Add(ingredientObject);
            UIManager.Instance.ingredientsUI.ReplaceIcon(ingredientObject.linkedItem.GetIcon(), _ingredientsInCauldron.Count - 1);

            IngredientAdded?.Invoke();
            
            CookingProcess();
        }

        /// <summary>
        /// Cooking process
        /// </summary>
        private void CookingProcess()
        {
            // Check if the dish can be cooked
            if (_ingredientsInCauldron.Count != maxIngredientsAmount) return;
        
            // Convert the list of objects to the list of class instances
            var ingredientsItems = _ingredientsInCauldron.Select(ingredient => ingredient.linkedItem).ToList();
            
            // Calculate the scores, identify the ingredients and the name of the dish
            var score = CalculationsHelper.CalculateScore(ingredientsItems);
            var dishName = dishesLibrary.ChooseDish(ingredientsItems);
            var ingredients = CalculationsHelper.GetSortedIngredients(ingredientsItems);
            
            // Update player's stats
            PlayerStats.Instance.AddScore(score);
            PlayerStats.Instance.CalculateBestDish(score, dishName, ingredients);
            PlayerStats.Instance.SaveLastDish(score, dishName, ingredients);
            PlayerStats.Instance.SaveAllData();
            
            UIManager.Instance.ingredientsUI.SpawnDishName($"{dishName} (+{score})" );
            
            DishCooked?.Invoke();
        
            // A short delay before the cauldron is cleared
            ActionDelay(delegate
            {
                _ingredientsInCauldron.Clear();
                UIManager.Instance.ingredientsUI.ResetAllIcons();
            }, 2.3f);
        }
        
        private void ActionDelay(UnityAction action, float delay)
        {
            StopAllCoroutines();
            StartCoroutine(ActionDelayProcess(action, delay));
        }

        private IEnumerator ActionDelayProcess(UnityAction action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }
    }
}