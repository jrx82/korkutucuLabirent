using UnityEngine;

public class GTAKamera : MonoBehaviour
{
    [Header("Takip Ayarlarý")]
    public Transform karakter;
    public Vector3 hedefOffset = new Vector3(0, 1.5f, 0); // Karakterin kafasý hizasý
    public float mesafe = 5.0f;
    public float kameraYariCapi = 0.2f; // Kameranýn kalýnlýðý (Duvara girmemesi için)

    [Header("Mouse ve Açý Ayarlarý")]
    public float fareHassasiyeti = 3.0f;
    public float minYukariBakma = -40f; // Aþaðý bakma sýnýrý
    public float maxYukariBakma = 80f;  // Yukarý bakma sýnýrý

    [Header("Duvar Ayarlarý")]
    public LayerMask duvarKatmani; // Kamera neleri duvar olarak görsün?

    private float rotasyonX = 0f; // Sað/Sol açýsý
    private float rotasyonY = 0f; // Yukarý/Aþaðý açýsý

    void Start()
    {
        // Mouse imlecini gizle
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Baþlangýç açýlarýný mevcut duruma göre ayarla
        Vector3 angles = transform.eulerAngles;
        rotasyonX = angles.y;
        rotasyonY = angles.x;
    }

    void LateUpdate()
    {
        if (karakter == null) return;

        // 1. Mouse Verilerini Al (Hem X hem Y)
        rotasyonX += Input.GetAxis("Mouse X") * fareHassasiyeti;
        rotasyonY -= Input.GetAxis("Mouse Y") * fareHassasiyeti;

        // 2. Yukarý/Aþaðý bakmayý sýnýrla (Takla atmasýn)
        rotasyonY = Mathf.Clamp(rotasyonY, minYukariBakma, maxYukariBakma);

        // 3. Rotasyonu Oluþtur (Hem X hem Y kullanarak)
        Quaternion rotasyon = Quaternion.Euler(rotasyonY, rotasyonX, 0);

        // 4. Ýdeal Pozisyonu Hesapla (Duvar olmasaydý nerede duracaktý?)
        Vector3 karakterKafasi = karakter.position + hedefOffset;
        Vector3 idealPozisyon = karakterKafasi - (rotasyon * Vector3.forward * mesafe);

        // 5. DUVAR KONTROLÜ (SphereCast - Kalýn Iþýn)
        RaycastHit hit;
        Vector3 finalPozisyon = idealPozisyon;

        // Karakterden kameraya doðru, kameranýn gideceði yöne ýþýn atýyoruz
        Vector3 yon = idealPozisyon - karakterKafasi;

        // Eðer arada "duvarKatmani" varsa:
        if (Physics.SphereCast(karakterKafasi, kameraYariCapi, yon.normalized, out hit, mesafe, duvarKatmani))
        {
            // Kamerayý çarptýðý yerin biraz önüne koy
            // (hit.normal * 0.1f) demek, duvardan 10cm öne çekmek demektir.
            finalPozisyon = hit.point + (hit.normal * 0.1f);
        }

        // 6. Uygula
        transform.rotation = rotasyon;
        transform.position = finalPozisyon;
    }
}