using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.

        PlayerAction inputActions;

        public Vector2 moveAxis;
        [SerializeField]
        float m_InputSensitivity = 1.5f;

        bool m_HasInput;
        Vector3 m_InputPosition;
        Vector3 m_PreviousInputPosition;

        // The speed at which the object will move
        public float speed = 10.0f;
        TURN currentTurn = TURN.FORWARD;


        float AccelerometerUpdateInterval  = 1f / 60.0f;
        float LowPassKernelWidthInSeconds  = .1f;

        private float LowPassFilterFactor => AccelerometerUpdateInterval / LowPassKernelWidthInSeconds; // tweakable
        private Vector3 lowPassValue;

        void Awake()
        {
            inputActions = new PlayerAction();
            Application.targetFrameRate = 30;
        }
        private void Start()
        {
            lowPassValue = Input.acceleration;
            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();


            //inputActions.Player.Move.performed += ctx => moveAxis.x = ctx.ReadValue<Vector2>().x;
            //inputActions.Player.Move.canceled += ctx => moveAxis.x = ctx.ReadValue<Vector2>().x;

            //inputActions.Player.Move.performed += ctx => moveAxis.y = ctx.ReadValue<Vector2>().y;
            //inputActions.Player.Move.canceled += ctx => moveAxis.y = ctx.ReadValue<Vector2>().y;

            inputActions.Player.Jump.performed += ctx => m_Jump = ctx.ReadValueAsButton();
            inputActions.Player.Jump.canceled += ctx => m_Jump = ctx.ReadValueAsButton();
        }
        private void Update()
        {
            // if (!m_Jump)
            //{
            //  m_Jump = Input.GetButtonDown("Jump");
            // }
            UnityAccelerometer();

#if UNITY_EDITOR
            m_InputPosition = Mouse.current.position.ReadValue();

            if (Mouse.current.leftButton.isPressed)
            {
                if (!m_HasInput)
                {
                    m_PreviousInputPosition = m_InputPosition;
                }
                m_HasInput = true;
            }
            else
            {
                m_HasInput = false;
            }
#else
            if (Touch.activeTouches.Count > 0)
            {
                m_InputPosition = Touch.activeTouches[0].screenPosition;

                if (!m_HasInput)
                {
                    m_PreviousInputPosition = m_InputPosition;
                }
                
                m_HasInput = true;
            }
            else
            {
                m_HasInput = false;
            }
#endif

            if (m_HasInput)
            {
                float normalizedDeltaPosition = (m_InputPosition.x - m_PreviousInputPosition.x) / Screen.width * m_InputSensitivity;
                float deltaPos = (m_InputPosition.y - m_PreviousInputPosition.y) / Screen.height * m_InputSensitivity;

                if (normalizedDeltaPosition >= 0.1f)
                {
                    if (currentTurn == TURN.FORWARD) DoTurn(TURN.RIGHT);
                    else DoTurn(TURN.FORWARD);
                }
                else if (normalizedDeltaPosition <= -0.1f) 
                {
                    if (currentTurn == TURN.FORWARD) DoTurn(TURN.LEFT);
                    else DoTurn(TURN.FORWARD);
                }
                if (deltaPos >= 0.1f)
                {
                    //Debug.Log("Up");
                    m_Jump = true;
                }
                //else if (deltaPos <= -0.1f)
                //{
                //    Debug.Log("Down");
                //}
            }

            m_PreviousInputPosition = m_InputPosition;
        }

        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
            float h = 0;//Input.GetAxis("Horizontal");
            float v = 0f;// moveAxis.y;//Input.GetAxis("Vertical");
            bool crouch = false;// = Input.GetKey(KeyCode.C);

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v*m_CamForward + h*m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v*Vector3.forward + h*Vector3.right;
            }
#if !MOBILE_INPUT
			// walk speed multiplier
	        if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

            // pass all parameters to the character control script
            m_Character.Move(m_Move, crouch, m_Jump);
            m_Jump = false;

        }
        void DoTurn(TURN turn)
        {
            switch (turn)
            {
                case TURN.FORWARD:
                    Debug.Log("Forward");
                    transform.rotation = new Quaternion(0f, 0f, 0f, 1f);
                    currentTurn = TURN.FORWARD;
                    break;
                case TURN.RIGHT:
                    Debug.Log("Right");
                    transform.rotation = new Quaternion(0f, 0.707106829f, 0f, 0.707106829f);
                    currentTurn = TURN.RIGHT;
                    break;
                case TURN.LEFT:
                    Debug.Log("Left");
                    transform.rotation = new Quaternion(0f, -0.707106829f, 0f, 0.707106829f);
                    currentTurn = TURN.LEFT;
                    break;
            }
        }
        Vector3 LowPassFilterAccelerometer()
        {
            lowPassValue = Vector3.Lerp(lowPassValue, Input.acceleration, LowPassFilterFactor);
            return lowPassValue * Time.deltaTime;
        }
        void UnityAccelerometer()
        {
            if(currentTurn == TURN.FORWARD)transform.position = new Vector3(Mathf.Clamp(transform.position.x + LowPassFilterAccelerometer().x * speed, -4f, 4f), transform.position.y, transform.position.z);
           // else transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.x + LowPassFilterAccelerometer().z * speed, -4f, 4f));
        }
      
        void AcelerometerMove()
        {
            // Get the accelerometer's x and y values
            float x = Input.acceleration.x;
            Debug.Log(Math.Round(x));
            //float y = Input.acceleration.y;

            // Create a vector using the x and y values
            Vector3 movement = new Vector3(transform.position.x + x, transform.position.y, transform.position.z);

            // Move the object based on the accelerometer's x and y values
            //transform.position = movement;
        }

        #region Input ENABLED/DESABLED
        private void OnEnable()
        {
            inputActions.Enable();
            EnhancedTouchSupport.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
            EnhancedTouchSupport.Disable();
        }
        #endregion
    }
}
[Serializable]
public enum TURN
{
    FORWARD,
    RIGHT,
    LEFT,
}