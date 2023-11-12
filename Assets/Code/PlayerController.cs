using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [SerializeField]
    Animator animator;
    [SerializeField]
    float speed = 1.0f;
    [SerializeField]
    float turnSpeed = 20f;
    public List<Tuple<Vector3, Quaternion>> moveHistory = new List<Tuple<Vector3, Quaternion>>();
    public List<GameObject> bodyList = new List<GameObject>();
    public List<GameObject> midPointBodyList = new List<GameObject>();
    [SerializeField]
    float segmentOffset = 2f;
    float stepOffset = 1f;
    public GameObject snakeSegment;
    bool isAlive = true;
    // Start is called before the first frame update
    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            GameObject.Destroy(gameObject);
        }
        GameManager.OnDeath += OnDeath;
    }
    private void OnDisable()
    {
        GameManager.OnDeath -= OnDeath;
    }
    void OnDeath()
    {
        isAlive = false;
    }
    void Start()
    {
        bodyList.Add(gameObject);
        moveHistory.Add(new Tuple<Vector3, Quaternion>(this.transform.position, this.transform.rotation));
    }

    private void Move()
    {
        stepOffset = segmentOffset / (speed * Time.deltaTime);

        if (Input.GetAxis("Horizontal") > 0)
        {
            //rotate the person
            gameObject.transform.Rotate(transform.up * Time.deltaTime * turnSpeed);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            gameObject.transform.Rotate(-transform.up * Time.deltaTime * turnSpeed);
        }
        for (int i = 1; i < bodyList.Count; i++)
        {
            GameObject currBody = bodyList[i];
            if (moveHistory.Count - i * (int)stepOffset > 0)
            {
                currBody.transform.position = moveHistory[moveHistory.Count - i * (int)stepOffset].Item1;
                currBody.transform.rotation = moveHistory[moveHistory.Count - i * (int)stepOffset].Item2;
            }
            //if last tale remove history
        }
        gameObject.transform.position = transform.position + transform.forward * Time.deltaTime * speed;
        moveHistory.Add(new Tuple<Vector3, Quaternion>(this.transform.position, this.transform.rotation));
    }
    private void FixedUpdate()
    {
        if (isAlive)
        {
            Move();
        }
    }

    public void GrowBody(Mesh mesh, Color color)
    {

        animator.SetTrigger("bite");
        if (snakeSegment != null)
        {
            bodyList.Add(Instantiate(snakeSegment));
        }
        else
        {
            bodyList.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
        }
        bodyList[bodyList.Count - 1].name = "cube" + (bodyList.Count - 1);

        bodyList[bodyList.Count - 1].GetComponent<SnakeBody>().SetColorandShape(mesh, color);
        //make object behind original
        bodyList[bodyList.Count - 1].transform.position =gameObject.transform.position- gameObject.transform.forward * gameObject.transform.lossyScale.y;
    }

}
