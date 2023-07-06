using Firebase;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class FirebaseInit : MonoBehaviour
{
    public UnityEvent OnFirebaseLoaded;
    public UnityEvent OnFirebaseFailed;

    public void Start()
    {
        CheckDependency();
    }

    async void CheckDependency()
    {
        var dependencyStatus = await FirebaseApp.CheckDependenciesAsync();

        if (dependencyStatus == DependencyStatus.Available)
        {
            OnFirebaseLoaded?.Invoke();
        }
        else
        {
            OnFirebaseFailed?.Invoke();
        }
    }
}
