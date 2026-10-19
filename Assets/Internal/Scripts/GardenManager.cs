using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;


[RequireComponent(typeof(ProceduralVineManager))]
public class GardenManager : MonoBehaviour
{
    [SerializeField] private GameObject _grassPrefab;
    [SerializeField] private readonly int _grassCount = 10;
    [SerializeField] private GameManagerScriptableObject _gameManagerScriptableObject;
    [SerializeField] private SpatialAudioController _grassSpatialAudioController;
    [SerializeField] private SpatialAudioController _vineSpatialAudioController;
    [SerializeField] private float _assetDistance = 0.5f;
    private List<GameObject> _grassList;
    private int _currentGrassIndex = 0;
    private ProceduralVineManager _proceduralVineManager;
    private bool _roomReady = false;
    

    private void Awake()
    {
        _grassList = new List<GameObject>();
        _proceduralVineManager= GetComponent<ProceduralVineManager>();
        _gameManagerScriptableObject.OnRoomReadyEvent.AddListener(() => { _roomReady = true; });
        _gameManagerScriptableObject.onInteractionRelease.AddListener(SpawnVine);
        _gameManagerScriptableObject.onInteractionDown.AddListener(SpawnGrass);
        for (int i = 0; i < _grassCount; i++)
        { 
            GameObject grass = Instantiate(_grassPrefab, Vector3.zero,Quaternion.identity,transform);
            grass.SetActive(false);
            _grassList.Add(grass);
        }
    }


    private bool CheckAssetDistance(Vector3 position)
    {
        foreach (GameObject grass in _grassList)
        {
            if (grass.activeSelf)
            {
                if (Vector3.Distance(grass.transform.position, position) <= _assetDistance)
                {
                    return false;
                }
            }
        
        }


        return true;
    }
    

    private void SpawnGrass(Ray ray, RaycastHit hit)
    {
        if (!CheckAssetDistance(hit.point))
        {
            return;
        }
        _grassSpatialAudioController.tryPlayDefaultClip(hit.point);
        MRUKAnchor.SceneLabels searchLabels = MRUKAnchor.SceneLabels.FLOOR;

        MRUKAnchor anchor = hit.collider.GetComponentInParent<MRUKAnchor>();
        if (anchor != null && anchor.HasAnyLabel(searchLabels))
        {

            _currentGrassIndex++;
            if (_currentGrassIndex >= _grassCount)
            {
                _currentGrassIndex = 0;
            }
            GameObject obj = _grassList[_currentGrassIndex];
            obj.SetActive(true);
            obj.transform.position = hit.point;
            obj.transform.up = hit.normal;
        }
        
    }

    private void SpawnVine(Ray ray, RaycastHit hit)
    {
        _vineSpatialAudioController.tryPlayDefaultClip(hit.point);
        MRUKAnchor.SceneLabels searchLabels = MRUKAnchor.SceneLabels.UNKNOWN;

        MRUKAnchor anchor = hit.collider.GetComponentInParent<MRUKAnchor>();
        if (anchor != null && !anchor.HasAnyLabel(searchLabels))
        {
            _proceduralVineManager.CreateVines(hit);
        }
    
    }

}
