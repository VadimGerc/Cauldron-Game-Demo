using UnityEngine;

namespace Effects
{
    /// <summary>
    /// Animation preset (universal - can be used in different scripts)
    /// </summary>
    [CreateAssetMenu(menuName = "Animation Preset", fileName = "Animation Preset")]
    public class AnimationPreset : ScriptableObject
    {
        /// <summary>
        /// Normalized animation curve
        /// </summary>
        public AnimationCurve animationCurve;
        
        /// <summary>
        /// Non-normalized animation time (X axis)
        /// </summary>
        public float duration;
        
        /// <summary>
        /// Non-normalized maximum animation value (Y-axis)
        /// </summary>
        public float maxValue;

    }
}
