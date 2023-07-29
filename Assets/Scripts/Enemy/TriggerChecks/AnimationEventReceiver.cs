using System;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventReceiver : MonoBehaviour
{
    public Enemy enemy;
    public event Action<AnimationEvent> hitAnimationStart; 
    public event Action<AnimationEvent> hitAnimationEnded; 
    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    private void OnAnimationStart(AnimationEvent animationEvent)
    {
        hitAnimationStart?.Invoke(animationEvent);
    }
    
    private void OnAnimationEnded(AnimationEvent animationEvent)
    {
        hitAnimationEnded?.Invoke(animationEvent);
    }


}