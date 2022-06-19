using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    protected SpriteRenderer sprite;
    protected Material m;
    protected SoundPlayer sp;

    protected Space currentSpace;

    [SerializeField] protected int TeamID = 0, PawnUp = 0;
    [SerializeField] protected int TypeID = 0; //0 = Pawn, 1 = Rook, 2 = Knight, 3 = Bishop, 4 = Queen, 5 = King, 6 = Jammer Pawn

    protected List<Color> teamColors;

    [SerializeField] protected bool FirstMove = true;
    protected bool BeingCaptured = false;
    IPlayerControl captureCallback;

    [SerializeField] protected float FadeEffectTimeScale = 2f;
    [SerializeField] protected float moveSpeed = 5f;

    [SerializeField] AudioClip destroySound, moveSound, landSound;

    protected virtual void Awake()
    {
        
        sprite = FindObjectOfType<SpriteRenderer>();
        teamColors = FindObjectOfType<GameSettings>().TeamColors;
        m = sprite.material;
    }

    // Start is called before the first frame update
    void Start()
    {
        sp = FindObjectOfType<SoundPlayer>();
        sprite.color = teamColors[TeamID];
        switch (TeamID) { case 1: PawnUp = 1; break; case 2: PawnUp = -1; break; }
        m.SetInteger("_IsOnTeam", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (sp == null)
        {
            sp = FindObjectOfType<SoundPlayer>();
        }
    }

    public void SetTeam(int ID) { TeamID = ID; if (sprite is null) sprite = GetComponent<SpriteRenderer>(); sprite.color = teamColors[TeamID]; switch (TeamID) { case 1: PawnUp = 1; break; case 2: PawnUp = -1; break; }
        m.SetInteger("_IsOnTeam", 1);
    }
    public int GetTeam() { return TeamID; }

    public virtual void SetPieceType(int ID) { TypeID = ID; StartCoroutine(PromoteEffect()); }
    public int GetPieceType() { return TypeID; }

    public bool IsFirstMove() { return FirstMove; }

    public void Move(Space targetSpace, IPlayerControl callback)
    {
         callback.ScheduleCallback();
        if (targetSpace.GetPiece() != null)
        {
            targetSpace.GetPiece().CaptureStart(callback);
            gameObject.AddComponent<CircleCollider2D>();
            var rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
        }
        if (TypeID == 0 && targetSpace.transform.position.x != transform.position.x && targetSpace.GetMarker() != null)
        {
            targetSpace.GetMarker().Capture();
        }
        if (TypeID == 0 && targetSpace.transform.position.y == currentSpace.transform.position.y + (PawnUp * 2))
        {
            var marker = new GameObject();
            marker.name = "EnPassant Marker";
            var comp = marker.AddComponent<EnPassantMarker>();
            comp.SetPawn(this);
            comp.SetTeam(TeamID);
            comp.SetSpace(FindObjectOfType<Board>().GetSpaceByCoordinates(new Vector2(currentSpace.transform.position.x,currentSpace.transform.position.y + PawnUp)));
        }
        if (TypeID == 5 && targetSpace.transform.position.x == currentSpace.transform.position.x + 2)
        {
            var rook = FindObjectOfType<Board>().GetSpaceByCoordinates(7, (int)transform.position.y).GetPiece();
            rook.Move(FindObjectOfType<Board>().GetSpaceByCoordinates(5, (int)transform.position.y),callback);
        }
        if (TypeID == 5 && targetSpace.transform.position.x == currentSpace.transform.position.x - 2)
        {
            var rook = FindObjectOfType<Board>().GetSpaceByCoordinates(0, (int)transform.position.y).GetPiece();
            rook.Move(FindObjectOfType<Board>().GetSpaceByCoordinates(3, (int)transform.position.y), callback);
        }

        StartCoroutine(MoveEffect(currentSpace.transform.position, targetSpace.transform.position, callback));
        currentSpace.SetPiece(null);
        currentSpace = targetSpace;
        currentSpace.SetPiece(this);
        FirstMove = false;
    }

    public void SetInitialSpace(Space targetSpace)
    {
        currentSpace = targetSpace;
        currentSpace.SetPiece(this);
        transform.position = currentSpace.transform.position;
    }

    public void CaptureStart(IPlayerControl callback)
    {
        captureCallback = callback;
        callback.ScheduleCallback();
        FindObjectOfType<Pieces>().Remove(this);
        BeingCaptured = true;
        sprite.sortingOrder = 10;
        gameObject.AddComponent<CircleCollider2D>();
        var rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    public void EnPassantCapture()
    {
        StartCoroutine(CaptureFadeEffect());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (BeingCaptured)
        {
            Vector3 point = collision.GetContact(0).point;
            Vector2 dir = new Vector2(point.x,point.y) - new Vector2(transform.position.x + 0.5f, transform.position.y + 0.5f);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            m.SetFloat("_HitAngle", angle - 90);
            StartCoroutine(CaptureFadeEffect());
        }
    }

    private void CaptureEnd()
    {
        captureCallback.Callback();
        Destroy(gameObject);
    }

    public Space GetSpace()
    {
        return currentSpace;
    }

    public List<Space> FindMoves(Board b)
    {
        List<Space> spaces = new List<Space>();

        switch (TypeID)
        {
            case 0:
                spaces.AddRange(MoveFinder.Pawn(b, this, FirstMove, PawnUp, TeamID));
                break;
            case 1:
                spaces.AddRange(MoveFinder.Rook(b, this, TeamID));
                break;
            case 2:
                spaces.AddRange(MoveFinder.Knight(b, this, TeamID));
                break;
            case 3:
                spaces.AddRange(MoveFinder.Bishop(b, this, TeamID));
                break;
            case 4:
                spaces.AddRange(MoveFinder.Queen(b, this, TeamID));
                break;
            case 5:
                spaces.AddRange(MoveFinder.King(b, this, TeamID));
                break;
            case 6:
                spaces.AddRange(MoveFinder.King(b, this, TeamID));
                break;
            default:
                //Debug
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        spaces.Add(b.GetSpaceByCoordinates(new Vector2(i, j)));
                    }
                }
                break;
        }

        return spaces;
    }

    private IEnumerator CaptureFadeEffect()
    {
        sp.PlaySound(destroySound);
        float time = 0;

        while (time < FadeEffectTimeScale)
        {
            m.SetFloat("_Fade", 2 - time);

            time += Time.deltaTime * FadeEffectTimeScale;
            yield return new WaitForEndOfFrame();
        }

        CaptureEnd();
    }

    private IEnumerator MoveEffect(Vector3 start, Vector3 target, IPlayerControl callback)
    {
        sp.PlaySound(moveSound);
        while (transform.position != target)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, Time.deltaTime * moveSpeed);

            yield return new WaitForEndOfFrame();
        }
        if (gameObject.TryGetComponent<CircleCollider2D>(out CircleCollider2D coll))
        { Destroy(coll); }
        if (gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        { Destroy(rb); }
        if (callback != null) callback.Callback();
        sp.PlaySound(landSound);
    }

    private IEnumerator PromoteEffect()
    {
        float time = 0;

        while (time < FadeEffectTimeScale)
        {
            m.SetFloat("_Fade", 2 - (time * 4));

            time += Time.deltaTime * FadeEffectTimeScale;
            yield return new WaitForEndOfFrame();
        }

        sprite.sprite = FindObjectOfType<Pieces>().GetSprite(TypeID);

        while (time > 0f)
        {
            m.SetFloat("_Fade", 2 - (time * 4));

            time -= Time.deltaTime * FadeEffectTimeScale;
            yield return new WaitForEndOfFrame();
        }
    }
}
