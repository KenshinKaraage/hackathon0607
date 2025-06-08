using UnityEngine;

public class ParticleClickSpawner : MonoBehaviour
{
    public GameObject effectPrefab;  // Particle System のプレハブ
    public Camera mainCamera;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = mainCamera.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Instantiate(effectPrefab, hit.point, Quaternion.identity);
            }
            else
            {
                // 地面などヒットしない場合、カメラ前方に出す（例）
                Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 5f));
                Instantiate(effectPrefab, worldPos, Quaternion.identity);
            }
        }
    }
}
