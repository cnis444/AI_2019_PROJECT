using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public abstract class InteractiveObject : MonoBehaviour
{
    public bool highlighted = false;

    private Outline outline;

    private void Awake() {
        outline = GetComponent<Outline>();
    }

    private void FixedUpdate() {
        Highlight();
    }

    #region Highlight

    private void Highlight() {
        if (outline) {
            outline.enabled = highlighted;
        }
        highlighted = false;
    }

    #endregion

    public abstract void DoSomething();
}
