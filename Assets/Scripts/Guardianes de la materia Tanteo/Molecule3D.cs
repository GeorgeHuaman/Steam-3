using SpatialSys.UnitySDK;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
public class Molecule3D : MonoBehaviour
{
    [Header("Composición base (por ejemplo N2 -> N:2)")]
    public List<ElementCount> baseComposition = new List<ElementCount>();

    [Header("Coeficiente (multiplicador)")]
    [Min(1)]
    public int coefficient = 1; // 1 significa que no se muestra el número

    [Header("Límites del coeficiente")]
    public int minCoefficient = 1; // mínimo visible (1 no se muestra)
    public int maxCoefficient = 9;

    [Header("Referencia visual (TextMeshPro en World Space)")]
    public TMP_Text coefText;

    [Header("Eventos")]
    public UnityEvent onCoefficientChanged;   
    public UnityEvent onBalancedCorrectly;
    public UnityEvent onUnbalanced;

    public SpatialInteractable interactable;

    private void Start()
    {
        interactable = GetComponent<SpatialInteractable>();
        interactable.onInteractEvent.unityEvent.AddListener(Interact);
    }
    public void Increase()
    {
        coefficient = Mathf.Clamp(coefficient + 1, minCoefficient, maxCoefficient);
        UpdateCoefVisual();
        onCoefficientChanged?.Invoke();
    }

    public void Decrease()
    {
        coefficient = Mathf.Clamp(coefficient - 1, minCoefficient, maxCoefficient);
        UpdateCoefVisual();
        onCoefficientChanged?.Invoke();
    }
    public void Interact()
    {
        if (SelectManager.instance.currentmol == SelectManager.Molecula.Azul)
        {
            Increase();
        }

        else
        {
            Decrease();
        }
    }
    public Dictionary<string, int> GetTotalElementCounts()
    {
        Dictionary<string, int> totals = new Dictionary<string, int>();
        foreach (var ec in baseComposition)
        {
            if (totals.ContainsKey(ec.element)) totals[ec.element] += ec.count * coefficient;
            else totals[ec.element] = ec.count * coefficient;
        }
        return totals;
    }

    private void UpdateCoefVisual()
    {
        if (coefText == null) return;
        // Si coeficiente es 1 mostramos vacío para simular "no se ve el 1"
        coefText.text = (coefficient <= 1) ? "" : coefficient.ToString();
    }
}
