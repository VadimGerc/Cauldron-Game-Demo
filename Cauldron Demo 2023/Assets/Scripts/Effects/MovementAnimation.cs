using System.Collections;
using UnityEngine;

namespace Effects
{
    public enum Direction
    {
        X,Y,Z
    }
    
    /// <summary>
    /// Changes the position of an object based on the curve
    /// </summary>
    public class MovementAnimation : MonoBehaviour
    {
        [SerializeField] private AnimationPreset animationPreset;

        /// <summary>
        /// Movement direction
        /// </summary>
        [Space] [SerializeField] private Direction direction;
        

        [Space] [SerializeField] private bool autoStart;

        private void Start()
        {
           if(autoStart) PlayAnimation();
        }

        private void PlayAnimation()
        {
            StopAllCoroutines();
            StartCoroutine(AnimationProcess(animationPreset.duration, animationPreset.maxValue));
        }

        private IEnumerator AnimationProcess(float duration, float maxValue)
        {
            var timer = 0f;
            var startPosition = transform.position;
            
            var moveDirection = direction switch
            {
                Direction.X => Vector3.right,
                Direction.Y => Vector3.up,
                _ => Vector3.up
            };

            while (timer < duration)
            {
                timer += Time.deltaTime;
                
                var normalizedTime = timer / duration;
                var value = animationPreset.animationCurve.Evaluate(normalizedTime) * maxValue;
                
                transform.position = startPosition + value * moveDirection;

                yield return null;
            }
        }
    }
}
