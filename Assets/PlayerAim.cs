using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public Transform firePoint;
    public float reticleOffset;
    public Transform reticle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAim();
    }
    [HideInInspector]
    public float aimAngle;
    Vector2 mousePosition;
    [HideInInspector]
    public Vector2 aimDirection;
    public void UpdateAim()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimDirection = (mousePosition - (Vector2)transform.position).normalized;
        aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, aimAngle);
        reticle.position = new Vector2(transform.position.x + aimDirection.x * reticleOffset, transform.position.y + aimDirection.y * reticleOffset);
    }
}
