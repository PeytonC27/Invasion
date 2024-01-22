using TMPro;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public int row;
    public int position;
    public bool HasCard { get; private set; } = false;

    GameManager gameManager;
    SpriteRenderer spriteRenderer;
    GameObject border;
    TMP_Text textLabel;

    public Card? Card { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        
        textLabel = transform.GetChild(0).gameObject.GetComponentInChildren<TMP_Text>();
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
        this.Card = card;
        HasCard = true;
        UpdateSprite(card.Sprite);
    }

    public Card RemoveCard()
    {
        Card temp = this.Card;
        Card = null;
        Sprite sprite = Resources.Load<Sprite>("sprites/empty_slot");
        UpdateSprite(sprite);
        HasCard = false;
        textLabel.text = "";
        return temp;
    }

    /// <summary>
    /// Changes the sprite of this tile
    /// </summary>
    /// <param name="newSprite"></param>
    public void UpdateSprite(Sprite newSprite)
    {
        spriteRenderer.sprite = newSprite;
    }

    public void SetLabel(string text)
    {
        textLabel.text = text;
    }

    public void DisableLabel()
    {
        textLabel.text = "";
    }
}