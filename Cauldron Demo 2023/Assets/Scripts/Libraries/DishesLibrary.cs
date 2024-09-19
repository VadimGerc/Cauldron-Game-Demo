using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Libraries
{
    /// <summary>
    /// Class containing information about the dish (name and quantity of the required ingredients)
    /// </summary>
    [Serializable]
    public class Dish
    {
        public string name;
    
        [SerializeField] private List<RequiredIngredients> requiredIngredients;

        /// <summary>
        /// Match the ingredients in the cauldron with all the products required for this dish, and determine if it has been cooked?
        /// </summary>
        /// <param name="ingredients">Ingredients in the cauldron</param>
        public bool IsSuitableDish(List<IngredientItem> ingredients)
        {
            return requiredIngredients.All(item => item.IsSuitableIngredient(ingredients));
        }
    }

    /// <summary>
    /// The product required for the dish and its quantity
    /// </summary>
    [Serializable]
    public class RequiredIngredients
    {
        [SerializeField] private IngredientType ingredientType;
    
        //Min and max products amount to cook the dish
        [SerializeField] private int minIngredients;
        [SerializeField] private int maxIngredients;

        /// <summary>
        /// Does the cauldron have the required amount of this ingredient?
        /// </summary>
        /// <param name="ingredients">Ингридиенты в кастрюле</param>
        public bool IsSuitableIngredient(List<IngredientItem> ingredients)
        {
            var ingredientsCount = ingredients.Count(item => item.type == ingredientType);
            return ingredientsCount >= minIngredients && ingredientsCount <= maxIngredients;
        }
    }

    /// <summary>
    /// SO dishes storage library
    /// </summary>
    [CreateAssetMenu(menuName = "Dishes Library", fileName = "Dishes Library")]
    public class DishesLibrary : ScriptableObject
    {
        /// <summary>
        /// List of dishes and required ingredients
        /// </summary>
        [SerializeField] private List<Dish> dishes = new List<Dish>();

        /// <summary>
        /// Name of the default dish (when no dish above is cooked)
        /// </summary>
        [SerializeField] private string defaultDishName;

        /// <summary>
        /// Choose a dish based on the ingredients in the cauldron (if the combination of ingredients is suitable for more than one dish, the first one will be chosen)
        /// </summary>
        public string ChooseDish(List<IngredientItem> ingredients)
        {
            var cookedDish = defaultDishName;

            foreach (var dish in dishes.Where(dish => dish.IsSuitableDish(ingredients)))
            {
                cookedDish = dish.name;
                break;
            }

            return cookedDish;
        }
    }
}