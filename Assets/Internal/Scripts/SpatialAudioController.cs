using ScriptableObjects;
using UnityEngine;
using Meta;

public class SpatialAudioController : MonoBehaviour
{
    [SerializeField] private AudioLibrary _audioLibrary;
    private AudioSource _audioSource;
    private MetaXRAudioSource _audioSourceXRAudioSource;
    private MetaXRAudioSourceExperimentalFeatures _audioSourceExperimentalFeatures;
    private AudioClip _audioClip;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSourceXRAudioSource = GetComponent<MetaXRAudioSource>();
        _audioSourceExperimentalFeatures = GetComponent<MetaXRAudioSourceExperimentalFeatures>();
        _audioLibrary.AudioEvent.AddListener(TryPlayClip);
    }
    public void TryPlayClip(string clipKey, Vector3 pos)
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
        if (_audioLibrary.TryGetClip(clipKey, out _audioClip))
        {
            transform.position = pos;
            _audioSource.PlayOneShot(_audioClip);
        }
        else
        { 
        Debug.LogError($"no clip with name \"{clipKey}\" found");
        }
        
    }

    public void tryPlayDefaultClip(Vector3 pos) 
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
        transform.position = pos;
        _audioSource.Play();
    }
   

    
}
