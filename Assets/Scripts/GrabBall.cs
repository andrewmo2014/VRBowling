using UnityEngine;
using System.Collections;

public class GrabBall : MonoBehaviour
{
    public SteamVR_TrackedObject controller;  //Reference to the controller
    private GameObject selectedObject;
    private GameObject grabbedObject;

    void OnTriggerEnter(Collider other)
    {
        selectedObject = other.gameObject; //Select an object by touching it
    }

    void OnTriggerExit(Collider other)
    {
        if (selectedObject == other.gameObject)
        {
            selectedObject = null; //Deselect the last object you touched
        }
    }

    void FixedUpdate()
    {
        //Get a reference to the steam controller we drag to the script:
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)controller.index);
        //If we have a grabbedObject selected then align it with the transform.position.
        if (grabbedObject != null)
        {
            grabbedObject.transform.position = this.gameObject.transform.position;
        }
        //If you press the trigger down while an object is selected and not grabbed,
        //Mark the object as grabbed and move it to the hand.
        if ((selectedObject != null && grabbedObject == null) && device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            grabbedObject = selectedObject;
            grabbedObject.transform.position = this.gameObject.transform.position;
        }
        else if (grabbedObject != null && device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            //If we let go of the trigger while an object is grabbed then apply physics
            var rigidbody = grabbedObject.GetComponent<Rigidbody>();
            //Apply the current device velocity to the object to throw it, we multiply 4 to make it
            //Easier to throw.              
            rigidbody.velocity = grabbedObject.transform.TransformVector(device.velocity * 4);

            //Apply the angular velocity from the device as well, also multiply by 4.            
            rigidbody.angularVelocity = grabbedObject.transform.TransformVector(device.angularVelocity * 4);
            rigidbody.maxAngularVelocity = rigidbody.angularVelocity.magnitude;

            //Now that the object has been thrown lets reset selection and grabbedObject so that we can select a new object!
            selectedObject = null;
            grabbedObject = null;
        }
    }
}
