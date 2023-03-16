using System.Collections.Generic;
using UnityEngine;

public class MdlCosmo : MonoBehaviour
{
    private Dictionary<string, SpriteRenderer> m_parts = new Dictionary<string, SpriteRenderer>();
    public CosmoAnimBehaviour animBehaviour { get; private set; }

    private void Awake()
    {
        SpriteRenderer[] parts = transform.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer p in parts)
        {
            m_parts.Add(p.gameObject.name, p);
        }
    }


    private void OnEnable()
    {
        animBehaviour = GetComponent<CosmoAnimBehaviour>();
    }


    public void FlipHorz(bool flip)
    {
        Vector3 scale = transform.localScale;
        scale.x = flip ? scale.x * -1.0f : Mathf.Abs(scale.x);
        transform.localScale = scale;
    }


    public void FlipVert(bool flip)
    {
        Vector3 scale = transform.localScale;
        scale.y = flip ? scale.y * -1.0f : Mathf.Abs(scale.y);
        transform.localScale = scale;
    }


    public void ResetFlip()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        scale.y = Mathf.Abs(scale.y);
        transform.localScale = scale;
    }


    public SpriteRenderer GetPart(string partName)
    {
        return m_parts[partName];
    }


    private void Reset()
    {
        ResetFlip();
    }
}
