using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setRPGTalkHolder : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<RPGTalkArea>().rpgtalkTarget = GameObject.Find("RPGTalkHolder").GetComponent<RPGTalk>();
	}
}
