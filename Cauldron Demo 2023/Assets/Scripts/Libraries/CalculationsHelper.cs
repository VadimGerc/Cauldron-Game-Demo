using System.Collections.Generic;
using System.Linq;

namespace Libraries
{
    /// <summary>
    /// Helps make calculations
    /// </summary>
    public static class CalculationsHelper
    {
        /// <summary>
        /// Calculates score and combos
        /// </summary>
        public static int CalculateScore(List<IngredientItem> ingredientItems)
        {
            // Sort the products and get the list and quantity of products of each type
            var sortProductGroups = ingredientItems.GroupBy(product => product.type).OrderByDescending(product => product.Count())
                .Select(product => new {BrandID = product.Key, Count = product.Count()}).ToList();

            // In each product group, multiply the number of ingredients by their points (for the base value) and then multiply by a multiplier depending on the quantity
            var baseScore = (from productGroup in sortProductGroups
                let productClass = ingredientItems.Find(item => item.type == productGroup.BrandID)
                let addScore = productGroup.Count * productClass.score
                let multiplier = productGroup.Count switch
                {
                    2 => 2,
                    3 => 1.5f,
                    4 => 1.25f,
                    _ => 1f
                }
                select addScore * multiplier).Sum();

            // If there are no repetitions of the ingredients, then multiply by 2
            if (!sortProductGroups.Exists(group => group.Count > 1))
                baseScore *= 2;
        
            return (int) baseScore;
        }

        /// <summary>
        /// Returns a list of products and their quantities
        /// </summary>
        /// <param name="ingredientItems"></param>
        /// <returns></returns>
        public static List<string> GetSortedIngredients(List<IngredientItem> ingredientItems)
        {
            var sortProductGroups = ingredientItems.GroupBy(product => product.type).OrderByDescending(product => product.Count())
                .Select(product => new {BrandID = product.Key, Count = product.Count()}).ToList();
        
            return sortProductGroups.Select(product => product.Count + " " + ingredientItems.Find(item => item.type == product.BrandID).name).ToList();
        }

        /// <summary>
        /// Generates combinations of ingredients
        /// </summary>
        /// <param name="items">All available ingredients</param>
        /// <param name="combinationSize">Max number of ingredients per combination</param>
        /// <returns></returns>
        public static List<List<IngredientItem>> GenerateCombinations(List<IngredientItem> items, int combinationSize)
        {
            var allCombinations = new List<List<IngredientItem>>();
            GenerateCombinationsRecursive(items, new List<IngredientItem>(), combinationSize, 0, allCombinations);
            return allCombinations;
        }

        private static void GenerateCombinationsRecursive(List<IngredientItem> items, List<IngredientItem> currentCombination, int size, int position, List<List<IngredientItem>> allCombinations)
        {
            if (currentCombination.Count == size)
            {
                allCombinations.Add(new List<IngredientItem>(currentCombination));
                return;
            }

            for (var i = position; i < items.Count; i++)
            {
                currentCombination.Add(items[i]);
                GenerateCombinationsRecursive(items, currentCombination, size, i, allCombinations);
                currentCombination.RemoveAt(currentCombination.Count - 1);
            }
        }
    
        /// <summary>
        /// Sort combinations by decreasing points
        /// </summary>
        public static List<List<IngredientItem>> SortCombinationsByScore(List<List<IngredientItem>> combinations)
        {
            return combinations.OrderByDescending(CalculateScore).ToList();
        }

    }
}
