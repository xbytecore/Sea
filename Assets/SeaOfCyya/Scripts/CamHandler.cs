using UnityEngine;

public class CamHandler : MonoBehaviour
{
    [Header("Player Settings")]
    public Transform player; // Referência ao jogador

    [Header("Camera Settings")]
    public float smoothSpeed = 0.125f; // Velocidade de suavização
    public Vector3 offset; // Distância entre a câmera e o jogador

    void Start()
    {
        // Definindo o offset, caso queira um valor padrão
        if (offset == Vector3.zero)
        {
          //  offset = new Vector3(0, 2, -10); // Exemplo de offset, ajustável conforme necessidade
        }
        Camera.main.transform.localPosition = new Vector3(0, 0, -1); // Posi��o relativa
        Camera.main.transform.localRotation = Quaternion.Euler(0, 0, 0); // Rota��o relativa
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // Posição desejada da câmera (com o offset)
            Vector3 desiredPosition = new Vector3 (player.position.x,player.position.y,-1) + offset;

            // Suavizando o movimento da câmera
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Atualiza a posição da câmera
            transform.position = smoothedPosition;
        }
    }
}
