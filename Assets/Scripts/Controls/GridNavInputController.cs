using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridNavInputController : MonoBehaviour
{
    Camera cam;
    Board board;
    Space selectedSpace;
    Space CursorSpace;

    bool usable;
    PlayerMove inputReturnObject;

    private void Awake()
    {
        cam = FindObjectOfType<Camera>();
        board = FindObjectOfType<Board>();
        usable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (usable)
        {
            HighlightRayCast();
        }
    }

    private void HighlightRayCast()
    {
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject g = hit.transform.gameObject;
            Space space;
            if (g.TryGetComponent<Space>(out space))
            {
                if (CursorSpace is null)
                {
                    CursorSpace = space;
                    space.CursorHighlight();
                    return;
                }
                if (CursorSpace == space)
                {
                    return;
                }
                if (CursorSpace != space)
                {
                    CursorSpace.UnCursorHighlight();
                    CursorSpace = space;
                    CursorSpace.CursorHighlight();
                    return;
                }
            }
        }

        if (CursorSpace is null) return;
        CursorSpace.UnCursorHighlight();
        CursorSpace = null;
        return;
    }

    private void OnClick(InputValue value)
    {
        if (usable && value.isPressed)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            //Debug.Log(ray);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject g = hit.transform.gameObject;
                Space space;
                if (g.TryGetComponent<Space>(out space))
                {
                    inputReturnObject.Select(space);
                }
            }
        }
    }

    public void Wake(PlayerMove p)
    {
        inputReturnObject = p;
        usable = true;
    }

    public void Sleep()
    {
        CursorSpace = null;
        inputReturnObject = null;
        usable = false;
    }

    private void OnDebug(InputValue value)
    {
        inputReturnObject.Debug(CursorSpace);
    }
}
