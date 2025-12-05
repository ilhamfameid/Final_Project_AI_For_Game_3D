using UnityEngine;

public class FollowGround : MonoBehaviour {
    public float raycastDistance = 1.0f; // Jarak raycast ke bawah
    public LayerMask groundLayer; // Layer tanah (opsional, bisa pakai default)

    private Transform parentTransform;
    private Vector3 offset; // Offset awal dari parent

    void Start() {
        parentTransform = transform.parent;
        if (parentTransform == null) {
            Debug.LogError("FollowGround: Parent not found!");
            return;
        }

        // Simpan offset awal (local position)
        offset = transform.localPosition;
    }

    void LateUpdate() {
        if (parentTransform == null) return;

        // Dapatkan posisi global parent
        Vector3 parentPosition = parentTransform.position;

        // Raycast ke bawah dari posisi parent + offset XZ
        Vector3 rayOrigin = new Vector3(
            parentPosition.x + offset.x,
            parentPosition.y + offset.y + raycastDistance,
            parentPosition.z + offset.z
        );

        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, raycastDistance + 0.1f, groundLayer)) {
            // Atur posisi Y agar menyentuh tanah
            float newY = hit.point.y + offset.y;
            transform.position = new Vector3(
                parentPosition.x + offset.x,
                newY,
                parentPosition.z + offset.z
            );
        } else {
            // Jika tidak ketemu tanah, tetap ikuti parent (bisa diganti dengan jatuh bebas)
            transform.position = parentTransform.position + offset;
        }
    }
}