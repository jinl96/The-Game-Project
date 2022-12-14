using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingBell : MonoBehaviour {

	// Use this for initialization
	void Start () {
		controls = MetaScript.GetControls();
	}
	private Controls controls;
	// Update is called once per frame
	void Update(){
		if(controls.keyDown(controls.HideBell)){
			controls.guards = false;
			ring();
		}
	}

	public bool danger = false;
	private void ring(){
		danger = !danger;
		foreach(GameObject g in GetComponent<OwnedNPCList>().getNPCs()){
			g.GetComponent<FightFlight>().forceBell(danger);
		}
	}
}
