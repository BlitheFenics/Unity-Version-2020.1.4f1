using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

// Inherits from WeatherSystem interface
public class LightController : MonoBehaviour,WeatherSystem
{
    // Lets you change the light based on the gradient, and gets a reference to the 2D lights
    public Gradient gradient;
    public Light2D[] lights;

    // Gets all components on the type Light2D
    public void GetComponent()
    {
        lights = GetComponentsInChildren<Light2D>();
    }

    // Sets the color for each light in the list based on the gradient value which changes with time
    public void SetParameter(float time)
    {
        foreach (var light in lights)
        {
            light.color = gradient.Evaluate(time * 0.25f);
        }
    }
}
