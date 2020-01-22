using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractiveObject
{
    private bool opened = false;

    public override void DoSomething() {
        if (opened) {
            Close();
        }
        else {
            Open();
        }
    }

    private void Open() {
        gameObject.SetActive(false);
    }

    private void Close() {
        gameObject.SetActive(true);
    }
}
