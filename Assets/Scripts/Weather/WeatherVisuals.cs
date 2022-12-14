using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherVisuals : MonoBehaviour {

    public GameObject acidRain;
    public GameObject rain;
    public GameObject snow;
    public GameObject blizzard;
    public GameObject cloudy;

    private GameObject player;

    private void Start()
    {
        player = MetaScript.getPlayer();
    }

    public void updateWeatherParticles(Weather.weatherTypes weather)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ParticleSystem currentParticleScript = player.GetComponentInChildren<ParticleSystem>();
        GameObject currentParticle = null;
        if (currentParticleScript != null)
        {
            currentParticle = currentParticleScript.gameObject;
        }
        GameObject newParticle = getParticleFromWeather(weather);

        if (!givenParticlesHaveSameName(currentParticle, newParticle))
        {
            if (currentParticle != null)
            {
                Destroy(currentParticle);
            }
            if (newParticle != null)
            {
                GameObject part = Instantiate(newParticle, player.transform);
                ParticleSystem sys = part.GetComponent<ParticleSystem>();
                sys.collision.SetPlane(0, player.transform);

                if(player.GetComponent<InBuilding>().getPlayerInBuilding())
                {
                    sys.Stop();
                }
            }
        }
    }

    private bool givenParticlesHaveSameName(GameObject a, GameObject b)
    {
        if (a == null || b == null)
        {
            return false;
        }

        string aName = a.name.Replace("(Clone)", "");
        string bName = b.name.Replace("(Clone)", "");
        return aName.Equals(bName);
    }

    private GameObject getParticleFromWeather(Weather.weatherTypes weather)
    {
        GameObject particle = null;
        switch (weather)
        {
            case Weather.weatherTypes.ACID_RAIN:
                particle = acidRain;
                break;
            case Weather.weatherTypes.BLIZZARD:
                particle = blizzard;
                break;
            case Weather.weatherTypes.CLOUDY:
                particle = cloudy;
                break;
            case Weather.weatherTypes.RAIN:
                particle = rain;
                break;
            case Weather.weatherTypes.SNOW:
                particle = snow;
                break;
            default:
                particle = null;
                break;
        }

        return particle;
    }

}
