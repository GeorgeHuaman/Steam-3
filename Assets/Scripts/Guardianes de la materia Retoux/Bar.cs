using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [Header("Barra")]
    public Image barFill;
    [Range(0f, 100f)] public float barAmount = 100f;
    public float durationAnim = 0.5f;

    [Header("Oscilacion (inicial)")]
    public float oscillationMin = 85f;
    public float oscillationMax = 97f;
    public float oscillationPeriod = 2f;
    public float startDelay = 0.5f;

    [Header("Cambio")]
    public float changeDuration = 0.5f;

    [Header("Oscilación calma después de ChangeAmount")]
    public float calmRange = 10f;   // cuánto por debajo del objetivo oscilar (ej. target - calmRange)
    public float calmPeriod = 3f;   // período de la oscilación calma

    private Coroutine oscillationRoutine;      // oscilación inicial
    private Coroutine changeRoutine;           // animación hacia el objetivo
    private Coroutine calmOscillationRoutine;  // oscilación calma tras ChangeAmount
    private bool locked = false;               // indica que la oscilación inicial fue detenida

    private void Start()
    {
        barAmount = Mathf.Clamp(barAmount, 0f, 100f);
        barFill.fillAmount = barAmount / 100f;

        // Inicia la oscilación "normal" después del startDelay
        oscillationRoutine = StartCoroutine(Oscillate());
    }

    /// <summary>
    /// Llamar para animar la barra hacia newAmount. Al llegar, entrará en una oscilación calmada
    /// entre (newAmount - calmRange) y newAmount.
    /// </summary>
    public void ChangeAmount(float newAmount)
    {
        newAmount = Mathf.Clamp(newAmount, 0f, 100f);

        // Marcamos para detener la oscilación inicial
        locked = true;

        // Detener oscilación inicial si está corriendo
        if (oscillationRoutine != null)
        {
            StopCoroutine(oscillationRoutine);
            oscillationRoutine = null;
        }

        // Detener animación previa si existe
        if (changeRoutine != null)
        {
            StopCoroutine(changeRoutine);
            changeRoutine = null;
        }

        // Detener cualquier oscilación calma previa
        if (calmOscillationRoutine != null)
        {
            StopCoroutine(calmOscillationRoutine);
            calmOscillationRoutine = null;
        }

        // Iniciar la animación hacia el nuevo valor. Al completar, arrancará la oscilación calma.
        changeRoutine = StartCoroutine(AnimateBarToAndCalmOscillate(newAmount));
    }

    private IEnumerator AnimateBarToAndCalmOscillate(float targetAmount)
    {
        float startAmount = barAmount;
        float elapsed = 0f;

        // Si changeDuration es 0, saltamos la animación y vamos directo a la oscilación calma
        if (changeDuration <= 0f)
        {
            barAmount = targetAmount;
            barFill.fillAmount = barAmount / 100f;
        }
        else
        {
            while (elapsed < changeDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / changeDuration);
                // Interpolación lineal; puedes cambiar a SmoothStep si prefieres suavizado
                barAmount = Mathf.Lerp(startAmount, targetAmount, t);
                barFill.fillAmount = barAmount / 100f;
                yield return null;
            }

            barAmount = targetAmount;
            barFill.fillAmount = barAmount / 100f;
        }

        changeRoutine = null;

        // Calcular rango de la oscilación calma
        float calmMin = Mathf.Clamp(targetAmount - calmRange, 0f, 100f);
        float calmMax = targetAmount; // oscilamos hasta el target

        // Si calmRange es 0 o negativo, no empezamos la oscilación calma
        if (calmRange > 0f && calmMin < calmMax)
        {
            calmOscillationRoutine = StartCoroutine(CalmOscillate(calmMin, calmMax, calmPeriod));
        }
    }

    private IEnumerator CalmOscillate(float minValue, float maxValue, float period)
    {
        float t = 0f;
        // Empezamos desde el valor actual (ayuda a transición suave)
        float start = barAmount;

        // si el valor actual está fuera del nuevo rango, ajustamos inmediatamente al borde más cercano
        if (barAmount > maxValue) barAmount = maxValue;
        if (barAmount < minValue) barAmount = minValue;

        while (true)
        {
            // Oscilación entre minValue y maxValue usando PingPong.
            float ping = Mathf.PingPong(t / (period * 0.5f), 1f);
            float value = Mathf.Lerp(minValue, maxValue, ping);

            barAmount = value;
            barFill.fillAmount = barAmount / 100f;

            t += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator Oscillate()
    {
        if (startDelay > 0f)
            yield return new WaitForSeconds(startDelay);

        float t = 0f;
        while (true)
        {
            if (locked) // si locked true, detenemos esta oscilación (la inicial)
                yield break;

            float ping = Mathf.PingPong(t / (oscillationPeriod * 0.5f), 1f);
            float value = Mathf.Lerp(oscillationMin, oscillationMax, ping);

            barAmount = value;
            barFill.fillAmount = barAmount / 100f;

            t += Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// (Opcional) Si quieres volver a la oscilación inicial en cualquier momento.
    /// Detendrá cualquier oscilación calma o animación pendiente y reiniciará la oscilación normal.
    /// </summary>
    public void ResumeInitialOscillation()
    {
        // Detener change animation y calm oscillation si existen
        if (changeRoutine != null)
        {
            StopCoroutine(changeRoutine);
            changeRoutine = null;
        }

        if (calmOscillationRoutine != null)
        {
            StopCoroutine(calmOscillationRoutine);
            calmOscillationRoutine = null;
        }

        locked = false;

        // Iniciar la oscilación inicial (con delay si lo deseas)
        if (oscillationRoutine == null)
            oscillationRoutine = StartCoroutine(Oscillate());
    }
}