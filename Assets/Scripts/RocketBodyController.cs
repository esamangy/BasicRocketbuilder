using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class RocketBodyController : MonoBehaviour
{
    [Header("References")]
    private List<Collider> triggers = new List<Collider>();
    private List<Collider> otherParts = new List<Collider>();
    private List<GameObject> ConnectedParts = new List<GameObject>();
    [SerializeField] private TMP_Text launchText;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject firework;

    [Header("part Locations")]
    private Vector3 engineloc = new Vector3(0f, -.42f, 0f);
    private Vector3 caploc = new Vector3(0f, .9f, 0f);
    [Header("Gameplay")]
    [SerializeField] private float launchAcceleration;
    private float launchSpeed;
    [SerializeField] private float LaunchTime;
    //private float LaunchTimer = 5;
    [SerializeField] private float FlyTime;
    
    void Start(){
        Collider [] t = GetComponentsInChildren<BoxCollider>();
        foreach (Collider g in t){
            triggers.Add(g);
        }
    }

    void FixedUpdate(){
        List <Collider> toRemove = new List<Collider>();
        if (otherParts.Count != 0){
            foreach (Collider t in triggers){
                if(t.transform.gameObject.tag == "Fin"){
                    foreach (Collider o in otherParts){
                        if (o.transform.gameObject.tag == "Fin"){
                            if (t.bounds.Contains(o.transform.position)) {
                                Destroy(o.transform.gameObject.transform.parent.GetComponent<Rigidbody>());
                                player.GetComponent<PlayerController>().removeHeldObject();
                                o.transform.gameObject.transform.parent.transform.position = t.transform.gameObject.transform.position + t.transform.gameObject.transform.up * .4f + -t.transform.gameObject.transform.forward * .1f;
                                o.transform.gameObject.transform.parent.transform.LookAt(t.transform.parent.transform.parent.transform.position + -t.transform.gameObject.transform.up * 12f, t.transform.parent.transform.parent.transform.up);
                                o.transform.parent.gameObject.transform.parent = transform;
                                ConnectedParts.Add(o.transform.parent.transform.gameObject);
                                toRemove.Add(o);
                                t.enabled = false; 
                            }
                        }
                    }
                }
                if(t.transform.gameObject.tag == "Cap"){
                    foreach (Collider o in otherParts){
                        if (o.transform.gameObject.tag == "Cap"){
                            if (t.bounds.Contains(o.transform.position)) {
                                Destroy(o.transform.gameObject.transform.parent.GetComponent<Rigidbody>());
                                player.GetComponent<PlayerController>().removeHeldObject();
                                o.transform.parent.transform.position = gameObject.transform.position + t.transform.gameObject.transform.forward * .9f;
                                o.transform.parent.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles);
                                o.transform.parent.gameObject.transform.parent = transform;
                                ConnectedParts.Add(o.transform.parent.transform.gameObject);
                                toRemove.Add(o);
                                t.enabled = false;
                            }
                        }
                    }
                }
                if(t.transform.gameObject.tag == "Engine"){
                    foreach (Collider o in otherParts){
                        if (o.transform.gameObject.tag == "Engine"){
                            if (t.bounds.Contains(o.transform.position)) {
                                Destroy(o.transform.gameObject.transform.parent.GetComponent<Rigidbody>());
                                player.GetComponent<PlayerController>().removeHeldObject();
                                o.transform.root.transform.position = gameObject.transform.position + -t.transform.gameObject.transform.forward * .4f;
                                o.transform.root.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles);;
                                o.transform.root.transform.parent = transform;
                                ConnectedParts.Add(o.transform.parent.transform.parent.transform.gameObject);
                                toRemove.Add(o);
                                t.enabled = false;
                            }
                        }
                    }
                }
            }
            foreach (Collider c in toRemove){
                otherParts.Remove(c);
            }
        }

        if(ConnectedParts.Count == 6){
            Launch();
        }
        // else if (LaunchTimer <= 0){
        //     Launch();
        // }
        // else{
        //     LaunchTimer -= Time.deltaTime;
        // }
    }

    void Launch(){
        if(FlyTime <= 0){
            Explode();
        }
        if(LaunchTime <= 0){
            launchText.text = "";
            player.GetComponent<PlayerController>().removeHeldObject();
            GetComponentInChildren<EngineController>().Launch();
            // PartController[] parts = GetComponentsInChildren<PartController>();
            // foreach(PartController p in parts){
            //     p.Freeze(false);
            // }
            Vector3 dir = transform.up;
            launchSpeed += launchAcceleration * Time.deltaTime;
            transform.position = transform.position + (dir * launchSpeed);
            FlyTime -= Time.deltaTime;
        }
        else{
            LaunchTime -= Time.deltaTime;
            launchText.text = "T - " + Mathf.Round(LaunchTime * 10.0f) * 0.1f;
        }
        
    }

    void Explode(){
        Instantiate(firework, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other){
        otherParts.Add(other);
        //Debug.Log("part added");
    }

    void OnTriggerExit(Collider other){
        otherParts.Remove(other);
        //Debug.Log("part removed");
    }
}
