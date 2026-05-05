using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MineralUI : MonoBehaviour
{
    public TextMeshProUGUI mineralText;

    private void Start()
    {
        UpdateMineralText();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnMineralChanged += UpdateMineralText;
        }
    }
    void OnDestroy()
    {
        // 오브젝트 파괴 시 이벤트 구독 해제 (메모리 누수 방지)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnMineralChanged -= UpdateMineralText;
        }
    }

    void UpdateMineralText()
    {
        mineralText.text = "Minerals: " + GameManager.Instance.minerals.ToString();
    }
}
