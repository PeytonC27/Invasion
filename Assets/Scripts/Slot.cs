using UnityEngine;

public class Slot : MonoBehaviour
{
    public int row;
    public int position;
    public bool HasCard { get; private set; } = false;

    GameManager gameManager;
    SpriteRenderer spriteRenderer;
    GameObject border;
    Card? card;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null && hit.transform.gameObject.name == this.name)
            {
                gameManager.OnSlotClicked(hit.transform.gameObject.GetComponent<Slot>());
            }
        }
    }

    public void AddCard(Card card)
    {
        this.card = card;
        HasCard = true;
        UpdateSprite(card.Sprite);
    }

    public void RemoveCard()
    {
        Sprite sprite = Resources.Load<Sprite>("sprites/empty_slot");
        UpdateSprite(sprite);
        card = null;
        HasCard = false;
    }

    /// <summary>
    /// Changes the sprite of this tile
    /// </summary>
    /// <param name="newSprite"></param>
    public void UpdateSprite(Sprite newSprite)
    {
        spriteRenderer.sprite = newSprite;
    }

    public void EnableBorder()
    {
        border.SetActive(true);
    }

    public void DisableBorder()
    {
        border.SetActive(false);
    }
}