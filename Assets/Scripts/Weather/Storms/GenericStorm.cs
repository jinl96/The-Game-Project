using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericStorm : MonoBehaviour {

	public virtual void removeStorm()
    {
        Destroy(gameObject);
    }
}
