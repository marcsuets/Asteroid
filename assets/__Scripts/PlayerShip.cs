#define DEBUG_PlayerShip_RespawnNotifications

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Rigidbody))]
public class PlayerShip : MonoBehaviour
{
    // This is a somewhat protected private singleton for PlayerShip
    static private PlayerShip   _S;
    static public PlayerShip    S
    {
        get
        {
            return _S;
        }
        private set
        {
            if (_S != null)
            {
                Debug.LogWarning("Second attempt to set PlayerShip singleton _S.");
            }
            _S = value;
        }
    }

    [Header("Set in Inspector")]
    public float        shipSpeed = 10f;
    public GameObject   bulletPrefab;
    public int jumps = 3;
    public float       distanceBetweenSpawn = 5;
    
    
    private int jumpsLeft;
    Rigidbody           rigid;
    private Vector3 deathPosition;
    private GameManager gm;
    private bool allowCollision = true;
    


    void Awake()
    {
        S = this;

        // NOTE: We don't need to check whether or not rigid is null because of [RequireComponent()] above
        rigid = GetComponent<Rigidbody>();
        
        ResetJumpsLeft();
        gm = GameManager.getInstance;
    }

    


    void Update()
    {
        // Using Horizontal and Vertical axes to set velocity
        float aX = CrossPlatformInputManager.GetAxis("Horizontal");
        float aY = CrossPlatformInputManager.GetAxis("Vertical");

        Vector3 vel = new Vector3(aX, aY);
        if (vel.magnitude > 1)
        {
            // Avoid speed multiplying by 1.414 when moving at a diagonal
            vel.Normalize();
        }

        rigid.velocity = vel * shipSpeed;

        // Mouse input for firing
        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }
    
    void Fire()
    {
        // Get direction to the mouse
        Vector3 mPos = Input.mousePosition;
        mPos.z = -Camera.main.transform.position.z;
        Vector3 mPos3D = Camera.main.ScreenToWorldPoint(mPos);

        // Instantiate the Bullet and set its direction
        GameObject go = Instantiate<GameObject>(bulletPrefab);
        go.transform.position = transform.position;
        go.transform.LookAt(mPos3D);
    }

    static public float MAX_SPEED
    {
        get
        {
            return S.shipSpeed;
        }
    }
    
	static public Vector3 POSITION
    {
        get
        {
            return S.transform.position;
        }
    }
    
    private void ResetJumpsLeft()
    {
        jumpsLeft = jumps;
    }

    public int GetJumpsLeft()
    {
        return jumpsLeft;
    }
    
    public String GetJumpsLeftToString()
    {
        return jumpsLeft.ToString();
    }

    public void DecreaseJumpsLeft()
    {
        jumpsLeft--;
    }

    public void KillSpaceShip()
    {
        gameObject.SetActive(false);
        DecreaseJumpsLeft();

        if (jumpsLeft < 0)
        {
            Debug.LogWarning("No jumps left. Destroyed ship.    " + gm);
            gm.GameOver();
            Destroy(gameObject);
        }
        else
        {
            deathPosition = gameObject.transform.position;
            Debug.Log("Jumps left: "+jumpsLeft);
            Invoke("SafeSpawn", 1.5f);
        }
        
    }

    public void SafeSpawn()
    {
        Vector3 pos;
        do
        {
            pos = ScreenBounds.RANDOM_ON_SCREEN_LOC;
        } while ((pos - deathPosition).magnitude < distanceBetweenSpawn);
        
        gameObject.transform.position = pos;
        gameObject.SetActive(true);
    }
    
    public void OnCollisionEnter(Collision other)
    {
        if (allowCollision)
        {
            allowCollision = false;
            KillSpaceShip();
            Invoke("AllowCollisionTrue", 0.5f);
        }
        
    }

    private void AllowCollisionTrue()
    {
        allowCollision = true;
    }
}
