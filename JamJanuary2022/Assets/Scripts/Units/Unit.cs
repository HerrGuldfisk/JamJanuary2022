using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{


    public virtual void DestroyUnit()
    {
        Destroy(this.gameObject);
    }
}
