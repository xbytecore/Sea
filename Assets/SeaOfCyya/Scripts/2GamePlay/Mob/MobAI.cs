using UnityEngine;

public class MobAI : MonoBehaviour
{
    public float patrolRange = 5f; // Distância máxima para patrulhar
    public float detectionRange = 10f; // Distância para detectar o jogador
    public float stopChaseRange = 15f; // Distância para parar de perseguir
    public float moveSpeed = 2f; // Velocidade de movimento do mob
    public float patrolWaitTime = 2f; // Tempo de espera entre os movimentos de patrulha
   
    public  Vector2 patrolOrigin; // Ponto de origem da patrulha
    public  Vector2 patrolTarget; // Próximo ponto de patrulha
    public  Transform targetPlayer; // Referência ao jogador mais próximo
    public  float patrolTimer; // Temporizador para a patrulha
    public  bool isChasing = false; // Se está perseguindo o jogador
    public float attackRange = 1f;
    void Start()
    {
        patrolOrigin = transform.position;
        SetNewPatrolTarget();
    }

    void Update()
    {
        // Detecta o jogador mais próximo dentro do alcance de detecção
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
                    // Começa a perseguir o jogador
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
            // Continua patrulhando se nenhum jogador estiver próximo
            Patrol();
        }
    }

    void Patrol()
    {
        if (Vector2.Distance(transform.position, patrolTarget) < 0.1f)
        {
            // Alcançou o alvo de patrulha, espera antes de definir um novo
            patrolTimer += Time.deltaTime;
            if (patrolTimer >= patrolWaitTime)
            {
                SetNewPatrolTarget();
                patrolTimer = 0f;
            }
        }
        else
        {
            // Move-se em direção ao alvo de patrulha
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

        // Aplica a rotação no eixo Y para virar o bot
        if (angle > 90 || angle < -90)
        {
            // Rotaciona o bot 180 graus no eixo Y
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            // Mantém a rotação original
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }



        if (distanceToPlayer <= attackRange && isChasing)
        {
            // Aqui você pode chamar a função de ataque ou outro comportamento
            TriggerAttack();
        }
    }

    void TriggerAttack()
    {
        // Lógica para iniciar o ataque do bot
        //Debug.Log("Bot está atacando!");

        GetComponent<CombatController>().SetPerformAttack();

        // Adicione a lógica de ataque aqui
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
        // Verde: Limite da patrulha (movimento aleatório).
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, patrolRange);

        // Amarelo: Limite de detecção (inicia perseguição).
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Vermelho: Limite de perseguição (interrompe perseguição).
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopChaseRange);
    }

}
