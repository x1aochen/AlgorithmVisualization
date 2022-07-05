using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField, Range(1f, 100f)]
    private float moveSpeed = 30f;

    //[SerializeField, Range(0.1f, 1f)]
    //private float rotateSpeed = 0.3f;

    //private Vector3 preMousePos;

    //private void Update()
    //{
    //    float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
    //    //�����ƶ�����
    //    if (scrollWheel != 0f)
    //    {
    //        //��ס�Ҽ����ı䵱ǰ�ƶ��ٶ�
    //        if (Input.GetMouseButton(1))
    //        {
    //            ChangeSpeed(scrollWheel);
    //        } 
    //        else //û�а�ס�Ҽ����ƶ����
    //        {
    //            MouseWheel(scrollWheel);
    //        }
    //    }

    //    //�����Ҽ�����¼λ�ã���ʼ�϶�
    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        preMousePos = Input.mousePosition;
    //    }

    //    MouseDrag(Input.mousePosition);

    //}

    //private void FixedUpdate()
    //{
    //    MoveCamera();
    //}

    private void ChangeSpeed(float delta)
    {
        moveSpeed += delta * 10;
        if (moveSpeed > 100)
        {

            moveSpeed = 100;
        }
        if (moveSpeed < 1)
        {
            moveSpeed = 1;
        }
    }

    private void MouseWheel(float delta)
    {

        transform.position += transform.forward * delta * moveSpeed;
        return;
    }

    //private void MouseDrag(Vector3 mousePos)
    //{
    //    Vector3 diff = mousePos - preMousePos;
    //    //С��kEpsilon�ж�Ϊû���ƶ�
    //    if (diff.magnitude < Vector3.kEpsilon)
    //        return;

    //    //if (Input.GetMouseButton(2))
    //    //{
    //    //    transform.Translate(-diff * Time.deltaTime * moveSpeed);
    //    //}
    //    //else
    //    if (Input.GetMouseButton(1))
    //        CameraRotate(new Vector2(-diff.y, diff.x) * rotateSpeed);

    //    preMousePos = mousePos;
    //}

    //public void CameraRotate(Vector2 angle)
    //{
    //    transform.RotateAround(transform.position, transform.right, angle.x);
    //    transform.RotateAround(transform.position, Vector3.up, angle.y);
    //}


    public void MoveCamera()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        if (hor != 0 || ver != 0)
        {
            transform.Translate(hor * Time.deltaTime * moveSpeed, 0, ver * Time.deltaTime * moveSpeed);
        }

    }
    private float mouseX, mouseY;
    public float mouseSensitivity = 100f;
    public float xRotation;
    private void Update()
    {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
            //�����ƶ�����
        if (scrollWheel != 0f)
        {
            //��ס�Ҽ����ı䵱ǰ�ƶ��ٶ�
            if (Input.GetMouseButton(1))
            {
                ChangeSpeed(scrollWheel);
            } 
            else //û�а�ס�Ҽ����ƶ����
            {
                MouseWheel(scrollWheel);
            }
        }
        if (Input.GetMouseButton(1))
        {
            mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            transform.Rotate(Vector3.up * mouseX, Space.World);
            transform.Rotate(Vector3.right * -mouseY);
        }
        MoveCamera();
    }
}
