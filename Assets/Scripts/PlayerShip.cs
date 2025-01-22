using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class NewBehaviourScript : MonoBehaviour
{
    public float shipVelocity;
    
    public GameObject bullet = null;
    public float bulletSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveShip();

        if (Input.GetKey(KeyCode.Mouse0))
        {
            ShootBullet();
        }
    }

    private void MoveShip()
    {
        // Get horizontal and vertical input axes
        float moveHorizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float moveVertical = CrossPlatformInputManager.GetAxis("Vertical");

        // Use the inputs to move the player
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
        
        if (movement.magnitude > 1)
        {
            movement.Normalize();
        }
        
        transform.Translate(movement * (Time.deltaTime * shipVelocity));
    }

    private void ShootBullet()
    {
        // Instanciar la bala a la posició de la nau
        Vector3 spawnPosition = transform.position; // Posició de la nau (jugador)
        GameObject bulletNew = Instantiate(bullet, spawnPosition, Quaternion.identity);

        // Obtenir la direcció del ratolí
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = spawnPosition.z; // Assegurar que la posició Z de la bala sigui la mateixa que la nau

        // Calcular la direcció en què ha de volar la bala
        Vector3 direction = (mousePosition - spawnPosition).normalized;

        // Obtenir el Rigidbody de la bala
        Rigidbody rb = bulletNew.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Aplicar la força en la direcció calculada
            rb.AddForce(direction * bulletSpeed, ForceMode.Impulse);
        }
    }
}
