using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.OpenXR;
using Microsoft.MixedReality.Toolkit.Utilities;
using Handedness = Microsoft.MixedReality.Toolkit.Utilities.Handedness;
using Microsoft.MixedReality.Toolkit;

public class InputController : MonoBehaviour, IMixedRealityPointerHandler
{
    Rigidbody rigidBody;

    Vector3? rightStartPoint;
    Vector3? leftStartPoint;

    public float minZoom = -2;
    public float maxZoom = 2;
    private void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
    }
    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        rightStartPoint = eventData.Pointer.Position;

        (leftStartPoint, rightStartPoint) = getHandsPosition();

        AppManager.instance.hideAllNewsPanel();
    }

    bool isRight;
    bool isLeft;
    float oldHandsDistance;

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
        Vector3? leftPos = null;
        Vector3? rightPos = null;
        (leftPos, rightPos) = getHandsPosition();

        //rotate
        if (rightPos != null && leftPos == null)
        {
            if (rightStartPoint != null)
            {
                float deltaX = rightStartPoint.Value.x - eventData.Pointer.Position.x;
                float deltaY = rightStartPoint.Value.y - eventData.Pointer.Position.y;
                Vector3 delta = eventData.Pointer.Position - rightStartPoint.Value;
                // transform.localRotation =Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, deltaX*150, 0),2*Time.deltaTime);

                //ussing physic for better user experienc of rotating the earth
                rigidBody.AddTorque(-deltaY * 3, deltaX * 3,0, ForceMode.Acceleration);
               
            }
        }

        //zoom
        if ((rightStartPoint != null && leftStartPoint != null) && (rightPos != null && leftPos != null))
        {
            float delta = leftPos.Value.x - rightPos.Value.x;
            Vector3 pos = transform.position;

            Vector3 newPos = new Vector3(pos.x, pos.y, pos.z + delta * 1.5f);
            newPos.z = Mathf.Clamp(newPos.z, minZoom, maxZoom);

          //  if (Mathf.Abs(delta - oldHandsDistance) > .0001f)
                transform.position = Vector3.Lerp(transform.position, newPos, .2f * Time.deltaTime);
          
            oldHandsDistance = delta;
        }

    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
        rightStartPoint = null;
        leftStartPoint = null;
        oldHandsDistance = 0;
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        
    }


    (Vector3?, Vector3?) getHandsPosition()
    {
        Vector3? _leftHand = null;
        Vector3? _rightHand = null;
        foreach (var controller in CoreServices.InputSystem.DetectedInputSources)
        {
            if (controller.SourceType == InputSourceType.Controller || controller.SourceType == InputSourceType.Hand)
            {
                foreach (var pointer in controller.Pointers)
                {
                    if (pointer is IMixedRealityNearPointer)
                    {
                        //   continue;
                    }
                    if (pointer.Result != null)
                    {
                        if (pointer.IsActive)
                        {
                            if (pointer.PointerName.ToLower().Contains("left"))
                            {
                                _leftHand = pointer.Position;

                            }
                            if (pointer.PointerName.ToLower().Contains("right"))
                            {
                                _rightHand = pointer.Position;
                            }
                        }
                    }
                }
            }

        }
        return (_leftHand, _rightHand);
    }


}
