using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sahneGecis : MonoBehaviour
{
    public void sahneYukle(int index)
    {
        SceneManager.LoadScene(index);
    }
    

}
