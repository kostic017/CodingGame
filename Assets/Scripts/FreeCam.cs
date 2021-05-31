using UnityEngine;

public class FreeCam : MonoBehaviour
{
    public float movementSpeed = 100f;

    public float zoomSensitivity = 50f;

    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += transform.up * movementSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.position -= transform.up * movementSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position -= transform.right * movementSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += transform.right * movementSpeed * Time.deltaTime;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            transform.position += transform.forward * movementSpeed * Time.deltaTime;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            transform.position -= transform.forward * movementSpeed * Time.deltaTime;   
        }

        var axis = Input.GetAxis("Mouse ScrollWheel");
        if (axis != 0)
        {
            transform.position += transform.forward * axis * zoomSensitivity;
        }
    }

}
