using UnityEngine;
using System.Collections;

public class Pointer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.Restart();
            }
        }
    }
}
