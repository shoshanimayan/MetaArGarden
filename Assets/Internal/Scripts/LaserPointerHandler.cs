using UnityEngine;

public class LaserPointerHandler : MonoBehaviour
{
    [SerializeField] private GameObject _pointer;
    [SerializeField] private GameObject _beam;
    [SerializeField] private GameManagerScriptableObject _gameManager;
    private bool _active = false;
    private Vector3 _start = Vector3.zero;
    private Vector3 _end = Vector3.zero;
    private float _baseScale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _baseScale =  _beam.transform.localScale.x;
        _gameManager.onInteractionDown.AddListener(OnInteractionDown);
        _gameManager.onInteractionRelease.AddListener(OnInteractionRelease);

    }

    void Start()
    {
        _pointer?.SetActive(false);
        _beam?.SetActive(false);
    }

    void Update()
    {
        if (_active && (_start != Vector3.zero && _end != Vector3.zero))
        {
            if (!_beam.activeSelf)
            {
                _beam.SetActive(true);
            }
            if (!_pointer.activeSelf)
            {
                _pointer.SetActive(true);
            }
            _pointer.transform.position = _end;
            SetLaser(_start, _end, _baseScale);

        }
        else
        {

            if (_beam.activeSelf)
            {
                _beam.SetActive(false);
            }
            if (_pointer.activeSelf)
            {
                _pointer.SetActive(false);
            }
        }
    }

    private void ToggleActive(bool state)
    {
        _active = state;
    }

    private void SetLaser(Vector3 start, Vector3 end, float width)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;
        
        _beam.transform.up = direction;
        _beam.transform.position = start + (direction / 2f);
        _beam.transform.localScale = new Vector3(width, distance/2f,width);
    }

    private void OnInteractionDown(Ray ray, RaycastHit hit) {
        _start = ray.origin;
        _end = hit.point;
        if (!_active)
        {
            ToggleActive(true);
        }
    
    }

    private void OnInteractionRelease(Ray ray, RaycastHit hit)
    {
        _start = Vector3.zero;
        _end = Vector3.zero;
        ToggleActive(false);

    }
}
