using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToolTipView : MonoBehaviour
{
    // Start is called before the first frame update
    public static ToolTipView Instance;

    [SerializeField] private GameObject tooltipPanel;
    [SerializeField] private TextMeshProUGUI titleText; // Sửa thành TextMeshProUGUI
    [SerializeField] private TextMeshProUGUI descriptionText; // Sửa thành TextMeshProUG

    private Coroutine hideTooltipRoutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowTooltip(string title, string description, Vector3 position)
    {
        if (hideTooltipRoutine != null)
        {
            StopCoroutine(hideTooltipRoutine);
            hideTooltipRoutine = null;
        }

        titleText.text = title;
        descriptionText.text = description;
        tooltipPanel.SetActive(true);
        tooltipPanel.transform.position = position;
    }

    public void HideTooltip()
    {
        if (hideTooltipRoutine != null)
        {
            StopCoroutine(hideTooltipRoutine);
        }
        hideTooltipRoutine = StartCoroutine(HideTooltipAfterDelay());
    }

    private IEnumerator HideTooltipAfterDelay()
    {
        yield return new WaitForSeconds(0.5f); // Điều chỉnh thời gian này theo nhu cầu
        tooltipPanel.SetActive(false);
    }
}

