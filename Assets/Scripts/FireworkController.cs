using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkController : MonoBehaviour
{
    private float lifetime = 1;
    void Start(){
        lifetime = GetComponent<ParticleSystem>().main.duration;
    }

    // Update is called once per frame
    void Update(){
        if(lifetime <= 0){
            Destroy(gameObject);
        }
        else{
            lifetime -= Time.deltaTime;
        }
        
    }
}
