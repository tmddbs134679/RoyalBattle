using Quantum;
using UnityEngine;

public class BillboardCamera : QuantumViewComponent<CameraViewContext>
{
    [SerializeField] private Vector3 worldOffset = new Vector3(0, 0.5f, 0); // Y축으로 0.5 올림

    void LateUpdate()
    {
        if (ViewContext?.virtualCamera == null) return;

        // 부모 위치 + 월드 오프셋
        if (transform.parent != null)
        {
            transform.position = transform.parent.position + worldOffset;
        }

        // 회전은 고정
        transform.rotation = Quaternion.Euler(-45f, -180f, 0f);
    }
}