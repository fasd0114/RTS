using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MinimapIconGenerator : MonoBehaviour
{
    [Header("아이콘 외형")]
    public int textureSize = 64;          // 해상도
    public int circleRadius = 28;         // 반지름
    public Color circleColor = Color.white; // 색상

    void Awake()
    {
        Texture2D tex = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
        Color clear = new Color(0, 0, 0, 0);
        for (int y = 0; y < textureSize; y++)
            for (int x = 0; x < textureSize; x++)
                tex.SetPixel(x, y, clear);

        Vector2 center = new Vector2(textureSize / 2f, textureSize / 2f);
        for (int y = 0; y < textureSize; y++)
        {
            for (int x = 0; x < textureSize; x++)
            {
                float dx = x - center.x;
                float dy = y - center.y;
                if (dx * dx + dy * dy <= circleRadius * circleRadius)
                    tex.SetPixel(x, y, circleColor);
            }
        }

        tex.Apply();

        Sprite dotSprite = Sprite.Create(
            tex,
            new Rect(0, 0, textureSize, textureSize),
            new Vector2(0.5f, 0.5f),
            textureSize    
        );

        GetComponent<Image>().sprite = dotSprite;
    }
}
