using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPointAtMouse : MonoBehaviour
{
    public Transform turret;  // Referencia a la torreta

    void Update()
    {
        // Obtener la posición del ratón en el mundo
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Ajustar la posición Z del ratón para que coincida con el Z del objeto (si es un juego 2D)
        mousePosition.z = turret.position.z;

        // Hacer que la torreta mire hacia el ratón, pero solo en el plano 2D
        turret.up = mousePosition - turret.position;
    }
}
