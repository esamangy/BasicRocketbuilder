using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : MonoBehaviour{
    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera cam;
    private bool CamLock = false;

    [Header("References")]
    [SerializeField] private GameObject Player;
    private GameObject heldObject;

    [Header("Gameplay")]
    [SerializeField] private float rotateSpeed;
    private bool grabbing = false;
    private float heldDist = -1;
    private Vector3 rotateDir = new Vector3(0, 0, 0);

    void Start(){
        cam.GetComponent<CinemachineInputProvider>().enabled = false;
    
    }

    void Update(){
        if(!CamLock){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100)){
                try{
                    if (!grabbing){
                        hit.transform.gameObject.GetComponent<PartController>().Highlight(Color.white);
                    }
                    else{
                        hit.transform.gameObject.GetComponent<PartController>().Highlight(Color.black);
                    }
                    
                }catch{

                }
                
            }
        }
    }

    void FixedUpdate(){
        if(grabbing){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 mydirection = ray.direction * heldDist;
            Vector3 moveto;
            moveto.x = transform.position.x + mydirection.x;
            moveto.y = transform.position.y + mydirection.y - .15f;
            moveto.z = transform.position.z + mydirection.z;
            heldObject.transform.position = moveto;

            heldObject.transform.Rotate((Vector3.forward * rotateDir.y * rotateSpeed));
            heldObject.transform.Rotate(Vector3.right * rotateDir.z * rotateSpeed);
            heldObject.transform.Rotate(Vector3.up * -rotateDir.x * rotateSpeed);
        }
    }

    void OnLookLock(InputValue value){
        CamLock = value.isPressed;
        if(CamLock){
            Cursor.lockState = CursorLockMode.Locked;
            cam.GetComponent<CinemachineInputProvider>().enabled = true;
        }
        else{
            Cursor.lockState = CursorLockMode.None;
            cam.GetComponent<CinemachineInputProvider>().enabled = false;
        }
    }

    void OnGrab(InputValue value){
        grabbing = value.isPressed;
        if(grabbing){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100)){
                if(hit.transform.gameObject.tag == "Part"){
                    heldObject = hit.transform.gameObject;
                    heldObject.GetComponent<PartController>().Freeze(false);
                    heldDist = hit.distance + .5f;
                }
                else{
                    grabbing = false;
                }
            }
            else{
                grabbing = false;
            }
        }
        else{
            heldObject = null;
        }
        
    }

    void OnScroll(InputValue value){
        if(grabbing){
            heldDist += value.Get<Vector2>().y * .1f;
        }
    }

    void OnFreeze(InputValue value){
        try{
            heldObject.GetComponent<PartController>().Freeze(true);
        }catch{

        }
    }

    void OnYaw(InputValue value){
        rotateDir.z = value.Get<float>();
    }

    void OnRoll(InputValue value){
        rotateDir.x = value.Get<float>();
    }
    void OnPitch(InputValue value){
        rotateDir.y = value.Get<float>();
    }

    public void removeHeldObject(){
        heldObject = null;
        grabbing = false;
    }

}
