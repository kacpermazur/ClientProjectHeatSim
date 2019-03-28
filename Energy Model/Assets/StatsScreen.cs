using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnToSim()
    {
        Camera.main.GetComponent<Animator>().SetBool("CamUp", true);
        Camera.main.GetComponent<Animator>().SetBool("CamUp", false);
    }
}
