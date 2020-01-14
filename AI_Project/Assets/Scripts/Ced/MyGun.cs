using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGun : MonoBehaviour
{

    public float range;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray r = new Ray(transform.position, transform.forward);
            Debug.DrawRay(transform.position, transform.forward * range, Color.red, 2f);
            RaycastHit[] targets = Physics.RaycastAll(r, range);
            foreach (RaycastHit item in targets)
            {
                if(item.transform.tag == "Bad")
                {
                    Destroy(item.transform.gameObject);
                }
            }
        }
    }
}
