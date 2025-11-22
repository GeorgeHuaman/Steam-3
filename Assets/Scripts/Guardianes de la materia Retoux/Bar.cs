using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [Header("Barra")]
    public Image barFill;
    public float barAmount = 100f;
    public float durationAnim = 0.5f;

    [Header("Oscilacion")]
    public float oscillationMin = 85f;
    public float oscillationMax = 97f;
    public float oscillationPeriod = 2f;
    public float startDelay = 0.5f;

    public float changeDuration = 0.5f;

    private Coroutine oscillationRoutine;
    private Coroutine changeRoutine;
    private bool locked = false;

    private void Start()
    {
        barAmount = Mathf.Clamp(barAmount, 0f, 100f);
        barFill.fillAmount = oscillationMin / 100f;

        oscillationRoutine = StartCoroutine(Oscillate());
    }

    public void ChangeAmount(float newAmount)
    {
        newAmount = Mathf.Clamp(newAmount, 0f, 100f);

        locked = true;

        if (oscillationRoutine != null)
        {
            StopCoroutine(oscillationRoutine);
            oscillationRoutine = null;
        }

        if (changeRoutine != null)
        {
            StopCoroutine(changeRoutine);
        }

        changeRoutine = StartCoroutine(AnimateBarTo(newAmount));
    }

    private IEnumerator AnimateBarTo(float targetAmount)
    {
        float startAmount = barAmount;
        float elapsed = 0f;

        if (changeDuration <= 0f)
        {
            barAmount = targetAmount;
            barFill.fillAmount = barAmount / 100f;
            yield break;
        }

        while (elapsed < changeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / changeDuration);

            barAmount = Mathf.Lerp(startAmount, targetAmount, t);
            barFill.fillAmount = barAmount / 100f;
            yield return null;
        }

        barAmount = targetAmount;
        barFill.fillAmount = barAmount / 100f;
        changeRoutine = null;
    }

    private IEnumerator Oscillate()
    {
        if (startDelay > 0f)
        {
            yield return new WaitForSeconds(startDelay);
        }
        float t = 0f;
        while (true)
        {
            if (locked)
                yield break;

            float ping = Mathf.PingPong(t / (oscillationPeriod * 0.5f), 1f);
            float value = Mathf.Lerp(oscillationMin, oscillationMax, ping);

            barAmount = value;
            barFill.fillAmount = barAmount / 100f;

            t += Time.deltaTime;
            yield return null;
        }
    }
}
