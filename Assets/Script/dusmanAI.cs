using UnityEngine;
using UnityEngine.AI;

public class dusmanAI : MonoBehaviour
{
    [Header("Ayarlar")]
    public Transform player;           // Hedef (Oyuncu)
    public float detectionRange = 15f; // Görme mesafesi
    public float attackRange = 2.5f;   // Saldýrý mesafesi
    public float attackCooldown = 2f;  // Saldýrý hýzý

    [Header("Can Ayarlarý")]
    public int maxHealth = 100;
    private int currentHealth;

    private NavMeshAgent agent;
    private Animator anim;
    private float lastAttackTime = 0;
    private bool isDead = false; // Ölü mü?

    void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();

        // Animator'ý hem kendinde hem de çocuk objelerde arar (Garanti olsun)
        anim = GetComponentInChildren<Animator>();

        if (player == null && GameObject.FindGameObjectWithTag("Player") != null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Eðer öldüyse hiçbir þey yapma
        if (isDead) return;

        // Eðer player bulunamadýysa hata vermemesi için koruma
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (distanceToPlayer > attackRange)
            {
                ChasePlayer(); // Bu fonksiyonu aþaðýda tanýmladýk
            }
            else
            {
                AttackPlayer(); // Bu fonksiyonu aþaðýda tanýmladýk
            }
        }
        else
        {
            Idle(); // Bu fonksiyonu aþaðýda tanýmladýk
        }
    }

    // --- HASAR ALMA VE ÖLME ---
    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        Debug.Log("Düþman hasar aldý! Kalan Can: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        agent.isStopped = true;       // Hareketi durdur
        agent.enabled = false;        // NavMesh'i kapat

        // Vurulmayý engellemek için Collider'ý kapat
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        anim.SetTrigger("die");       // Ölüm animasyonunu çal
        Destroy(gameObject, 5f);      // 5 saniye sonra yok et
    }

    // --- HAREKET FONKSÝYONLARI (EKSÝK OLANLAR BUNLARDI) ---

    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
        anim.SetBool("isChasing", true);
    }

    void AttackPlayer()
    {
        agent.isStopped = true;
        anim.SetBool("isChasing", false);

        // -- Yüzünü Dönme Kýsmý (Sadece Y ekseninde) --
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(targetPosition);

        // Saldýrý zamanlamasý
        if (Time.time > lastAttackTime + attackCooldown)
        {
            anim.SetTrigger("attack");
            lastAttackTime = Time.time;
        }
    }

    void Idle()
    {
        agent.isStopped = true;
        anim.SetBool("isChasing", false);
    }

    // Editörde çizgileri görmek için
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}