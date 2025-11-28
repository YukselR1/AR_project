using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void buttonAnim()
    {
        GetComponent<Animation>().Play("buttonanim");
    }
}
