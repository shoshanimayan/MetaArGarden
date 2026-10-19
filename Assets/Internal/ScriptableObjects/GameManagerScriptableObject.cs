using System.Linq;
using UnityEngine;
using UnityEngine.Events;



[CreateAssetMenu(fileName = "GameManagerScriptableObject", menuName = "Scriptable Objects/GameManagerScriptableObject")]

public class GameManagerScriptableObject : ScriptableObject
{
   

  
    public UnityEvent OnRoomReadyEvent;
    public UnityEvent<Ray,RaycastHit> onInteractionDown;
    public UnityEvent<Ray,RaycastHit> onInteractionRelease;

    private void OnEnable()
    {
       
    }

    public void OnRoomReady()
    { 
        OnRoomReadyEvent?.Invoke();
    }

    public void OnInteractionDown(Ray ray, RaycastHit hit)
    {
        onInteractionDown?.Invoke(ray,hit);
    }

    public void OnInteractionRelase(Ray ray, RaycastHit hit)
    { 
        onInteractionRelease?.Invoke(ray, hit);
    }

    

}
