using UnityEngine;

public class MobAI : MonoBehaviour
{
    public float patrolRange = 5f; // Dist�ncia m�xima para patrulhar
    public float detectionRange = 10f; // Dist�ncia para detectar o jogador
    public float stopChaseRange = 15f; // Dist�ncia para parar de perseguir
    public float moveSpeed = 2f; // Velocidade de movimento do mob
    public float patrolWaitTime = 2f; // Tempo de espera entre os movimentos de patrulha
   
    public  Vector2 patrolOrigin; // Ponto de origem da patrulha
    public  Vector2 patrolTarget; // Pr�ximo ponto de patrulha
    public  Transform targetPlayer; // Refer�ncia ao jogador mais pr�ximo
    public  float patrolTimer; // Temporizador para a patrulha
    public  bool isChasing = false; // Se est� perseguindo o jogador
    public float attackRange = 1f;
    void Start()
    {
        patrolOrigin = transform.position;
        SetNewPatrolTarget();
    }

    void Update()
    {
        // Detecta o jogador mais pr�ximo dentro do alcance de detec��o
        DetectClosestPlayer();

        if (targetPlayer != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, targetPlayer.position);

            if (isChasing)
            {
                if (distanceToPlayer > stopChaseRange)
                {
                    // Para de perseguir e volta para a patrulha
                    isChasing = false;
                    SetNewPatrolTarget();
                }
                else
                {
                    // Continua perseguindo o jogador
                    ChasePlayer();
                }
            }
            else
            {
                if (distanceToPlayer <= detectionRange)
                {
                    // Come�a a perseguir o jogador
                    isChasing = true;
                }
                else
                {
                    // Continua patrulhando
                    Patrol();
                }
            }
        }
        else
        {
            // Continua patrulhando se nenhum jogador estiver pr�ximo
            Patrol();
        }
    }

    void Patrol()
    {
        if (Vector2.Distance(transform.position, patrolTarget) < 0.1f)
        {
            // Alcan�ou o alvo de patrulha, espera antes de definir um novo
            patrolTimer += Time.deltaTime;
            if (patrolTimer >= patrolWaitTime)
            {
                SetNewPatrolTarget();
                patrolTimer = 0f;
            }
        }
        else
        {
            // Move-se em dire��o ao alvo de patrulha
            MoveTowards(patrolTarget);
        }
    }

    void SetNewPatrolTarget()
    {
        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomY = Random.Range(-patrolRange, patrolRange);
        patrolTarget = patrolOrigin + new Vector2(randomX, randomY);
    }

    void ChasePlayer()
    {
        MoveTowards(targetPlayer.position);
    }

    void MoveTowards(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
        float distanceToPlayer = Vector2.Distance(transform.position, target);


        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Aplica a rota��o no eixo Y para virar o bot
        if (angle > 90 || angle < -90)
        {
            // Rotaciona o bot 180 graus no eixo Y
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            // Mant�m a rota��o original
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }



        if (distanceToPlayer <= attackRange && isChasing)
        {
            // Aqui voc� pode chamar a fun��o de ataque ou outro comportamento
            TriggerAttack();
        }
    }

    void TriggerAttack()
    {
        // L�gica para iniciar o ataque do bot
        //Debug.Log("Bot est� atacando!");

        GetComponent<CombatController>().SetPerformAttack();

        // Adicione a l�gica de ataque aqui
    }

    void DetectClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float closestDistance = detectionRange;
        targetPlayer = null;

        foreach (GameObject player in players)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                targetPlayer = player.transform;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Verde: Limite da patrulha (movimento aleat�rio).
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, patrolRange);

        // Amarelo: Limite de detec��o (inicia persegui��o).
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Vermelho: Limite de persegui��o (interrompe persegui��o).
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopChaseRange);
    }

}
