using UnityEngine;
using UnityEngine.UI;

public class BuildUI : MonoBehaviour
{
    public static BuildUI Instance;

    public GameObject buildPanelObject;
    public Button buildCommandCenterButton;
    public Button buildBarracksButton;
    public GameObject commandCenterPrefab;
    public GameObject barracksPrefab;

    void Awake()
    {
        Instance = this;
        buildCommandCenterButton.onClick.AddListener(OnClickBuildCommandCenter);
        buildBarracksButton.onClick.AddListener(OnClickBuildBarracks);
        Hide();
    }

    public void Show() => buildPanelObject.SetActive(true);
    public void Hide() => buildPanelObject.SetActive(false);

    void OnClickBuildCommandCenter()
    {
        BuildManager.Instance.StartPlacingBuilding(commandCenterPrefab);
        Hide();
    }

    void OnClickBuildBarracks()
    {
        BuildManager.Instance.StartPlacingBuilding(barracksPrefab);
        Hide();
    }
}
