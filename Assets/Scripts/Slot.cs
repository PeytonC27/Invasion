using TMPro;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public int row;
    public int position;
    public bool HasCard { get; private set; } = false;

    GameManager gameManager;
    SpriteRenderer spriteRenderer;
    TMP_Text textLabel;
    GameObject highlight;

    public Card Card { get; private set; }

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        textLabel = transform.GetChild(0).GetComponentInChildren<TMP_Text>();
        highlight = transform.GetChild(1).gameObject;

        highlight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        // mouse is over the card
        if (hit.collider != null && hit.transform.gameObject.name == this.name)
        {
            if (Input.GetMouseButtonDown(0))
                gameManager.OnSlotClicked(hit.transform.gameObject.GetComponent<Slot>());

            gameManager.DisplayCardInfo(Card);

            if (!IsEnemySlot())
                highlight.SetActive(true);
        }
        else
        {
            highlight.SetActive(false);
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

    public bool IsEnemySlot()
    {
        return position >= Board.boardLength/2;
    }
}