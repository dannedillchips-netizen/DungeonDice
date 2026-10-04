using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class ThrowObject : MonoBehaviour,IMoveWithPointer
{
    [Header("Custom Object Speed")]
    public float MaxObjectSpeed = 40;
    
    [Space(10)]
    [Header("Flick Speed")]
    public float FlickSpeed = 0.4f;

    public string respawnName = "";
    
    private float startTime, endTime, swipeDistance, swipeTime;
    private Vector2 startPos, endPos;
    private float tempTime;

    private float FlickLength = 100f;
    private float ObjectVelocity = 0;
    private float ObjectSpeed = 0;
    private Vector3 angle;

    private bool thrown, Held = false;
    public float howClose = 3f;
    private Vector2 currentPosition,newPosition;
    Vector3 Velocity;
    
    private Rigidbody rb;
    
    private Camera cam;
    
    public Animator animator;
    
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void Update()
    {
        if (Held)
        {
            tempTime = Time.time - startTime;
            if (tempTime > FlickSpeed)
            {
                startTime = Time.time;
                startPos = newPosition;
            }
            
            currentPosition = Vector2.Lerp(currentPosition, newPosition, Time.deltaTime * 8f);
        
            Vector3 v3 = currentPosition;
            v3.z = cam.nearClipPlane * howClose;
            Vector3 OldPosition = transform.position;
            transform.position = cam.ScreenToWorldPoint(v3);
            Vector3 MovingDir =  (OldPosition - transform.position).normalized;
            //Debug.Log(MovingDir);
            //transform.rotation = Quaternion.LookRotation(MovingDir).ToEuler();
            //transform.rotation = transform.rotation * ((Quaternion.LookRotation(MovingDir * Time.deltaTime)));
        }
    }

    public bool CanBeMovedWithPointer()
    {
        return !(Held || thrown) ;
    }

    public void DragStartWithPointer(Vector2 Screenlocation)
    {
        newPosition = Screenlocation;
        
        startPos = newPosition;
        currentPosition = newPosition;
        Held = true;
        startTime = Time.time;
        
        cam = Camera.main;
        
        Vector3 v3 = Screenlocation;
        v3.z = cam.nearClipPlane * howClose;
        transform.position = cam.ScreenToWorldPoint(v3);
        
        Vector3 pointerDir = (cam.transform.position - cam.ScreenToWorldPoint(v3));
        transform.rotation = Quaternion.LookRotation(pointerDir.normalized);//Get Dice Facing Camera Rotation
    }
    public void MoveToTagetScreenPosition(Vector2 Screenlocation)
    {
        newPosition = Screenlocation;
    }

    public void DragEndWithPointer()
    {
        Held = false;
        
        endTime = Time.time;
        endPos = newPosition;
        swipeDistance = (endPos - startPos).magnitude;
        swipeTime = endTime - startTime;


        //swipeTime < FlickSpeed && swipeDistance > FlickLength && 
        if (endPos.y > startPos.y)
        {
            thrown = true;
            CalSpeed();
            MoveAngle();
            rb.AddForce(angle * ObjectSpeed, ForceMode.Impulse);
            rb.useGravity = true;
            Invoke("_DeSpawn",5);
            Debug.Log("Throw");
        }
        else
        {
            Debug.Log("ToSlow");
            _DeSpawn();
        }
    }

    void _DeSpawn()
    {
        animator.SetBool("Despawn", true);
    }

    void _DeSpawnComplete()
    {
        Destroy(this.gameObject);
    }

    void MoveAngle()
    {
        //angle = cam.ScreenToWorldPoint(new Vector3(endPos.y + 50f,(cam.nearClipPlane)- howClose, cam.nearClipPlane));
        //angle = (cam.transform.forward + cam.transform.up).normalized;
        Vector3 v3Start = startPos;
        v3Start.z = cam.nearClipPlane * howClose;
        v3Start = cam.ScreenToWorldPoint(v3Start);
        
        Vector3 v3End = currentPosition;
        v3End.z = cam.nearClipPlane * howClose;
        v3End = cam.ScreenToWorldPoint(v3End);
        
        Vector3 travelDir = (v3End - v3Start).normalized;
        if (travelDir.y < 0)
        {
            travelDir.y = -travelDir.y;
        }
        angle = (cam.transform.forward + travelDir).normalized;
    }

    void CalSpeed()
    {
        Debug.Log(swipeDistance);
        ObjectSpeed = swipeDistance * 0.05f;
        return;
        FlickLength = swipeDistance;
        if (swipeTime > 0)
        {
            ObjectVelocity = FlickLength / (FlickLength - swipeTime);
        }
        ObjectSpeed =  ObjectVelocity * 5;
        ObjectSpeed = ObjectSpeed - (ObjectSpeed * 1.7f);
        if (ObjectSpeed < -MaxObjectSpeed)
        {
            ObjectSpeed = -MaxObjectSpeed;
        }
        swipeTime = 0;
    }
}