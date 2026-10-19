using TMPro;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class TutorialCanvasHandler : MonoBehaviour
{
    [SerializeField] private RectTransform _tutorialPanel;
 
    [SerializeField] private GameManagerScriptableObject _gameManagerScriptableObject;

    [SerializeField] private float _distance = 1f;
    [SerializeField] private float _heightOffset = 0f;

    private bool _active = true;

    private Transform _eyeCam;
    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _eyeCam = Camera.main?.transform;
        _tutorialPanel.gameObject.SetActive(true);
        _gameManagerScriptableObject.onInteractionRelease.AddListener(OnRelease);

    }

    private void OnRelease(Ray ray, RaycastHit hit)
    {
        _tutorialPanel.gameObject.SetActive(false);
        _active = false;
    }

    private void FixedUpdate()
    {
        if (_active && _eyeCam!=null)
        {
            Transform canvasTransform = _canvas.transform;
            Vector3 eyeForward = _eyeCam.forward;
            Vector3 eyePos = _eyeCam.position;
            Vector3 targetPos = eyePos + (eyeForward * _distance);
            targetPos.y += _heightOffset;

            canvasTransform.position = targetPos;
            Vector3 LookAtEye = new Vector3(eyePos.x, eyePos.y, eyePos.z);
            canvasTransform.LookAt(LookAtEye);
            canvasTransform.Rotate(0, 180, 0);


        }
    }


}
