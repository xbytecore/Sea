using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementHandler : NetworkBehaviour
{
    [Header("Movement Settings")]
    public ShipNetModel shipNetModel;
    [SerializeField] private float moveSpeed = 5f; // Velocidade de movimento
    [SerializeField] private GameObject projectilePrefab; // Referência ao prefab do projétil
    public Transform shootingPoint; // Ponto de onde o projétil será disparado

    private Rigidbody2D rb; // Referência ao Rigidbody2D
    public Animator animator; // Referência ao Animator
    private Vector2 movementInput; // Entrada do jogador
    public bool canMove;

    public Rigidbody2D playerRB;
    public  CombatController combatController;

    // Chamado uma vez quando o objeto é inicializado
    void Start()
    {
        // Obtém o componente Rigidbody2D e Animator anexados ao objeto
      
        combatController = GetComponent<CombatController>();    
        rb = GetComponent<Rigidbody2D>();
        playerRB = gameObject.GetComponent<Rigidbody2D>();
       // animator = GetComponent<Animator>();
    }
   
    // Chamado a cada frame para capturar entradas
    void Update()
    {
        if (!GetComponent<NetPlayerController>().isOwned) { return; } // Garante que apenas o jogador local controle o movimento e atire

        

        if (GetComponent<NetPlayerController>().netClientController.netShipController.gameObject.GetComponent<ShipNetModel>().isLemeOn) { return; }
        if (!combatController.combatModel.canAttack) {
            rb.linearVelocity = Vector3.zero;

            // Zerar a velocidade angular (rotação)
            rb.angularVelocity = 0;

            return; }
        // Captura as entradas do teclado (horizontal e vertical)
        movementInput.x = Input.GetAxis("Horizontal");
        movementInput.y = Input.GetAxis("Vertical");

        // Verifica se o jogador pressionou a tecla 'K' para atirar
        if (Input.GetKeyDown(KeyCode.K))
        {
            CmdShoot(); // Chama o comando para atirar
        }

       

        // Atualiza o estado da animação
        UpdateAnimation();
    }

    // Chamado a cada frame fixo para manipulação de física
    void FixedUpdate()
    {
        if (!GetComponent<NetPlayerController>().isOwned) { return; }
        ApplyShipVelocityToPlayer();
    }

    // Método para aplicar a velocidade do barco ao jogador
    void ApplyShipVelocityToPlayer()
    {
        if (GetComponent<NetPlayerController>().netClientController.netShipController.gameObject.GetComponent<ShipNetModel>().isLemeOn)
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

        if (movementInput.x > 0) // Se o jogador se mover para a direita
        {
            // Gira para a direita (sem rotação no eixo Y)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (movementInput.x < 0) // Se o jogador se mover para a esquerda
        {
            // Gira 180 graus no eixo Y para virar para a esquerda
            transform.rotation = Quaternion.Euler(0, 180, 0);
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
