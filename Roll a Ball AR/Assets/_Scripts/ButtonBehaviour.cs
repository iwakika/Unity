using UnityEngine;
using System.Collections;
using Vuforia;
using UnityEngine.SceneManagement;
using System;

public class ButtonBehaviour : MonoBehaviour, Vuforia.IVirtualButtonEventHandler
{

    /// <summary>
    /// Boolean to tell if there is contact, and to do not get the script every frame that there is contact.
    /// </summary>
    private bool contact = false;
    
    // it is used as a lock
    /// <summary>
    /// Boolean used as a lock to see if there is a game object near enough to this game object.
    /// </summary>
    private bool found = false;

    /// <summary>
    /// Maximum distance to tell if there is contact or not
    /// </summary>
    [SerializeField] private int _detectionDistance;
    /// <summary>
    /// Maximum distance to tell if there is contact or not
    /// (get, set)
    /// </summary>
    public int DetectionDistance
    {
        get { return _detectionDistance; }
        set
        {
            if (value >= 0)
            {
                _detectionDistance = value;
            }
        }
    }

    /// <summary>
    /// Game object that represents a rect that will be drawn between this game object and other game object that is near enough.
    /// </summary>
    private GameObject rect;

    /// <summary>
    /// Game object that will be checked. Game objects that are not in this array will not have contact detected
    /// </summary>
    public GameObject[] distanceSubjects;

    /// <summary>
    /// The game object that will act as a base to calulate the distance. Notice that depending the position of this game object the detection distance should be higher or smaller.
    /// </summary>
    public GameObject distanceObserver;

    public PlayerController player;

    public Material buttonMaterial;


    // Use this for initialization
    void Start()
    {
        // Initiate the virtual buttons
        VirtualButtonBehaviour[] vb = GetComponentsInChildren<VirtualButtonBehaviour>();
        foreach (VirtualButtonBehaviour vbb in vb)
        {
            vbb.RegisterEventHandler(this);
        }

    }

    // Update is called once per frame
    void Update()
    {
     
        // check if there was detection and contact   
        foreach (GameObject distanceGameObject in distanceSubjects)
        {
            found = false;
            if (Vector3.Distance(distanceObserver.transform.position, distanceGameObject.transform.position) < _detectionDistance)
            {
                found = true;

                // draws the rect to have a visual response of the detection
                drawRect(distanceGameObject.transform.position);

                if (found && !contact)
                {
                    
                    // creates the rect
                    createRect();
                    
                    contact = true;
                }
                // to get just the first contact
                break;
            }
        }

        // contact recives what is in found, be it true, or false
        // this is a way to ensure that in there was contact, true is kept, if not than contact changes
        contact = found;

        // if there was no contact
        if (!contact)
        {
            
            // destroy the rect
            GameObject.Destroy(rect);
        }
    }


    public void OnButtonPressed(VirtualButtonAbstractBehaviour vb)
    {
        // takes action depend of the button name
        switch (vb.VirtualButtonName)
        {
            case "restart":
                player.Restart();

                buttonMaterial.color = Color.green;

                break;
        }
    }

    public void OnButtonReleased(VirtualButtonAbstractBehaviour vb)
    {
        switch (vb.VirtualButtonName)
        {
            case "restart":
                player.Restart();

                buttonMaterial.color = Color.white;

                break;
        }
    }
    
    /// <summary>
    /// Creates a rect.
    /// </summary>
    private void createRect()
    {
        // reacte a empty game object
        rect = new GameObject();
        // sets the position of the rect as the position of the distanceObserver.
        rect.transform.position = distanceObserver.transform.position;
        // adds a line renderer component on the game object
        rect.AddComponent<LineRenderer>();
    }

    /// <summary>
    /// Change the position of the rect.
    /// </summary>
    /// <param name="endPosition">The new end position if the rect.</param>
    private void drawRect(Vector3 endPosition)
    {
        // if there is a rect
        if (rect != null) {
            // get the line render
            LineRenderer lr = rect.GetComponent<LineRenderer>();
            // sets it material as green
            lr.material = Resources.Load<Material>("GreenMaterial");
            // set its stat position
            lr.SetPosition(0, distanceObserver.transform.position);
            // set its final position 
            lr.SetPosition(1, endPosition);
        }
    }
}
