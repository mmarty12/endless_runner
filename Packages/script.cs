using Cinemachine;
using UnityEngine;

public class AddConfiner : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public Collider2D boundingShape;

    void Start()
    {
        // Add the Cinemachine Confiner component if it doesn't exist
        var confiner = virtualCamera.gameObject.AddComponent<CinemachineConfiner>();
        confiner.m_BoundingShape2D = boundingShape;
        confiner.InvalidatePathCache();
    }
}
