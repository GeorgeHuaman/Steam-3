using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmController : MonoBehaviour
{
    [Header("Lights")]
    public List<Light> alarmLights = new List<Light>();

    [Header("Intensity Settings")]
    public float baseIntensity = 0.5f;
    public float maxIntensity = 4f;
    public float pulseSpeed = 1.2f;
    public AnimationCurve intensityCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Color settings")]
    public Color normalColor = new Color(1f, 0.12f, 0.12f);
    public Color flickerColor = new Color(1f, 0.2f, 0.2f);

    [Header("Emission (renderers)")]
    public string emissionColorProperty = "_EmissionColor";
    public float emissionMin = 0.2f;
    public float emissionMax = 5f;

    [Header("Random flicker")]
    public float randomFlickerAmount = 0.3f;

    [Header("Audio")]
    public AudioSource alarmAudio;
    public AudioClip alarmClip;

    [Header("Behavio")]

    public bool alarmActive = true;

    public List<Material> runtimeMats = new List<Material>();

    private void Awake()
    {
        if (alarmAudio == null) alarmAudio = GetComponent<AudioSource>();

        if (alarmAudio != null && alarmClip != null)
        {
            alarmAudio.clip = alarmClip;
            alarmAudio.loop = true;
        }
    }

    void OnEnable()
    {
        if (alarmAudio != null && alarmClip != null) alarmAudio.Play();
    }

    void OnDisable()
    {
        if (alarmAudio != null) alarmAudio.Stop();
    }

    void Update()
    {
        if (!alarmActive) return;

        float t = Time.time * pulseSpeed;

        // pulso base con curve + pingpong para loop suave
        float curveEval = intensityCurve.Evaluate(Mathf.PingPong(t, 1f));

        for (int i = 0; i < alarmLights.Count; i++)
        {
            var L = alarmLights[i];
            if (L == null) continue;

            // introduce una pequeña variación por luz (para no ser idénticas)
            float variation = Mathf.PerlinNoise(i * 10f, t) * randomFlickerAmount;

            float intensity = Mathf.Lerp(baseIntensity, maxIntensity, curveEval + variation);
            L.intensity = intensity;

            // color ligero alternado para darle pulso
            float cBlend = Mathf.Clamp01(curveEval + variation * 0.5f);
            L.color = Color.Lerp(normalColor, flickerColor, cBlend);
        }

        // actualizar emisión en materiales
        foreach (var mat in runtimeMats)
        {
            if (mat == null) continue;
            float e = Mathf.Lerp(emissionMin, emissionMax, curveEval + Mathf.PerlinNoise(Time.time * 1.3f, 0f) * 0.2f);
            Color emColor = normalColor * e;
            if (mat.HasProperty(emissionColorProperty))
                mat.SetColor(emissionColorProperty, emColor);
        }
    }

    public void StopAlarm()
    {
        alarmActive = false;
        if (alarmAudio != null) alarmAudio.Stop();

        // opcional: dejar las luces en estado estable (rojo tenue)
        foreach (var L in alarmLights) if (L != null) L.intensity = baseIntensity;
        foreach (var mat in runtimeMats) if (mat != null && mat.HasProperty(emissionColorProperty))
                mat.SetColor(emissionColorProperty, normalColor * emissionMin);
    }

}
