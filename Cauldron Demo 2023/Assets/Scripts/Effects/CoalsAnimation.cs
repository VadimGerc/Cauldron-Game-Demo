using Gameplay;
using UnityEngine;

namespace Effects
{
    /// <summary>
    /// Animates the coals (applies to the active phase)
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class CoalsAnimation : MonoBehaviour
    {
        [SerializeField] private AnimationPreset animationPreset;

        /// <summary>
        /// Transition time between active state and inactive state
        /// </summary>
        [SerializeField] private float fadingTime;

        private SpriteRenderer _spriteRenderer;

        /// <summary>
        /// If it's animated now
        /// </summary>
        private bool _isAnimating;
        
        /// <summary>
        /// If the animation starts
        /// </summary>
        private bool _isInTransition;
        
        /// <summary>
        /// If the animation stops
        /// </summary>
        private bool _isReversing;

        private float _timer;
        private float _transitionTimer;

        private float _startAlphaValue;
        private float _currentAlphaValue;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _currentAlphaValue = GetAlpha();

            KitchenManager.Instance.IngredientAdded += StartAnimation;
            KitchenManager.Instance.DishCooked += StopAnimation;
        }
        
        private void Update()
        {
            CalculateAlpha();
            SetAlpha(_currentAlphaValue);
        }
        
        private void StartAnimation()
        {
            if (_isAnimating && !_isReversing) return;

            print("start animation");
            _startAlphaValue = _currentAlphaValue;
            _transitionTimer = 0;
            _isAnimating = true;
            _isReversing = false;
            
            // Start the transition to the curve values
            _isInTransition = true;
        }

        private void StopAnimation()
        {
            if (!_isAnimating || _isReversing) return;

            print("stop animation");
            
            _transitionTimer = 0;
            _isInTransition = true;
            
            // Start the transition from the curve values to 0
            _isReversing = true; 
        }

        /// <summary>
        /// Calculates the desired transparency value
        /// </summary>
        private void CalculateAlpha()
        {
            if (!_isAnimating) return;

            if (_isInTransition)
            {
                _transitionTimer += Time.deltaTime;

                if (_transitionTimer > fadingTime)
                {
                    _transitionTimer = fadingTime;
                    _isInTransition = false;
                    // Ending the transition
                }

                var normalizedTransitionTime = _transitionTimer / fadingTime;
                
                // Change the alpha value depending on the current state
                _currentAlphaValue = _isReversing
                    ? Mathf.Lerp(_currentAlphaValue, 0, normalizedTransitionTime)
                    : Mathf.Lerp(_startAlphaValue, animationPreset.animationCurve.Evaluate(0), normalizedTransitionTime);

                if (_isInTransition) return;

                if (_isReversing)
                {
                    _isAnimating = false;
                    _isReversing = false;
                    // Ending the animation
                }
                else
                {
                    _startAlphaValue = _currentAlphaValue;
                    _timer = 0;
                }
            }
            else
            {
                _timer += Time.deltaTime;

                // Reset timer for continuous animation
                if (_timer > animationPreset.duration)
                {
                    _timer = 0; 
                }

                var normalizedTime = _timer / animationPreset.duration;
                _currentAlphaValue = Mathf.Lerp(_startAlphaValue, animationPreset.animationCurve.Evaluate(normalizedTime), normalizedTime);
            }
        }

        private float GetAlpha()
        {
            return _spriteRenderer.color.a;
        }

        private void SetAlpha(float alphaValue)
        {
            var spriteRendererColor = _spriteRenderer.color;
            spriteRendererColor.a = alphaValue;
            _spriteRenderer.color = spriteRendererColor;
        }
    }
}
