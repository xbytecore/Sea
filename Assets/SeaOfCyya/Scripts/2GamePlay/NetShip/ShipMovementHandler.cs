using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipMovementHandler : MonoBehaviour
{
    public ShipNetModel shipNetModel;
    [Header("Ship Settings")]
     private float thrustForce = 50f; // Força de propulsão
     private float rotationSpeed = 5f; // Velocidade de rotação
     private float drag = 1f; // Arrasto para simular resistência da água
     private float rotationSmoothness = 10f; // Suavização da rotação

    private Rigidbody2D rb; // Referência ao Rigidbody2D
    private float thrustInput; // Entrada de aceleração (frente/trás)
    private float rotationInput; // Entrada de rotação (esquerda/direita)

    public bool canMove;

    // Chamado uma vez no início
    void Start()
    {
        // Obtém o Rigidbody2D
        shipNetModel = GetComponent<ShipNetModel>();
        rb = GetComponent<Rigidbody2D>();
        rb.linearDamping = drag; // Configura o arrasto
    }

    // Chamado a cada frame para capturar entradas
    void Update()
    {
       // if (!canMove) { return; }

        // Captura as entradas do jogador
     //   thrustInput = Input.GetAxis("Vertical"); // Frente/Trás (W/S ou Setas Cima/Baixo)
        rotationInput = Input.GetAxis("Horizontal"); // Esquerda/Direita (A/D ou Setas)
    }

    // Chamado a cada frame fixo para manipulação de física
    void FixedUpdate()
    {
        //   if (!canMove) { return; }

        // Aplica a rotação
        if (shipNetModel.isLemeOn)
        {
            float targetRotation = rotationInput * rotationSpeed * Time.fixedDeltaTime;
            rb.rotation += targetRotation; // Aplica a rotação diretamente
        }
        
     

        // Suavização da rotação: Aplique apenas se necessário
        // rb.rotation = Mathf.LerpAngle(rb.rotation, rb.rotation + targetRotation, rotationSmoothness * Time.fixedDeltaTime);

        // Aplica a força de propulsão na direção em que o navio está apontando
        if (shipNetModel.isAncorOn || !shipNetModel.isVelaOn) { return; }
        Vector2 force = transform.up * 1 * thrustForce;
        rb.AddForce(force);
    }
}
