using Unity.Mathematics;
using UnityEngine;

public class playerYürüme : MonoBehaviour
{
  
    
    void Update()
    {
        if (Input.GetKey(KeyCode.A)) { transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, -90, 0), .05f); }
        if (Input.GetKey(KeyCode.D)) { transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 90, 0), .05f); }
        if (Input.GetKey(KeyCode.W)) { transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 360, 0), .05f); }
        if (Input.GetKey(KeyCode.S)) { transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, -180, 0), .05f); }

        float MoveX = Input.GetAxis("Horizontal");
        float MoveZ = Input.GetAxis("Vertical");
        transform.position = transform.position + new Vector3(MoveX * 10f * Time.deltaTime, 0, MoveZ * 10f * Time.deltaTime);
    }
}
