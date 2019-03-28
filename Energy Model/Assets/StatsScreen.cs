using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsScreen : MonoBehaviour
{
    public GameObject settingsMenu;
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
        StartCoroutine(ShowSettings());
        Camera.main.GetComponent<Animator>().SetBool("CamUp", true);
        Camera.main.GetComponent<Animator>().SetBool("CamDown", false);
        //Camera.main.GetComponent<Animator>().SetBool("CamUp", false);
    }

    IEnumerator ShowSettings()
    {
        yield return new WaitForSecondsRealtime(1);
        settingsMenu.SetActive(true);
        settingsMenu.GetComponent<Animator>().SetBool("DropIn", true);

    }
}
