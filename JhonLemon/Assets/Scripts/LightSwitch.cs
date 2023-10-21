using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    private bool PlayerInZone;

    public GameObject lightorobj;
    public GameObject lightorobj2;
    // Start is called before the first frame update
    void Start()
    {
        PlayerInZone = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (PlayerInZone && Input.GetKeyDown(KeyCode.F))
        {
            lightorobj.SetActive(true);
            lightorobj2.SetActive(true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            PlayerInZone = true;
        }

    }
}