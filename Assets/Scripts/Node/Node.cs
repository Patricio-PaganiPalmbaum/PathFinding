using System;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    public bool IsLocked
    {
        get
        {
            return isLocked;
        }
        set
        {
            isLocked = value;
            textCost.gameObject.SetActive(!value);
            SetColor(isLocked ? blockedColor : defaultColor);
        }
    }

    public float Cost
    {
        get
        {
            return cost;
        }
        set
        {
            cost = value;
            textCost.text = cost.ToString();
        }
    }

    public Action<Node> OnSelectCell = delegate { };

    public Color defaultColor;
    public Color blockedColor;

    public Vector2Int gridPosition;
    public Text textCost;

    private Material material;

    private float cost;
    private bool isLocked;

    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
        Reset();
    }

    public void SetColor(Color color)
    {
        material.color = color;
    }

    public void ResetColor()
    {
        SetColor(defaultColor);
    }

    private void Reset()
    {
        IsLocked = false;
        Cost = 1;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            IsLocked = !IsLocked;
        }

        if (Input.GetMouseButtonDown(0))
        {
            OnSelectCell.Invoke(this);
        }

        Cost = Mathf.Clamp(Cost + (int)Input.mouseScrollDelta.y, 0, 100);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            textCost.enabled = !textCost.enabled;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }
    }
}
