using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Libraries
{
    /// <summary>
    /// Used for linking items in the library and spawning containers in the game
    /// </summary>
    public enum IngredientType
    {
        Potato, 
        Pepper,
        Carrot,
        Onion,
        Meat
    }

    /// <summary>
    /// The main class of the ingredients that stores all the information about it
    /// </summary>
    [Serializable]
    public class IngredientItem
    { 
        public string name;
        public IngredientType type;
        
        /// <summary>
        /// Score points for this ingredient
        /// </summary>
        public int score;
        
        /// <summary>
        /// Unique ID for each ingredient (needed to overwrite values from JSON file)
        /// </summary>
        [Space] public string id;

        /// <summary>
        /// Sprite used in game
        /// </summary>
        [Space] [SerializeField] private Sprite sprite;

        /// <summary>
        /// Icon for UI
        /// </summary>
        [SerializeField] private Sprite icon;


        public Sprite GetSprite()
        {
            if(sprite != null)
                return sprite;
            
            Debug.LogError("[" + sprite + "] doesn't have a sprite in the IngredientsLibrary.");
            return null;
        }

        public Sprite GetIcon()
        {
            if(icon != null)
                return icon;
            
            Debug.LogError("[" + name + "] doesn't have an icon in the IngredientsLibrary.");
            return null;
        }

        /// <summary>
        /// Overwrites the ingredient parameters with new ones
        /// </summary>
        /// <param name="loadedItem"></param>
        public void OverrideParameters(IngredientItem loadedItem)
        {
            name = loadedItem.name;
            score = loadedItem.score;
            type = loadedItem.type;

            //TODO: здесь можно расширить логику и также подгружать иконку и спрайт
        }
    }
    
    /// <summary>
    /// A class storing a list of all ingredients
    /// </summary>
    [Serializable]
    public class IngredientsList
    {
        [SerializeField] private List<IngredientItem> ingredientItems = new List<IngredientItem>();

        public IngredientItem FindByType(IngredientType type)
        {
            return ingredientItems.Find(item => item.type == type);
        }

        public List<string> GetAllNames()
        {
            return ingredientItems.Select(item => item.name).ToList();
        }

        /// <summary>
        /// Overwrites parameters of all ingredients with new ones loaded from JSON file
        /// </summary>
        /// <param name="loadedList"></param>
        public void OverrideWithLoadedValues(IngredientsList loadedList)
        {
            foreach (var loadedItem in loadedList.ingredientItems)
            {
                // Search for a suitable product in the local list by id
                var localItem = ingredientItems.Find(ingredient => ingredient.id == loadedItem.id);
                localItem?.OverrideParameters(loadedItem);
            }
        }

        /// <summary>
        /// Generates combinations and sorts them by points
        /// </summary>
        public List<List<IngredientItem>> GenerateCombinations(int maxIngredientsAmount)
        {
            var combinations = CalculationsHelper.GenerateCombinations(ingredientItems, maxIngredientsAmount);
            var sortedCombinations = CalculationsHelper.SortCombinationsByScore(combinations);

            return sortedCombinations;
        }
    }

    /// <summary>
    /// SO ingredients storage library
    /// </summary>
    [CreateAssetMenu(menuName = "Ingredients Library", fileName = "Ingredients Library")]
    public class IngredientsLibrary : ScriptableObject
    {
        public IngredientsList ingredients;
    }
}