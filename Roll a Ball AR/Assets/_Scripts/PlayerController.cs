using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {

    public float speed;

    public Text countText;

    public Text winText;

    private Rigidbody rb;

    private int count;

    private Renderer render;

    //private bool gravityEnable;

	[SerializeField] private bool _move;
    public bool Move {
        get
        {
            return _move;
        }
        set
        {
            _move = value;
            rb.useGravity = value;
        }
    }

	public bool exemplo { get; set;}

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start () {
        render = gameObject.GetComponent<Renderer>();

        //gravityEnable = true;
        _move = true;

        Restart();
	}
	
	// Update is called once per frame
    // before render the frame
	void Update () {
        
	}

    // before physics
    void FixedUpdate()
    {
        if (_move) {

            float horizontalMove, verticalMove;
            //horizontalMove = Input.GetAxis("Horizontal");
            //verticalMove = Input.GetAxis("Vertical");
            horizontalMove = CrossPlatformInputManager.GetAxis("Horizontal");
            verticalMove = CrossPlatformInputManager.GetAxis("Vertical");

            Vector3 movement = new Vector3(horizontalMove, 0, verticalMove);

            rb.AddForce(movement * speed);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pick Up")
        {
            other.gameObject.SetActive(false);
            count++;

            SetCountText();
        }
    }

    void SetCountText()
    {
        countText.text = "Pontos: " + count.ToString();

        if (count >= 12)
        {
            winText.text = "Você venceu";
        }
    }

    public void StopPlayer()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void Restart()
    {
        count = 0;
        SetCountText();

        winText.text = "";

        GameObject pickups = GameObject.FindGameObjectWithTag("Pick Up Holder");
        foreach (Transform pick in pickups.transform)
        {
            pick.gameObject.SetActive(true);
        }

        StopPlayer();

        gameObject.transform.localPosition = new Vector3(0, 0.05f, 0);
    }
}
