using UnityEngine;
using System.Collections;

public class PanelZoneManager : MonoBehaviour
{
    public static PanelZoneManager Instance;

    [Header("Zone Panels")]
    public GameObject desertZonePanel;
    public GameObject graveyardZonePanel;
    public GameObject villageZonePanel;

    [Header("Settings")]
    public float displayTime = 1.5f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowDesertZone()
    {
        StartCoroutine(ShowPanel(desertZonePanel));
    }

    public void ShowGraveyardZone()
    {
        StartCoroutine(ShowPanel(graveyardZonePanel));
    }

    public void ShowVillageZone()
    {
        StartCoroutine(ShowPanel(villageZonePanel));
    }

    private IEnumerator ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
        yield return new WaitForSeconds(displayTime);
        panel.SetActive(false);
    }
}
