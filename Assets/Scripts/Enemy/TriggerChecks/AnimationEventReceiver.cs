using System;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventReceiver : MonoBehaviour
{
    public event Action<AnimationEvent> AnimationStart; 
    public event Action<AnimationEvent> AnimationEnded;

    private void OnAnimationStart(AnimationEvent animationEvent)
    {
        AnimationStart?.Invoke(animationEvent);
    }
    
    private void OnAnimationEnded(AnimationEvent animationEvent)
    {
        AnimationEnded?.Invoke(animationEvent);
    }


}