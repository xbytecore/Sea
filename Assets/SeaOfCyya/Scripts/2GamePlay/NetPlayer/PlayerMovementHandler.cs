using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementHandler : NetworkBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f; // Velocidade de movimento
    [SerializeField] private GameObject projectilePrefab; // Referência ao prefab do projétil
    public Transform shootingPoint; // Ponto de onde o projétil será disparado

    private Rigidbody2D rb; // Referência ao Rigidbody2D
    public Animator animator; // Referência ao Animator
    private Vector2 movementInput; // Entrada do jogador
    public bool canMove;

    public Rigidbody2D playerRB;

    // Chamado uma vez quando o objeto é inicializado
    void Start()
    {
        // Obtém o componente Rigidbody2D e Animator anexados ao objeto
        rb = GetComponent<Rigidbody2D>();
        playerRB = gameObject.GetComponent<Rigidbody2D>();
       // animator = GetComponent<Animator>();
    }

    // Chamado a cada frame para capturar entradas
    void Update()
    {
        if (!GetComponent<NetPlayerController>().isOwned) { return; } // Garante que apenas o jogador local controle o movimento e atire

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!GetComponent<MovementHandler>().canMove)
            {
                GetComponent<NetPlayerController>().netClientController.netShipController.GetComponent<ShipMovementHandler>().canMove = false;
                GetComponent<MovementHandler>().canMove = true;
            }
            else
            {
                GetComponent<NetPlayerController>().netClientController.netShipController.GetComponent<ShipMovementHandler>().canMove = true;
                GetComponent<MovementHandler>().canMove = false;
            }
        }

        if (!canMove) { return; }

        // Captura as entradas do teclado (horizontal e vertical)
        movementInput.x = Input.GetAxis("Horizontal");
        movementInput.y = Input.GetAxis("Vertical");

        // Verifica se o jogador pressionou a tecla 'K' para atirar
        if (Input.GetKeyDown(KeyCode.K))
        {
            CmdShoot(); // Chama o comando para atirar
        }

        // Rotaciona o personagem de acordo com a direção de movimento
        if (movementInput.magnitude > 0) // Se o jogador estiver se movendo
        {
           // float angle = Mathf.Atan2(movementInput.y, movementInput.x) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        // Atualiza o estado da animação
        UpdateAnimation();
    }

    // Chamado a cada frame fixo para manipulação de física
    void FixedUpdate()
    {
        ApplyShipVelocityToPlayer();
    }

    // Método para aplicar a velocidade do barco ao jogador
    void ApplyShipVelocityToPlayer()
    {
        if (!canMove)
        {
            // Obtém o Rigidbody2D do barco e do jogador
            Rigidbody2D shipRb = GetComponent<NetPlayerController>().netClientController.netShipController.GetComponent<Rigidbody2D>();

            // Soma a velocidade do barco com a do jogador para que ele possa se mover e também ser empurrado pelo barco
            playerRB.linearVelocity = shipRb.linearVelocity;
        }
        else
        {
            // Quando o jogador pode mover, aplica o movimento controlado
            playerRB.linearVelocity = movementInput * moveSpeed;
        }
    }

    // Método que chama o comando para disparar
    [Command]
    void CmdShoot()
    {
        // Instancia o projétil no servidor
        GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position, shootingPoint.rotation);

        // Spawna o projétil na rede
        NetworkServer.Spawn(projectile);

        // Aplica a velocidade no projétil

    }

    // Atualiza as animações baseadas no movimento
    void UpdateAnimation()
    {
        bool isMoving = movementInput.magnitude > 0; // Verifica se o jogador está se movendo
        animator.SetBool("isWalking", isMoving); // Atualiza o parâmetro do Animator

        if (movementInput.x != 0) // Apenas se houver movimento horizontal
        {
          //  Vector3 localScale = transform.localScale;
         //   localScale.x = movementInput.x > 0 ? 1 : -1; // Define o flip dependendo da direção
           // transform.localScale = localScale;
        }
    }
}
