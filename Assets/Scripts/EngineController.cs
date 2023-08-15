using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineController : MonoBehaviour
{
    private ParticleSystem fire;
    //private float timer = 5;

    void Awake(){
        fire = GetComponentInChildren<ParticleSystem>();

    }

    // Update is called once per frame
    void Update(){
        // if(timer < 0){
        //     Launch();
        // }
        // else{
        //     timer -= Time.deltaTime;
        // }
        
    }

    public void Launch(){
        fire.Play();
    }
}
