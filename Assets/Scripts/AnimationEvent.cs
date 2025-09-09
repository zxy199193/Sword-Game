using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public event System.Action OnAnimationEnd;

    public void EndActionAni()
    {
        OnAnimationEnd?.Invoke();
        Debug.Log("Event Sent");
    }
}
