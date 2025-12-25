using UnityEngine;
using UnityEngine.AI;

public class dusmanAI : MonoBehaviour
{
    [Header("Hareket ve Saldýrý Ayarlarý")]
    public Transform player;           // Hedef (Oyuncu)
    public float detectionRange = 15f; // Görme mesafesi
    public float attackRange = 2.5f;   // Saldýrý mesafesi
    public float attackCooldown = 2f;  // Kaç saniyede bir vursun?

    [Header("Can Ayarlarý")]
    public int maxHealth = 100;        // Düþmanýn toplam caný
    private int currentHealth;

    [Header("Ses Ayarlarý")]
    public AudioClip saldiriSesi;      // Saldýrýnca çýkacak ses (Hýýaa!)
    private AudioSource audioSource;

    private NavMeshAgent agent;
    private Animator anim;
    private float lastAttackTime = 0;
    private bool isDead = false;       // Ölü mü kontrolü

    void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();

        // Animator bazen child objede olabiliyor, garantiye alalým
        anim = GetComponentInChildren<Animator>();

        // AudioSource bileþenini al, yoksa ekle
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // Player'ý otomatik bul (Eðer elle atanmadýysa)
        if (player == null && GameObject.FindGameObjectWithTag("Player") != null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (isDead) return;       // Öldüyse iþlem yapma
        if (player == null) return; // Player yoksa hata verme

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (distanceToPlayer > attackRange)
            {
                ChasePlayer();
            }
            else
            {
                AttackPlayer();
            }
        }
        else
        {
            Idle();
        }
    }

    // --- HAREKET FONKSÝYONLARI ---
    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
        anim.SetBool("isChasing", true);
    }

    void AttackPlayer()
    {
        agent.isStopped = true; // Vururken dur
        anim.SetBool("isChasing", false);

        // Yüzünü oyuncuya dön (Sadece Y ekseninde)
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(targetPosition);

        // Saldýrý zamaný geldiyse
        if (Time.time > lastAttackTime + attackCooldown)
        {
            anim.SetTrigger("attack");

            // --- SESÝ ÇAL ---
            if (saldiriSesi != null)
            {
                audioSource.PlayOneShot(saldiriSesi);
            }

            lastAttackTime = Time.time;
        }
    }

    void Idle()
    {
        agent.isStopped = true;
        anim.SetBool("isChasing", false);
    }

    // --- HASAR ALMA VE ÖLME ---
    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        // Debug.Log("Düþman Caný: " + currentHealth); // Test için açabilirsin

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        agent.isStopped = true;
        agent.enabled = false; // NavMesh'i kapat

        // Vurulmayý engellemek için Collider'ý kapat
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        anim.SetTrigger("die");  // Ölüm animasyonu
        Destroy(gameObject, 5f); // 5 saniye sonra cesedi sil
    }

    // Editörde menzilleri görmek için çizgiler
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}