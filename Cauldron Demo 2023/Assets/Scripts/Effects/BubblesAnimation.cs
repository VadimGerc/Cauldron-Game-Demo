using Gameplay;
using UnityEngine;

namespace Effects
{
    /// <summary>
    /// Animates bubbles when adding new ingredients to the cauldron
    /// </summary>
    [RequireComponent(typeof(ParticleSystem))]
    public class BubblesAnimation : MonoBehaviour
    {
        private ParticleSystem _particleSystem;
        
        private void Start()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            KitchenManager.Instance.IngredientAdded += PlayAnimation;
        }

        private void PlayAnimation()
        {
            _particleSystem.Play();
        }
    }
}
