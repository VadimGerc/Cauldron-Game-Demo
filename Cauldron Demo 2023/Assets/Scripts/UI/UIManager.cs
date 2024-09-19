using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UI
{
   /// <summary>
   /// A class containing UI items associated with ingredients
   /// </summary>
   [Serializable]
   public class IngredientsUI
   {
      [SerializeField] private Image iconPlaceholder;
      [SerializeField] private Transform iconsHolder;
      [SerializeField] private Sprite defaultSprite;
   
      [Space] [SerializeField] private TMP_Text dishNamePlaceholder;
      [SerializeField] private Transform worldCanvas;

      private List<Image> _instantiatedIcons = new List<Image>();

      /// <summary>
      /// Spawns basic product icons ('?') to be replaced by ingredient icons
      /// </summary>
      /// <param name="ingredientsAmount">Maximum number of ingredients</param>
      public void Initialize(int ingredientsAmount)
      {
         for (var i = 0; i < ingredientsAmount; i++)
         {
            var instantiatedIcon = Object.Instantiate(iconPlaceholder, iconsHolder);
            _instantiatedIcons.Add(instantiatedIcon);
         }
      }

      /// <summary>
      /// Replaces the icon of an ingredient under a specific index
      /// </summary>
      public void ReplaceIcon(Sprite icon, int index)
      {
         if(index > _instantiatedIcons.Count - 1) return;
      
         _instantiatedIcons[index].sprite = icon;
         _instantiatedIcons[index].gameObject.GetComponent<ScaleAnimation>().Scale();
      }

      /// <summary>
      /// Spawns an object that displays the name of a dish
      /// </summary>
      /// <param name="name"></param>
      public void SpawnDishName(string name)
      {
         if (!dishNamePlaceholder || !worldCanvas) return;
      
         var instantiatedObject = Object.Instantiate(dishNamePlaceholder, worldCanvas);
         instantiatedObject.text = name;
      }

      /// <summary>
      /// Reset all icons to default values ('?')
      /// </summary>
      public void ResetAllIcons()
      {
         foreach (var icon in _instantiatedIcons)
         {
            icon.sprite = defaultSprite;
            icon.gameObject.GetComponent<ScaleAnimation>().Scale();
         }
      }
   }

   /// <summary>
   /// A class containing UI items associated with player's data
   /// </summary>
   [Serializable]
   public class StatsUI
   {
      [SerializeField] private TMP_Text scoreText;
      [SerializeField] private TMP_Text bestDishScore;
      [SerializeField] private TMP_Text lastDishScore;

      public void UpdateScore(string newScore)
      {
         scoreText.text = "Счёт: " + newScore;
      }

      public void UpdateBestDish(string newDish)
      {
         bestDishScore.text = "Лучшее блюдо: " + newDish;
      }

      public void UpdateLastDish(string newDish)
      {
         lastDishScore.text = "Последнее блюдо: " + newDish;
      }

      public void UpdateAllStats(string score, string bestDish, string lastDish)
      {
         UpdateScore(score);
         UpdateBestDish(bestDish);
         UpdateLastDish(lastDish);
      }
   }

   /// <summary>
   /// Controls the entire UI
   /// </summary>
   public class UIManager : MonoBehaviour
   {
      public static UIManager Instance;
   
      public IngredientsUI ingredientsUI;
      [Space] public StatsUI statsUI;
   
      private void Awake()
      {
         Instance = this;
      }
   }
}