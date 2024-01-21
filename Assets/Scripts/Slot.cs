using UnityEngine;

public class Slot : MonoBehaviour
{
    public int row;
    public int position;

    SpriteRenderer spriteRenderer;
    GameObject border;
    Card card;

    // Start is called before the first frame update
    void Start()
    {
        //gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                Debug.Log("Hit slot " + hit.transform.gameObject.name);
                //gameManager.TriggerTileHit(hit.transform.gameObject.GetComponent<Tile>());
            }
        }
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