using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatsBarUI : MonoBehaviour
{
    UnitStats m_unitStats;

    [Header("Stats Images")]
    [SerializeField]
    Image HP;
    [SerializeField]
    Image SP;
    [SerializeField]
    Image SPD;
    [Header("Canvas Settings")]
    [SerializeField]
    Canvas m_canvas;
    [Header("Animation Settings")]
    [SerializeField]
    float m_baseDistance = 10f;
    [SerializeField]
    float m_scaleFactor = 1f;
    [Header("Pool Settings")]
    [SerializeField]
    float m_fillAnimationSpeed = 2f;

    Camera m_camera;
    Vector3 m_originalScale;
    float m_basePool = 100f;
    bool m_toggle = false;
    bool m_isDead = false;

    private float m_currentSPFill { get; set; } = 0f;
    private float m_currentSPDFill { get; set; } = 0f;

    public event Action OnSP;
    public event Action OnSPD;
    public event Action OnHP;


    void Awake() => m_unitStats = GetComponent<UnitStats>();
    private void Start()
    {
        m_camera = Camera.main;
        m_originalScale = m_canvas.transform.localScale;

        SP.fillAmount = m_basePool;
        SPD.fillAmount = m_basePool;
    }
    private void OnDestroy()
    {
        OnSP = null;
        OnSPD = null;
        OnHP = null;
    }

    private void Update()
    {
        BarAnimation();
        if (m_toggle)
            UpdateFillAnimations();
    }

    [Button("Toggle")]
    public void Toggle()
    {
        if (m_toggle) 
        {
            SP.fillAmount = 1f;
            SPD.fillAmount = 1f;
            m_currentSPFill = m_basePool;
            m_currentSPDFill = m_basePool;
        }
        else 
        {
            SP.fillAmount = 0f;
            SPD.fillAmount = 0f;
            m_currentSPFill = 0f;
            m_currentSPDFill = 0f;
        }
        m_toggle = !m_toggle;
    }

    void BarAnimation()
    {
        Vector3 direction = m_camera.transform.position - transform.position;
        Vector3 verticalDirection = new Vector3(0, direction.y, direction.z);
        if (verticalDirection != Vector3.zero)
            m_canvas.transform.rotation = Quaternion.LookRotation(verticalDirection);

        float distance = direction.magnitude;
        float scale = (distance / m_baseDistance) * m_scaleFactor;
        m_canvas.transform.localScale = m_originalScale * scale;
    }
    void UpdateFillAnimations()
    {
        if (m_unitStats.CurrentHP <= 0 && !m_isDead)
        {
            OnHP?.Invoke();
            m_isDead = true;
        }
        if (m_isDead) return;

        m_currentSPFill += m_unitStats.SP * Time.deltaTime;
        m_currentSPDFill += m_unitStats.SPD * Time.deltaTime;
        m_currentSPFill = Mathf.Clamp(m_currentSPFill, 0f, m_basePool);
        m_currentSPDFill = Mathf.Clamp(m_currentSPDFill, 0f, m_basePool);

        float targetSPFill = m_currentSPFill / m_basePool;
        float targetSPDFill = m_currentSPDFill / m_basePool;
        float hpFillAmount = m_unitStats.CurrentHP / m_unitStats.HP;

        if (SP.fillAmount >= 0.99f && m_currentSPFill >= m_basePool)
        {
            m_currentSPFill = 0f;
            SP.fillAmount = 0f; 
            OnSP?.Invoke();
        }
        else
            SP.fillAmount = Mathf.Lerp(SP.fillAmount, targetSPFill, m_fillAnimationSpeed * Time.deltaTime);

        if (SPD.fillAmount >= 0.99f && m_currentSPDFill >= m_basePool)
        {
            m_currentSPDFill = 0f;
            SPD.fillAmount = 0f; 
            OnSPD?.Invoke();
        }
        else
            SPD.fillAmount = Mathf.Lerp(SPD.fillAmount, targetSPDFill, m_fillAnimationSpeed * Time.deltaTime);

        hpFillAmount = Mathf.Clamp01(hpFillAmount);
        HP.fillAmount = hpFillAmount;
    }
}