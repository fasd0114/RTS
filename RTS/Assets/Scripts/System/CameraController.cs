using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 30f;

    [Header("Zoom Settings (Orthographic)")]
    public float zoomSpeed = 200f;
    public float minZoomSize = 5f;
    public float maxZoomSize = 10f;

    [Header("Map Bounds (world XZ)")]
    public float minX = 0f;
    public float maxX = 200f;
    public float minZ = 0f;
    public float maxZ = 200f;

    [Header("Camera Tilt")]
    [Tooltip("X-axis rotation for 45° top-down view")]
    public float tiltAngle = 45f;

    [Header("Ground Plane Y")]
    public float groundY = 0f;

    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true;

        // 카메라 기울기(틸트) 설정
        Vector3 euler = transform.eulerAngles;
        euler.x = tiltAngle;
        transform.eulerAngles = euler;
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
        ClampToMap();
    }

    void HandleMovement()
    {
        Vector3 pos = transform.position;

        // WASD 키 또는 화면 모서리로 이동
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - 5)
            pos += Vector3.forward * moveSpeed * Time.deltaTime;
        if (Input.GetKey("s") || Input.mousePosition.y <= 5)
            pos -= Vector3.forward * moveSpeed * Time.deltaTime;
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - 5)
            pos += Vector3.right * moveSpeed * Time.deltaTime;
        if (Input.GetKey("a") || Input.mousePosition.x <= 5)
            pos -= Vector3.right * moveSpeed * Time.deltaTime;

        transform.position = pos;
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            cam.orthographicSize -= scroll * zoomSpeed * Time.deltaTime;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoomSize, maxZoomSize);
        }
    }

    /// <summary>
    /// 뷰포트의 네 모서리가 지면(Y = groundY)에 닿는 지점을 계산하여
    /// 맵 경계를 벗어나지 않도록 카메라 위치를 보정합니다.
    /// </summary>
    void ClampToMap()
    {
        Plane ground = new Plane(Vector3.up, new Vector3(0f, groundY, 0f));
        Vector3[] worldCorners = new Vector3[4];
        Vector2[] viewportPoints = new Vector2[]
        {
            new Vector2(0f, 0f),
            new Vector2(1f, 0f),
            new Vector2(0f, 1f),
            new Vector2(1f, 1f)
        };

        for (int i = 0; i < viewportPoints.Length; i++)
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(viewportPoints[i].x, viewportPoints[i].y, 0f));
            if (ground.Raycast(ray, out float enter))
                worldCorners[i] = ray.GetPoint(enter);
            else
                worldCorners[i] = transform.position;
        }

        float cminX = Mathf.Min(worldCorners[0].x, worldCorners[1].x, worldCorners[2].x, worldCorners[3].x);
        float cmaxX = Mathf.Max(worldCorners[0].x, worldCorners[1].x, worldCorners[2].x, worldCorners[3].x);
        float cminZ = Mathf.Min(worldCorners[0].z, worldCorners[1].z, worldCorners[2].z, worldCorners[3].z);
        float cmaxZ = Mathf.Max(worldCorners[0].z, worldCorners[1].z, worldCorners[2].z, worldCorners[3].z);

        Vector3 offset = Vector3.zero;
        if (cminX < minX) offset.x = minX - cminX;
        if (cmaxX > maxX) offset.x = maxX - cmaxX;
        if (cminZ < minZ) offset.z = minZ - cminZ;
        if (cmaxZ > maxZ) offset.z = maxZ - cmaxZ;

        transform.position += offset;
    }

    // 에디터에서 맵 경계와 카메라 뷰포트 모서리 표시용
    void OnDrawGizmosSelected()
    {
        // 맵 경계
        Gizmos.color = Color.green;
        Vector3 bl = new Vector3(minX, groundY, minZ);
        Vector3 br = new Vector3(maxX, groundY, minZ);
        Vector3 tl = new Vector3(minX, groundY, maxZ);
        Vector3 tr = new Vector3(maxX, groundY, maxZ);
        Gizmos.DrawLine(bl, br);
        Gizmos.DrawLine(br, tr);
        Gizmos.DrawLine(tr, tl);
        Gizmos.DrawLine(tl, bl);

        // 뷰포트 코너(지면 교차점)
        Camera c = cam != null ? cam : GetComponent<Camera>();
        if (c == null) return;

        Plane ground = new Plane(Vector3.up, new Vector3(0f, groundY, 0f));
        Gizmos.color = Color.red;
        Vector2[] viewportPoints = new Vector2[]
        {
            new Vector2(0f, 0f),
            new Vector2(1f, 0f),
            new Vector2(0f, 1f),
            new Vector2(1f, 1f)
        };

        foreach (var vp in viewportPoints)
        {
            Ray ray = c.ViewportPointToRay(new Vector3(vp.x, vp.y, 0f));
            if (ground.Raycast(ray, out float enter))
                Gizmos.DrawSphere(ray.GetPoint(enter), 0.3f);
        }
    }
}
