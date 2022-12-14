using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class NPCManager : MonoBehaviour {


    [SerializeField] private GameObject Button_Template;

    private GameObject AllUIObjects;
    private GameObject NPCManagerGameObject;
    private GameObject mainUIGameObject;
    private GameObject mainDisplayGameObject;
    private GameObject mainDisplayContent;

    private GameObject currentResources;

    void Start () {
        AllUIObjects = GameObject.Find ("AllUIObjectsCanvas");
        mainUIGameObject = AllUIObjects.transform.GetChild (0).gameObject;
        NPCManagerGameObject = AllUIObjects.transform.GetChild (1).gameObject;

        mainDisplayGameObject = NPCManagerGameObject.transform.GetChild (0).gameObject;
        mainDisplayContent = mainDisplayGameObject.transform.GetChild (0).GetChild (0).gameObject;

        currentResources = mainUIGameObject.transform.GetChild (2).gameObject;


        this.gameObject.GetComponent<Button> ().onClick.AddListener (() => enableMenu ());
    }

    public void enableMenu () {
        reloadMenu ();
        this.gameObject.SetActive (false);
        NPCManagerGameObject.SetActive (true);
        mainDisplayGameObject.SetActive (true);
    }

    public void disableMenu () {
        this.gameObject.SetActive (true);
        currentResources.GetComponent<currentResourcesUIController> ().updateResourcesUI ();
        mainDisplayGameObject.SetActive (false);
        NPCManagerGameObject.SetActive (false);
    }

    public void reloadMenu() {
        clearUI ();
        generateUI ();
    }

    private void clearUI () {
        foreach (Transform child in mainDisplayContent.transform) {
            Destroy (child.gameObject);
        }
    }

    /// <summary>
    /// Generates the UI for the NPC's.
    /// </summary>
    private void generateUI () {
        GameObject tempGameObject;
        foreach (var theNPC in MetaScript.GetNPC().getNPCs()) {
            tempGameObject = Instantiate (Button_Template) as GameObject;
            tempGameObject.SetActive (true);
            mainDisplay aButtonScript = tempGameObject.GetComponent<mainDisplay> ();
            aButtonScript.setNPC (theNPC);
            tempGameObject.transform.SetParent (mainDisplayContent.transform, false);
        }
    }

}
