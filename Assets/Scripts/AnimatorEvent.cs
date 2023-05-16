using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimatorEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent event1;
    [SerializeField] private UnityEvent event2;
    [SerializeField] private UnityEvent event3;

    public void Event1()
    {
        event1?.Invoke();
    }
    public void Event2()
    {
        event2?.Invoke();
    }
    public void Event3()
    {
        event3?.Invoke();
    }
}
