using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToForest : MonoBehaviour
{
    [SerializeField] private bool isOutDoor = false;
    public GameObject interactionHint;
    private bool isPlayerNearby;

    void Start()
    {
        isPlayerNearby = false;
    }
    
    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            GoToForestScene();
        }
    }

    void GoToForestScene()
    {
        SceneManager.LoadScene("Scenes/florest");
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("player"))
        {
            isPlayerNearby = true;
            if (interactionHint != null)
                interactionHint.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("player"))
        {
            isPlayerNearby = false;
            if (interactionHint != null)
                interactionHint.SetActive(false);
        }
    }
}
