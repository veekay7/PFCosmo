// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using UnityEngine;

public class AlertBoxController : MonoBehaviourSingleton<AlertBoxController>
{
    [SerializeField]
    private AlertBoxBasic m_basicAlertBox = null;
    [SerializeField]
    private AlertBoxConfirm m_confirmAlertBox = null;

    public AlertBoxBasic basicAlertBox => m_basicAlertBox;
    public AlertBoxConfirm confirmAlertBox => m_confirmAlertBox;

    public static bool alertIsShown { get; set; }


    protected override void OnEnable()
    {
        base.OnEnable();
    }


    private void Start()
    {
        HideAll();
    }


    [ContextMenu("Hide All")]
    public void HideAll()
    {
        Debug.Assert(m_basicAlertBox != null && m_confirmAlertBox != null);
        m_basicAlertBox.Hide();
        m_confirmAlertBox.Hide();
    }
}
