// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Authors: VinTK
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Collectible : MonoBehaviour
{
    [Header("Object")]
    [SerializeField]
    private CollectibleEffect m_effect = null;

    public UnityEvent onCollect = new UnityEvent();

    private Collider2D m_collider2D;
    private Renderer[] m_renderers;
    private AudioSource m_audioSource;


    private void Awake()
    {
        m_collider2D = GetComponent<Collider2D>();
        m_renderers = GetComponentsInChildren<Renderer>();
        m_audioSource = GetComponent<AudioSource>();

        if (m_effect != null)
        {
            m_effect.Init(this);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject hitObject = collision.gameObject;

        // Only objects with the player tag can pick up the collectible
        if (!hitObject.CompareTag("Player"))
            return;

        // Apply the effect to the object it has collided with
        if (m_effect != null)
            m_effect.Apply(hitObject);

        // Disable the collided once we collected the thing
        m_collider2D.enabled = false;

        // Play the sound
        m_audioSource.Play();

        // Don't show the thing
        for (int i = 0; i < m_renderers.Length; i++)
        {
            m_renderers[i].enabled = false;
        }

        StartCoroutine(Co_WaitUntilSoundFinish(() =>
        {
            Destroy(gameObject);
            if (onCollect != null)
                onCollect.Invoke();
        }));
    }


    public CollectibleEffect GetEffect()
    {
        return m_effect;
    }


    private IEnumerator Co_WaitUntilSoundFinish(Action action)
    {
        yield return new WaitUntil(() => m_audioSource.isPlaying == false);
        if (action != null)
            action();
    }
}
