using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    public Transform target;            // arrastra aquí el Player
    public float smoothTime = 0.12f;    // suavidad del seguimiento

    // Límites del mapa EN MUNDO (para no salirte)
    public Vector2 minBounds = new Vector2(0.5f, 0.5f);
    public Vector2 maxBounds = new Vector2(19.5f, 19.5f); // para 20x20 con celdas centradas a .5

    private Vector3 velocity = Vector3.zero;
    private Camera cam;

    void Awake() => cam = GetComponent<Camera>();

    void LateUpdate()
{
    if (target == null) return;

    Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);
    Vector3 newPos = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

    float halfHeight = cam.orthographicSize;
    float halfWidth  = halfHeight * cam.aspect;

    float spanX = maxBounds.x - minBounds.x; // ancho útil (19.5 - 0.5 = 19)
    float spanY = maxBounds.y - minBounds.y; // alto útil

    // Solo clampea si la cámara "cabe" en ese eje
    if (halfWidth * 2f < spanX)
        newPos.x = Mathf.Clamp(newPos.x, minBounds.x + halfWidth,  maxBounds.x - halfWidth);

    if (halfHeight * 2f < spanY)
        newPos.y = Mathf.Clamp(newPos.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);

    transform.position = newPos;
}
}
