using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace DataManagers
{
    /// <summary>
    /// A class containing information about the already cooked dish (used for the last and best dish)
    /// </summary>
    [Serializable]
    public class CookedDishData
    {
        [SerializeField] private int score;
        [SerializeField] private string name;
        [SerializeField] private string ingredients;

        /// <summary>
        /// Overwrites the dish information (called when the best or the last dish is changed)
        /// </summary>
        public void OverrideDish(int score, string name, List<string> ingredients)
        {
            this.score = score;
            this.name = name;
            this.ingredients = "";
        
            for (var i = 0; i < ingredients.Count; i++)
            {
                this.ingredients += ingredients[i].ToLower();

                if (i < ingredients.Count - 1)
                    this.ingredients += ", ";
            }
        }

        /// <summary>
        /// Is this dish still the best (compared to the one plauer just made)?
        /// </summary>
        /// <param name="score">Points for a new cooked dish</param>
        public bool IsBetterThanNewOne(int score)
        {
            return this.score > score;
        }
    
        /// <summary>
        /// Output dish information in string format
        /// </summary>
        /// <returns></returns>
        public string PrintDishInfo()
        {
            if (string.IsNullOrEmpty(name)) return "";
        
            return name + " (" + ingredients + ") [" + score + "]";
        }
    }

    /// <summary>
    /// Class storing all player's statistics and working with them
    /// </summary>
    [Serializable]
    public class PlayerData
    {
        /// <summary>
        /// Earned score
        /// </summary>
        [SerializeField] private int score;

        [SerializeField] private CookedDishData bestDishData = new CookedDishData();
        [SerializeField] private CookedDishData lastDishData = new CookedDishData();
    
        public void AddScore(int addedScore)
        {
            score += addedScore;
            UIManager.Instance.statsUI.UpdateScore(score.ToString());
        }
    
        /// <summary>
        /// Checks if this dish is the best one, and if so, overwrites the current [bestDishData]
        /// </summary>
        /// <param name="score">Points for the new dish</param>
        /// <param name="name">Name of the new dish</param>
        /// <param name="ingredients">Ingredients from the new dish</param>
        public void CalculateBestDish(int score, string name, List<string> ingredients)
        {
            // If the new dish is better than the previous best result, overwrite it
            if (bestDishData.IsBetterThanNewOne(score)) return;
        
            bestDishData.OverrideDish(score, name, ingredients);
            UIManager.Instance.statsUI.UpdateBestDish(bestDishData.PrintDishInfo());
        }

        /// <summary>
        /// Similar to the best dish save the last one
        /// </summary>
        public void SaveLastDish(int score, string name, List<string> ingredients)
        {
            lastDishData.OverrideDish(score, name, ingredients);
            UIManager.Instance.statsUI.UpdateLastDish(lastDishData.PrintDishInfo());
        }

        /// <summary>
        /// Updates the entire UI at once (used during initialization at startup)
        /// </summary>
        public void UpdateAllUI()
        {
            UIManager.Instance.statsUI.UpdateAllStats(score.ToString(), bestDishData.PrintDishInfo(), lastDishData.PrintDishInfo());
        }
    }

    /// <summary>
    /// The script works with player's data (points, dishes cooked, best dish, etc.)
    /// </summary>
    public class PlayerStats : MonoBehaviour
    {
        public static PlayerStats Instance;
    
        private void Awake()
        {
            Instance = this;
        }

        [HideInInspector] [SerializeField] private PlayerData playerData = new PlayerData();

        private void Start()
        {
            LoadAllData();
        }

        public void AddScore(int addedScore)
        {
            playerData.AddScore(addedScore);
        }
    
        public void CalculateBestDish(int score, string name, List<string> ingredients)
        {
            playerData.CalculateBestDish(score, name, ingredients);
        }
    
        public void SaveLastDish(int score, string name, List<string> ingredients)
        {
            playerData.SaveLastDish(score, name, ingredients);
        }
        
        public void LoadAllData()
        {
            playerData = SavesHelper.LoadDataFromFile();
            playerData.UpdateAllUI();
        }

        public void SaveAllData()
        {
            SavesHelper.SaveDataToFile(playerData);
        }

        /// <summary>
        /// Deletes the save
        /// </summary>
        public void ResetData()
        {
            playerData = new PlayerData();
            SavesHelper.SaveDataToFile(playerData);
        }
    }
}