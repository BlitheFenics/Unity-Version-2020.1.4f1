using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Let's you set components in other scripts and have them affected by the same time parameter
public interface WeatherSystem
{
    void GetComponent();

    void SetParameter(float time);
}

// Makes all instances of a script execute in Edit Mode and not just Play Mode
[ExecuteInEditMode]

// Automatically adds AudioSource component as a dependencie
[RequireComponent(typeof(AudioSource))]

public class WeatherController : MonoBehaviour
{
    // Sets the range of time, grabs setters, sets up bools, grabs audio clips from the inspector, and sets an array of GameObjects in this case particle effects for weather
    [Range(0, 4)]
    public float time;
    public WeatherSystem[] setters;
    public bool sunny, rain, weather;
    public GameObject[] WeatherParticles;
    private int particleEffect;
    public AudioClip raining, thunder, sun;

    // This function is called when the object becomes enabled and active
    private void OnEnable()
    {
        StartCoroutine(Sound());
        time = 0;
        weather = true;
        sunny = true;
        rain = false;
        GetSetters();
    }

    // This gets every component that inherits from the WeatherSystem interface
    private void GetSetters()
    {
        setters = GetComponentsInChildren<WeatherSystem>();
        foreach(var setter in setters)
        {
            setter.GetComponent();
        }
    }

    // Gets the audio source component and either plays, stops, or switches the audio clip with time durations for how long each segment should last
    IEnumerator Sound()
    {
        AudioSource ASWeather = GetComponent<AudioSource>();
        ASWeather.Play();
        yield return new WaitForSeconds(2f);
        ASWeather.Stop();
        yield return new WaitForSeconds(2.4f);
        ASWeather.clip = raining;
        ASWeather.Play();
        yield return new WaitForSeconds(9.3f);
        ASWeather.Stop();
        yield return new WaitForSeconds(0f);
        ASWeather.clip = thunder;
        ASWeather.Play();
        yield return new WaitForSeconds(4f);
        ASWeather.clip = raining;
        ASWeather.Play();
        yield return new WaitForSeconds(7.5f);
        ASWeather.clip = thunder;
        ASWeather.Play();
        yield return new WaitForSeconds(4f);
        ASWeather.Stop();
        yield return new WaitForSeconds(3.5f);
        ASWeather.clip = sun;
        ASWeather.Play();
    }
    
    void Update()
    {
        // Looks for setters and appllies time to there parameter
        if (setters.Length > 0)
        {
            foreach (var setter in setters)
            {
                setter.SetParameter(time);
            }
        }

        // triggers the rain particle effect and switches bools at set moments in times range
        if (time < 1f)
        {
            sunny = true;
        }
        if (time > 1f)
        {
            sunny = false;
        }
        if (time > 2)
        {
            particleEffect = 0;
            StartEffect(particleEffect);
            rain = true;  
        }
        if (time < 2)
        {
            StopEffect(particleEffect);
            rain = false; 
        }
        
        // If time goes past 4 which is currently its max range the weather variable will be set to false, if time goes below 0 which is the lowest number in the range the weather variable will be set to true and it will start the cououtine from the begining 
        if (time > 4)
        {
            weather = false;
        }
        if (time < 0)
        {
            StartCoroutine(Sound());
            weather = true; 
        }

        // If weather is true then time will lerp from the current time (which should be 0) and move towards 4.1 at a rate of deltaTime * 0.15 since thats a reasonable transition rate given how small the range is
        if (weather)
        {
            time = Mathf.Lerp(time, 4.1f, Time.deltaTime * 0.15f);
        }

        //// If weather is false then time will lerp from the current time (which should be 4.1) and move towards -0.1 at a rate of deltaTime * 0.15 since thats a reasonable transition rate given how small the range is
        else if (!weather)
        {
            time = Mathf.Lerp(time, -0.1f, Time.deltaTime * 0.15f);
        }
    }

    // These functions take an integer parameter and pull that index from the WeatherParticles array and set whether or not the particle effect is active
    private void StartEffect(int particleEffect)
    {
        WeatherParticles[particleEffect].SetActive(true);
    }

    private void StopEffect(int particleEffect)
    {
        WeatherParticles[particleEffect].SetActive(false);
    }
}
