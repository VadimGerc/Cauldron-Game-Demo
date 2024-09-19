using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Effects
{
    /// <summary>
    /// Controls the alpha value of the color depending on the curve (can be used with Images or TMP_Texts)
    /// </summary>
    public class AlphaAnimation : MonoBehaviour
    {
        [SerializeField] private Image imageComponent;
        [SerializeField] private TMP_Text textComponent;

        [Space] [SerializeField] private AnimationPreset animationPreset;
    
        [Space] [SerializeField] private bool autoStart;

        private void Start()
        {
            if(autoStart) StartAnimation();
        }

        private void StartAnimation()
        {
            StopAllCoroutines();
            StartCoroutine(ChangeColorProcess(animationPreset.duration));
        }

        private IEnumerator ChangeColorProcess(float duration)
        {
            var timer = 0f;
            var mainColor = imageComponent ? imageComponent.color : textComponent.color;
        
            while (timer < duration)
            {
                timer += Time.deltaTime;
                var value = animationPreset.animationCurve.Evaluate(timer / duration);
                
                if(imageComponent) imageComponent.color = new Color(mainColor.r, mainColor.g, mainColor.b, value);
                else textComponent.color = new Color(mainColor.r, mainColor.g, mainColor.b, value);

                yield return null;
            }
        }
    }
}
