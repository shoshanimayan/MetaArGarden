using UnityEngine;
using UnityEngine.XR.Hands;

public class PlayerInputHandler : MonoBehaviour
{

    [SerializeField] private OVRInput.RawButton _rightShootButton;
    [SerializeField] private OVRHand _rightHand;
    [SerializeField] private OVRInput.RawButton _leftShootButton;
    [SerializeField] private OVRHand _leftHand;
    [SerializeField] private float _maxRayDistance=10f;
    [SerializeField] private GameManagerScriptableObject _gameManagerScriptableObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

    // Update is called once per frame
    void Update()
    {
        if (_leftHand != null && _leftHand.IsTracked)
        {
            // Check if the thumb and index finger are pinching
            bool isLeftPinching = _leftHand.IsPressed();
            bool isLeftReleased = _leftHand.IsReleased();
            Debug.Log(isLeftReleased.ToString());
            if (isLeftPinching)
            {
                Debug.Log("Pinching left hand");

                PinchRaycast(_leftHand);
            }
            else if (isLeftReleased)
            {
                PinchReleaseRaycast(_leftHand);

                Debug.Log("Left Hand Pinch released");
            }
        }

        
        if (_rightHand != null && _rightHand.IsTracked)
        {
            // Check if the thumb and index finger are pinching

            bool isRightPinching = _rightHand.IsPressed();
            bool isRightReleased = _rightHand.IsReleased();
            if (isRightPinching)
            {
                Debug.Log("Pinching Right hand");
                PinchRaycast(_rightHand);
            }
            else if (isRightReleased)
            {
                Debug.Log("Right Hand Pinch released");
                PinchReleaseRaycast(_rightHand);

            }
        }

        bool isLeftTracked = OVRInput.GetControllerPositionTracked(OVRInput.Controller.LTouch);
        bool isRightTracked = OVRInput.GetControllerPositionTracked(OVRInput.Controller.RTouch);

        if (isLeftTracked && OVRInput.Get(_leftShootButton))
        {
            RaycastFromController(_leftShootButton, true);

        }
        else if (isLeftTracked && OVRInput.GetUp(_leftShootButton)) {
            RaycastFromController(_leftShootButton, false);


        }

        if (isRightTracked && OVRInput.Get(_rightShootButton))
        {
            RaycastFromController(_rightShootButton, true);

        }
        else if (isRightTracked && OVRInput.GetUp(_rightShootButton))
        {
            RaycastFromController(_rightShootButton, false);


        }
    }

    private void RaycastFromController(OVRInput.RawButton button,bool press)
    {
        Vector3 controllerPos;
        Quaternion controllerRot;
        if (button == _leftShootButton)
        {
            controllerPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            controllerRot = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);

        }
        else
        {
            controllerPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            controllerRot = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
        }

        Transform trackingSpace = transform;
        if (OVRManager.instance != null)
        {
            trackingSpace = OVRManager.instance.GetComponent<OVRCameraRig>().trackingSpace;
        }

        Vector3 rayOrigin = trackingSpace.TransformPoint(controllerPos);
        Vector3 rayDirection = trackingSpace.TransformDirection(controllerRot * Vector3.forward);

       
        RaycastHit hit;
        Ray ray = new Ray(rayOrigin, rayDirection);

        if (Physics.Raycast(ray, out hit, _maxRayDistance))
        {
            if (press)
            {
                _gameManagerScriptableObject.OnInteractionDown(ray, hit);
            }
            else
            {
                _gameManagerScriptableObject.OnInteractionRelase(ray, hit);
            }

        }
        else
        {
            Debug.Log("no raycast hit");
        }
    }

    private void PinchRaycast( OVRHand ovrHand) 
    {
        Transform pointerTransform = ovrHand.PointerPose;
        Vector3 origin = pointerTransform.position;
        Vector3 forward = pointerTransform.forward;
        RaycastHit raycastHit;
        Ray ray = new Ray(origin, forward);

        if (Physics.Raycast(ray, out raycastHit, _maxRayDistance))
        {
            
            _gameManagerScriptableObject.OnInteractionDown(ray,raycastHit);
        }
    }

    private void PinchReleaseRaycast(OVRHand ovrHand)
    {
        Transform pointerTransform = ovrHand.PointerPose;
        Vector3 origin = pointerTransform.position;
        Vector3 forward = pointerTransform.forward;
        RaycastHit raycastHit;
        Ray ray = new Ray(origin, forward);

        if (Physics.Raycast(ray, out raycastHit, _maxRayDistance))
        {
            _gameManagerScriptableObject.OnInteractionRelase(ray, raycastHit);
        }
    }



}

