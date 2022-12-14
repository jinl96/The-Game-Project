using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour {


	public void updateHUD(){
		ResourceManager res = MetaScript.getRes();
		UnityEngine.UI.Text resHUD = gameObject.GetComponentInChildren<UnityEngine.UI.Text>();

		for(int i=0;i<(int)ResourceTypes.NumberOfTypes;i++){
			resHUD.text = ((ResourceTypes)i).ToString() +":"+ res.getResource(i)+"\n";
		}
		// resHUD.text="Wood: " + res.getResource(ResourceTypes.WOOD)
		// + "\nFood: " + res.getResource(ResourceTypes.FOOD);

	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		updateHUD();
	}
}
