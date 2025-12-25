using UnityEngine;

public class playerYurume : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float speed = 5f;        // Yürüme hızı
    public float donusHizi = 10f;   // Karakterin dönme yumuşaklığı

    private Rigidbody rb;
    private Transform cam;          // Kameranın pozisyonunu alacağız

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Sahnedeki "Main Camera"yı otomatik bul
        cam = Camera.main.transform;
    }

    void FixedUpdate()
    {
        // 1. Klavyeden Girişleri Al
        float moveX = Input.GetAxis("Horizontal"); // A - D
        float moveZ = Input.GetAxis("Vertical");   // W - S

        // 2. Hareket Vektörünü Hesapla (Kameraya Göre!)
        // Kameranın ileri (forward) ve sağ (right) vektörlerini alıyoruz
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;

        // Kameranın yere bakış açısını sıfırlıyoruz ki karakter yere çakılmasın
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // Yönü oluşturuyoruz: (İleri Tuşu * Kamera İlerisi) + (Yan Tuş * Kamera Yanı)
        Vector3 movement = (camForward * moveZ + camRight * moveX).normalized;

        // 3. Hareket Varsa İşlem Yap
        if (movement.magnitude > 0.1f)
        {
            // A) Karakterin Yönünü O Tarafa Çevir (Yumuşakça)
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, donusHizi * Time.deltaTime);

            // B) Karakteri O Yöne İt (Rigidbody ile)
            // Y eksenindeki hızı (rb.velocity.y) koruyoruz ki yerçekimi bozulmasın
            rb.linearVelocity = new Vector3(movement.x * speed, rb.linearVelocity.y, movement.z * speed);
        }
        else
        {
            // Tuşa basmıyorsak kaymayı önlemek için hızı (Y hariç) sıfırla
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
    }
}