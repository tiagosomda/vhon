using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehavior : MonoBehaviour
{
    public void DefaultTrigger(bool flag)
    {
        if (flag)
        {
            transform.localScale = new Vector3(.25f, .25f, .25f);
        }
        else
        {
            transform.localScale = new Vector3(.1f, .1f, .1f);
        }
    }
}
