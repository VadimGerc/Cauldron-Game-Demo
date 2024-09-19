using System.Collections;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// A script that changes the scale parameter along a curve
    /// </summary>
    public class ScaleAnimation : MonoBehaviour
    {
        [SerializeField] private bool autoStart;
        [SerializeField] private AnimationCurve scaleCurve;

        private void OnEnable()
        {
            if (autoStart) Scale();
        }

        public void Scale()
        {
            if (!gameObject.activeSelf) return;
        
            StopAllCoroutines();
            StartCoroutine(ScaleProcess());
        }

        private IEnumerator ScaleProcess()
        {
            var elapsedTime = 0f;

            while (true)
            {
                float curveValue;
                if (elapsedTime <= scaleCurve.keys[^1].time)
                {
                    curveValue = scaleCurve.Evaluate(elapsedTime);
                    transform.localScale = new Vector3(curveValue, curveValue, curveValue);
                    elapsedTime += Time.deltaTime;

                }
                else
                {
                    curveValue = scaleCurve.keys[^1].value;
                    transform.localScale = new Vector3(curveValue, curveValue, curveValue);

                    break;
                }

                yield return null;
            }
        }
    }
}
