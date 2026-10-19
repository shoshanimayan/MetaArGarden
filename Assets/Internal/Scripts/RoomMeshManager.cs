using Meta.XR.MRUtilityKit;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;

public class RoomMeshManager : MonoBehaviour
{

    [SerializeField] private MRUK _mRUK;
   
    [SerializeField] private GameManagerScriptableObject _gameManagerScriptableObject;

    private List<Transform> _partsToEffect;
    private MRUKRoom _room;


    private void Awake()
    {
        _mRUK.RoomCreatedEvent.AddListener(StartMeshProcess);

    }

    private void StartMeshProcess(MRUKRoom room)
    {
        _room = room;
        _gameManagerScriptableObject.OnRoomReady();
        
        
    }

    
}
