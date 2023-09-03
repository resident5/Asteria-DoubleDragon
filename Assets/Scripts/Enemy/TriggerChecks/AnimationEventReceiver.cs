using System;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventReceiver : MonoBehaviour
{
    public event Action<AnimationEvent> AnimationStart;
    public event Action<AnimationEvent> AnimationEnded;
    public event Action<AnimationEvent> DeathAnimationEnded;
    public event Action<AnimationEvent> DeathAnimationStarted;


    private void OnAnimationStart(AnimationEvent animationEvent)
    {
        AnimationStart?.Invoke(animationEvent);
    }
    
    private void OnAnimationEnded(AnimationEvent animationEvent)
    {
        AnimationEnded?.Invoke(animationEvent);
    }

    private void OnDeathEnded(AnimationEvent animationEvent)
    {
        DeathAnimationEnded?.Invoke(animationEvent);
    }

    private void OnDeathStarted(AnimationEvent animationEvent)
    {
        DeathAnimationStarted?.Invoke(animationEvent);
    }

}