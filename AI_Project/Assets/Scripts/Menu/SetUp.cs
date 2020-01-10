using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUp : MonoBehaviour
{

    public MapParam mapParam;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


}
