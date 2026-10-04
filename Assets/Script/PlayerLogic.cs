using UnityEngine;
using UnityEngine.InputSystem;
using Camera = UnityEngine.Camera;

public class PlayerLogic : MonoBehaviour
{

    Camera cam;
    
    public InputActionAsset InputActions;

    public float howClose = 6f;

    public GameObject[] spawnObjects;
    
    private InputAction m_DragStart_action;
    private InputAction m_DragCurrent_action;
    private InputAction m_DragDelta_action;
    
    public GameObject HeldDice;
    
    bool isDragging = false;
    private IMoveWithPointer _moveWithPointer;

    private void OnEnable()
    {
        InputActions.FindActionMap("Touchscreen Gestures").Enable();
        InputActions.FindActionMap("Android Mouse Interaction").Enable();
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("Touchscreen Gestures").Disable();
        InputActions.FindActionMap("Android Mouse Interaction").Disable();
    }

    private void Awake()
    {
        m_DragStart_action = InputActions.FindAction("Tap Start Position");
        m_DragCurrent_action = InputActions.FindAction("Drag Current Position");
        m_DragDelta_action = InputActions.FindAction("Drag Delta");
    }

    void Start()
    {
        cam = Camera.main;
    }

// Update is called once per frame
    void Update()
    {
        Debug.DrawRay(cam.transform.position, cam.transform.forward, Color.red); 
        if (isDragging)
        {
            if (m_DragCurrent_action.ReadValue<Vector2>().magnitude == 0)//Has player let go of drag
            {
                isDragging = false;
                ResolveRelease();
            }
            else if(HeldDice != null)
            {
                _moveWithPointer.MoveToTagetScreenPosition(m_DragCurrent_action.ReadValue<Vector2>());
            }
        }
        else if (m_DragStart_action.ReadValue<Vector2>().magnitude != 0)//Has player started drag
        {
            isDragging = true;
            SpawnDice(); 
        }
    }


    private void SpawnDice()
    {
        HeldDice = Instantiate(spawnObjects[Random.Range(0,spawnObjects.Length -1 )]);//SpawnDice
        _moveWithPointer = HeldDice.GetComponent<IMoveWithPointer>();
        _moveWithPointer.DragStartWithPointer(m_DragStart_action.ReadValue<Vector2>());

    }

    private void ResolveRelease()
    {
        if (HeldDice != null)
        {
            _moveWithPointer.DragEndWithPointer();
        }
        _moveWithPointer = null;
        HeldDice = null;
    }
}