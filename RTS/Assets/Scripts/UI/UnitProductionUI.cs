using UnityEngine;
using UnityEngine.UI;

public class UnitProductionUI : MonoBehaviour
{
    public static UnitProductionUI Instance;

    [Header("생산 패널")]
    public GameObject commandCenterPanel;
    public GameObject barracksPanel;

    private CommandCenter currentCC;
    private Barracks currentBarracks;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            HideAll();
    }

    public void ShowCommandCenter(CommandCenter cc)
    {
        currentCC = cc;
        commandCenterPanel.SetActive(true);
        barracksPanel.SetActive(false);
    }

    public void ShowBarracks(Barracks barracks)
    {
        currentBarracks = barracks;
        barracksPanel.SetActive(true);
        commandCenterPanel.SetActive(false);
    }
    public void ProduceSCV()
    {
        currentCC?.ProduceSCV();
    }
    public void ProduceMarine()
    {
        currentBarracks?.ProduceMarine();
    }
    public void HideAll()
    {
        commandCenterPanel.SetActive(false);
        barracksPanel.SetActive(false);
    }
}
