using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space : MonoBehaviour
{
    SpriteRenderer sprite;
    SpriteRenderer CancelOverlay;
    SpriteRenderer MoveOverlay;
    SpriteRenderer CaptureOverlay;

    [SerializeField] Color baseColor = Color.white;
    
    Piece pieceOnSpace;
    EnPassantMarker markerOnSpace;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();

        var renderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer s in renderers)
        {
            if (s.gameObject.name == "CancelOverlay") CancelOverlay = s;
            if (s.gameObject.name == "MoveOverlay") MoveOverlay = s;
            if (s.gameObject.name == "CaptureOverlay") CaptureOverlay = s;
        }
    }

    private void Start()
    {
        if ((transform.position.x + transform.position.y) % 2 == 1)
        {
            SetBaseColor(Color.white);
        }
        if ((transform.position.x + transform.position.y) % 2 == 0)
        {
            SetBaseColor(new Color(0.7f,0.7f,0.8f));
        }
    }

    public void SetBaseColor(Color c) { baseColor = c; sprite.color = baseColor; }

    public void CursorHighlight()
    {
    }

    public void UnCursorHighlight()
    {
    }

    public void OverlayCapture()
    {
        CaptureOverlay.color = new Color(CaptureOverlay.color.r, CaptureOverlay.color.g, CaptureOverlay.color.b, 1);
    }

    public void OverlayCancel()
    {
        CancelOverlay.color = new Color(CancelOverlay.color.r, CancelOverlay.color.g, CancelOverlay.color.b, 1);
    }

    public void OverlayMove()
    {
        MoveOverlay.color = new Color(MoveOverlay.color.r, MoveOverlay.color.g, MoveOverlay.color.b, 1);
    }

    public void ClearOverlays()
    {
        CancelOverlay.color = new Color(CancelOverlay.color.r, CancelOverlay.color.g, CancelOverlay.color.b, 0);
        MoveOverlay.color = new Color(MoveOverlay.color.r, MoveOverlay.color.g, MoveOverlay.color.b, 0);
        CaptureOverlay.color = new Color(CaptureOverlay.color.r, CaptureOverlay.color.g, CaptureOverlay.color.b, 0);
    }

    public Piece GetPiece() { return pieceOnSpace; }
    public void SetPiece(Piece p) { pieceOnSpace = p; }

    public EnPassantMarker GetMarker() { return markerOnSpace; }
    public void SetMarker( EnPassantMarker e) { markerOnSpace = e; }
}
