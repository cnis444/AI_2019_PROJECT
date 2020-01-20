using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUp : MonoBehaviour
{

    public MapParam mapParam;
    public SetOfBulding setOfBulding;
    public SetOfItem setOfItem;
    public SetOfNPC setOfNPC;
    public SetOfQuest setOfQuest;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


}
