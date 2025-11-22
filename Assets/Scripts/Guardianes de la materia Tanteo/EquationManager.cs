using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class EquationManager : MonoBehaviour
{
    [Header("Lista de moléculas (arrastrar desde la escena)")]
    public List<Molecule3D> reactants = new List<Molecule3D>();
    public List<Molecule3D> products = new List<Molecule3D>();

    [Header("UI worldspace TextMeshPro para mostrar totales")]
    public TMP_Text leftTotalsText;   // total de elementos en reactivos
    public TMP_Text rightTotalsText;  // total de elementos en productos

    [Header("Eventos")]
    public UnityEvent onBalanced;     // cuando la ecuación queda balanceada
    public UnityEvent onUnbalanced;

    private bool lastBalancedState = false;

    private void OnEnable()
    {
        foreach (var m in reactants) m.onCoefficientChanged.AddListener(RecalculateAndUpdateUI);
        foreach (var m in products) m.onCoefficientChanged.AddListener(RecalculateAndUpdateUI);

        RecalculateAndUpdateUI();
    }

    private void OnDisable()
    {
        foreach (var m in reactants) m.onCoefficientChanged.RemoveListener(RecalculateAndUpdateUI);
        foreach (var m in products) m.onCoefficientChanged.RemoveListener(RecalculateAndUpdateUI);
    }

    public void RecalculateAndUpdateUI()
    {
        var leftTotals = SumTotals(reactants);
        var rightTotals = SumTotals(products);

        UpdateUIText(leftTotalsText, leftTotals);
        UpdateUIText(rightTotalsText, rightTotals);

        bool balanced = AreDictionariesEqual(leftTotals, rightTotals);
        if (balanced != lastBalancedState)
        {
            lastBalancedState = balanced;
            if (balanced) onBalanced?.Invoke();
            else onUnbalanced?.Invoke();
        }
    }

    private Dictionary<string, int> SumTotals(List<Molecule3D> list)
    {
        Dictionary<string, int> acc = new Dictionary<string, int>();
        foreach (var mol in list)
        {
            var t = mol.GetTotalElementCounts();
            foreach (var kv in t)
            {
                if (acc.ContainsKey(kv.Key)) acc[kv.Key] += kv.Value;
                else acc[kv.Key] = kv.Value;
            }
        }
        return acc;
    }

    private void UpdateUIText(TMP_Text text, Dictionary<string, int> totals)
    {
        if (text == null) return;
        // Orden alfabético para consistencia
        var keys = totals.Keys.OrderBy(k => k).ToList();
        if (keys.Count == 0) { text.text = ""; return; }

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine("Elemento : Total");
        foreach (var k in keys)
        {
            sb.AppendLine($"{k} : {totals[k]}");
        }
        text.text = sb.ToString();
    }

    private bool AreDictionariesEqual(Dictionary<string, int> a, Dictionary<string, int> b)
    {
        if (a.Count != b.Count) return false;
        foreach (var kv in a)
        {
            if (!b.ContainsKey(kv.Key)) return false;
            if (b[kv.Key] != kv.Value) return false;
        }
        return true;
    }

}
