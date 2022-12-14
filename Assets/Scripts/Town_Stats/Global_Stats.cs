using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global_Stats : MonoBehaviour {
    [SerializeField]
    private float foodSaved = 0;

    [SerializeField]
    private float militaryAbility = 1;

    [SerializeField]
    private float damageMultiplier = 1f;

    [SerializeField]
    private float healthMultiplier = 1f;

    [SerializeField]
    private float gatherMultiplier = 1;

    [SerializeField]
    private float baseGather = 1;

    [SerializeField]
    private bool hasHeatProtection;

    [SerializeField]
    private bool hasColdProtection;


    public float getFoodSaved()
    {
        return foodSaved;
    }

    public void setFoodSaved(float food)
    {
        foodSaved = food;
        publish();
    }

    public float getBaseGather()
    {
        return baseGather;
    }

    public void setBaseGather(float gather)
    {
        baseGather = gather;
        publish();
    }

    public float getMilitaryAbility()
    {
        return militaryAbility;
    }

    public void setMilitaryAbility(float military)
    {
        militaryAbility = military;
        publish();
    }

    public void setAtkMultiplier(float multiplier){
        damageMultiplier += multiplier;
        publish();
    }

    public float getAtkMultiplier(){
        return damageMultiplier;
    }

    public float getHealthMultiplier(){
        return healthMultiplier;
    }

    public void setHealthMultiplier(float multiplier){
        healthMultiplier += multiplier;
        publish();
    }

    //public float getHealthPool()
    //{
    //    return healthPool;
    //}

    //public void setHealthPool(float health)
    //{
    //    healthPool = health;
    //}

    public void setGatherMultiplier(float mult)
    {
        Debug.Log("Setting Gather Mult: "+mult);
        gatherMultiplier = mult;
        if (gatherMultiplier < 0)
        {
            gatherMultiplier = 0;
        }
        publish();
    }

    public float getGatherMultiplier()
    {
        return gatherMultiplier;
    }

    public void setHasHeatProtection(bool has)
    {
        hasHeatProtection = has;
        publish();
    }

    public bool getHasHeatProtection()
    {
        return hasHeatProtection;
    }

    public void setHasColdProtection(bool has)
    {
        hasColdProtection = has;
        publish();
    }

    public bool getHasColdProtection()
    {
        return hasColdProtection;
    }
    
    private List<IStatsListener> subscribers = new List<IStatsListener>();

    void publish(){
        foreach(IStatsListener l in subscribers){
            l.publish(this);
        }
        MetaScript.updateGlobalStatsUI();
    }
}
