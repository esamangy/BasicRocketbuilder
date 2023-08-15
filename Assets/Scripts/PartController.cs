using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartController : MonoBehaviour
{
    private Outline outliner;
    private Rigidbody body;
    private float timer = 0;

    void Awake(){
        outliner = GetComponent<Outline>();
        body = GetComponent<Rigidbody>();
    }

    void Start(){
        
    }

    void Update(){
        Debug.DrawRay(transform.position, transform.up, Color.black);
        Debug.DrawRay(transform.position, -transform.forward, Color.green);

        if (timer > 0){
            timer -= Time.deltaTime;
        }
        else{
            outliner.OutlineColor = Color.clear;
        }
        
    }

    public void Freeze(bool flip){
        if(flip){
            GetComponent<Rigidbody>().isKinematic = !GetComponent<Rigidbody>().isKinematic;
        }
        else{
            GetComponent<Rigidbody>().isKinematic = true;
        }
        
    }

    public void Highlight(Color color){
        outliner.OutlineColor = color;
        timer = .01f;
    }
    
    // public void setFollow(GameObject follow, Vector3 followpos){
    //     followbody = follow;
    // }
}
