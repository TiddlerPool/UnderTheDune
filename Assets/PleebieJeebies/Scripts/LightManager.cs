using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LightManager : MonoBehaviour
{
    [SerializeField, Header("Managed Objects")] private Light DirectionalLight = null;
    [SerializeField] private LightPreset DayNightPreset, LampPreset;
    [SerializeField] private List<Light> SpotLights = new List<Light>();

    [SerializeField] private int currentID;
    [SerializeField] private bool musicStopped;

    [SerializeField, Range(0, 1440), Header("Modifiers"), Tooltip("The game's current time of day")] private float timeOfDay;
    public float TimeOfDay
    {
        get { return timeOfDay; }
        set { timeOfDay = value; }
    }

    [SerializeField] private int days = 1;
    public int Days
    {
        get { return days; }
        set { days = value; }
    }

    [SerializeField, Tooltip("Angle to rotate the sun")] private float SunDirection = 170f;
    [SerializeField, Tooltip("How fast time will go")] private float timeMultiplier = 1;
    public float TimeMultiplier
    {
        get { return timeMultiplier; }
        set { timeMultiplier = value; }
    }
    [SerializeField] private bool ControlLights = true;

    private const float inverseDayLength = 1f / 1440f;

    /// <summary>
    /// On project start, if controlLights is true, collect all non-directional lights in the current scene and place in a list
    /// </summary>
    private void Start()
    {
        if (ControlLights)
        {
            Light[] lights = FindObjectsOfType<Light>();
            foreach (Light li in lights)
            {
                switch (li.type)
                {
                    case LightType.Disc:
                    case LightType.Point:
                    case LightType.Rectangle:
                    case LightType.Spot:
                        SpotLights.Add(li);
                        break;
                    case LightType.Directional:
                    default:
                        break;
                }
            }
        }

        AudioEvents.current.onMusicStopped += MusicStop;
    }

    /// <summary>
    /// This method will not run if there is no preset set
    /// On each frame, this will calculate the current time of day factoring game time and the time multiplier (1440 is how many minutes exist in a day 24 x 60)
    /// Then send a time percentage to UpdateLighting, to evaluate according to the set preset, what that time of day should look like
    /// </summary>
    private void Update()
    {
        if (DayNightPreset == null)
            return;

        timeOfDay = timeOfDay + (Time.deltaTime * timeMultiplier);
        if (timeOfDay > 1440)
        {
            timeOfDay = 0f;
            days++;
        }
        UpdateLighting(timeOfDay * inverseDayLength);
        UpdateMusic();
    }

    /// <summary>
    /// Based on the time percentage recieved, set the current scene's render settings and light coloring to the preset
    /// In addition, rotate the directional light (the sun) according to the current time
    /// </summary>
    /// <param name="timePercent"></param>
    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = DayNightPreset.AmbientColour.Evaluate(timePercent);
        RenderSettings.fogColor = DayNightPreset.FogColour.Evaluate(timePercent);

        //Set the directional light (the sun) according to the time percent
        if (DirectionalLight != null)
        {
            if (DirectionalLight.enabled == true)
            {
                DirectionalLight.color = DayNightPreset.DirectionalColour.Evaluate(timePercent);
                DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3(SunlightAngle(timePercent), SunDirection, 0));
            }
        }

        //Go through each spot light, ensure it is active, and set it's color accordingly
        foreach (Light lamp in SpotLights)
        {
            if (lamp != null)
            {
                if(lamp.CompareTag("CampfireLight"))
                {
                    continue;
                }
                if (lamp.isActiveAndEnabled && lamp.shadows != LightShadows.None && LampPreset != null)
                {
                    lamp.color = LampPreset.DirectionalColour.Evaluate(timePercent);
                }
            }
        }
    }

    private float SunlightAngle(float timePercent)
    {
        float sunAngle = -16f;
        if(timePercent >= 0 && timePercent < 0.25f)
        {
            sunAngle = timePercent * -360f + 74f;
        }

        if(timePercent >= 0.25f && timePercent < 0.75f)
        {
            sunAngle = timePercent * -360f + 254f;
        }

        if(timePercent >= 0.75f && timePercent <= 1f)
        {
            sunAngle = timePercent * -360f + 344f;
        }
        
        return sunAngle;
    }

    private void UpdateMusic()
    {
        int time = Mathf.RoundToInt(timeOfDay);
        int[] triggerPoints = { 360, 720, 1080, 1440 };
        string[] clipName = { "DesertDay1", "DesertDay2", "DesertNight1", "DesertNight2" };

        var closestNumber = triggerPoints.OrderBy(num => Mathf.Abs(num - timeOfDay)).First();
        int id = (closestNumber / 360) - 1;
        currentID = id;

        if(musicStopped)
        {
            AudioManager.PlayMusic(clipName[id], AudioManager.library);
            musicStopped = false;
        }

        if (timeMultiplier > 1)
        {
            AudioEvents.current.TimeAccelerate();
        }
    }

    private void MusicStop()
    {
        musicStopped = true;
    }
}