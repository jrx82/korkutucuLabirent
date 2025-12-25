using UnityEngine;

public class kilicScript : MonoBehaviour
{
    public int damage = 20;       // Düþmana kaç hasar verecek?
    public AudioClip vurusSesi;   // Vurunca çýkacak ses (Çat!)
    private AudioSource source;

    void Start()
    {
        // Kýlýçta AudioSource var mý bak, yoksa ekle
        source = GetComponent<AudioSource>();
        if (source == null)
            source = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Sadece "Enemy" etiketli objelere vur
        if (other.CompareTag("Enemy"))
        {
            // Önce direkt çarptýðýmýz objede script var mý bak
            dusmanAI enemy = other.GetComponent<dusmanAI>();

            // Bulamazsan parent (üst) objesine bak
            if (enemy == null)
                enemy = other.GetComponentInParent<dusmanAI>();

            // Düþmaný bulduysak hasar ver
            if (enemy != null)
            {
                enemy.TakeDamage(damage);

                // --- SESÝ ÇAL ---
                if (vurusSesi != null && source != null)
                {
                    source.PlayOneShot(vurusSesi);
                }
            }
        }
    }
}