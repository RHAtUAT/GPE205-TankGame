using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*TODO: Fix Horizonal splitscreen UI scaling
 *TODO: Make scoreBar a class
 *
 */
public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    private bool scaledUI = false;
    private Vector3 UIScale;

    [Range(-1000, 1000)]
    public float inset;

    [Range(-1000, 1000)]
    public float size;

    [Range(0, 3)]
    public int edge;

    //UI elements
    [Header("UI Elements")]
    public Canvas gameUI;
    public TextMeshProUGUI category4Text;     //The 4th column in CategoriesBackground

    [Header("Player 1 UI")]
    public Image scoreUI1;
    public OptionsMenu optionsMenu1;
    public Image player1HealthBackground;
    public Image player1ShieldHealthBackground;
    public TextMeshProUGUI scoreBar1ScoreText;
    public TextMeshProUGUI scoreBar1KillsText;
    public TextMeshProUGUI scoreBar1DeathsText;
    public TextMeshProUGUI p1ScoreBar2ScoreText;
    public TextMeshProUGUI p1ScoreBar2KillsText;
    public TextMeshProUGUI p1ScoreBar2DeathsText;
    public Camera camera1;
    public RectTransform player1UIScreen;
    public RectTransform player1SafeArea;


    [Header("Player 2 UI")]
    public Image scoreUI2;
    public OptionsMenu optionsMenu2;
    public Image player2HealthBackground;
    public Image player2ShieldHealthBackground;
    public TextMeshProUGUI p2ScoreBar1ScoreText;
    public TextMeshProUGUI p2ScoreBar1KillsText;
    public TextMeshProUGUI p2ScoreBar1DeathsText;
    public TextMeshProUGUI scoreBar2ScoreText;
    public TextMeshProUGUI scoreBar2KillsText;
    public TextMeshProUGUI scoreBar2DeathsText;
    public Camera camera2;
    public RectTransform player2UIScreen;
    public RectTransform player2SafeArea;



    void Awake()
    {

        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UIScale = player1SafeArea.transform.localScale;

        SetScreenLayout();

        if (GameManager.instance.splitScreen == true)
        {
            player2UIScreen.gameObject.SetActive(true);
        }
        if (GameManager.instance.limitedLives == true)
        {
            category4Text.text = "Lives";
        }
    }

    public void DeathScreen(Player player)
    {

    }

    public void SetScreenLayout()
    {
        //Toggle Splitscreen Options
        optionsMenu1.splitScreenOptions.gameObject.SetActive(GameManager.instance.splitScreen);
        optionsMenu2.splitScreenOptions.gameObject.SetActive(GameManager.instance.splitScreen);

        //Debug.Log("player1UISize: " + player1UIScreen.sizeDelta);


        float scaledGameUIHeight = (gameUI.pixelRect.height / gameUI.scaleFactor);
        float scaledGameUIWidth = (gameUI.pixelRect.width / gameUI.scaleFactor);

        //Scale the UI to half its size
        if (GameManager.instance.splitScreen == true && scaledUI == false)
        {
            player1SafeArea.transform.localScale = new Vector3(player2SafeArea.transform.localScale.x / 2, player2SafeArea.transform.localScale.y / 2, player2SafeArea.transform.localScale.z);
            player2SafeArea.transform.localScale = new Vector3(player2SafeArea.transform.localScale.x / 2, player2SafeArea.transform.localScale.y / 2, player2SafeArea.transform.localScale.z);
            scaledUI = true;

        }

        //Debug.Log("ScreenLayout: " + PlayerPrefs.GetInt("ScreenLayout", 0));
        //Debug.Log("Camera Config: " + GameManager.instance.cameraConfig);

        switch (GameManager.instance.cameraConfig)
        {
            case CameraConfig.SinglePlayer:
                camera1.rect = new Rect(0, 0, 1.0f, 1.0f);
                camera2.gameObject.SetActive(false);
                break;

            case CameraConfig.SplitScreenH:

                camera2.gameObject.SetActive(true);

                //Set the cameras to half of the screen size vertically
                camera1.rect = new Rect(0, 0.5f, 1.0f, 0.5f);
                camera2.rect = new Rect(0, 0, 1.0f, 0.5f);

                //Scale the UI area to half of the screen
                player1SafeArea.sizeDelta = new Vector2(scaledGameUIWidth, scaledGameUIHeight / 2);
                player1UIScreen.sizeDelta = new Vector2(scaledGameUIWidth, scaledGameUIHeight / 2);
                player1UIScreen.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, scaledGameUIHeight / 2);

                //Scale the UI area to half of the screen
                player2SafeArea.sizeDelta = new Vector2(scaledGameUIWidth, scaledGameUIHeight / 2);
                player2UIScreen.sizeDelta = new Vector2(scaledGameUIWidth, scaledGameUIHeight / 2);
                player2UIScreen.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, scaledGameUIHeight / 2);
                break;

            case CameraConfig.SplitScreenV:

                camera2.gameObject.SetActive(true);

                camera1.rect = new Rect(0, 0, 0.5f, 1f);
                camera2.rect = new Rect(0.5f, 0, 0.5f, 1f);

                //Scale the UI area to half the screen
                player1SafeArea.sizeDelta = new Vector2(scaledGameUIWidth / 2, scaledGameUIHeight);
                player1UIScreen.sizeDelta = new Vector2(scaledGameUIWidth / 2, scaledGameUIHeight);
                player1UIScreen.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, scaledGameUIWidth / 2);

                //Scale the UI area to half the screen
                player2SafeArea.sizeDelta = new Vector2(scaledGameUIWidth / 2, scaledGameUIHeight);
                player2UIScreen.sizeDelta = new Vector2(scaledGameUIWidth / 2, scaledGameUIHeight);
                player2UIScreen.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, scaledGameUIWidth / 2);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.player1.pawn != null)
        {

            //Keep player1's scoreboard updated with the correct values
            scoreBar1ScoreText.text = GameManager.instance.player1.pawn.stats.score.ToString();
            scoreBar1KillsText.text = GameManager.instance.player1.pawn.stats.kills.ToString();

            p1ScoreBar2ScoreText.text = GameManager.instance.player2.pawn.stats.score.ToString();
            p1ScoreBar2KillsText.text = GameManager.instance.player2.pawn.stats.kills.ToString();

            if (GameManager.instance.limitedLives == false)
            {
                scoreBar1DeathsText.text = GameManager.instance.player1.pawn.stats.deaths.ToString();
                p1ScoreBar2DeathsText.text = GameManager.instance.player2.pawn.stats.deaths.ToString();
            }
            else
            {
                scoreBar1DeathsText.text = GameManager.instance.player1.pawn.stats.lives.ToString();
                p1ScoreBar2DeathsText.text = GameManager.instance.player2.pawn.stats.lives.ToString();

            }
        }

        if (GameManager.instance.player2 != null)
        {
            //Keep player2's scoreboard updated with the correct values
            scoreBar2ScoreText.text = GameManager.instance.player2.pawn.stats.score.ToString();
            scoreBar2KillsText.text = GameManager.instance.player2.pawn.stats.kills.ToString();

            p2ScoreBar1ScoreText.text = GameManager.instance.player1.pawn.stats.score.ToString();
            p2ScoreBar1KillsText.text = GameManager.instance.player1.pawn.stats.kills.ToString();


            if (GameManager.instance.limitedLives == false)
            {
                scoreBar2DeathsText.text = GameManager.instance.player2.pawn.stats.deaths.ToString();
                p2ScoreBar1DeathsText.text = GameManager.instance.player1.pawn.stats.deaths.ToString();
            }
            else
            {
                scoreBar2DeathsText.text = GameManager.instance.player2.pawn.stats.lives.ToString();
                p2ScoreBar1DeathsText.text = GameManager.instance.player1.pawn.stats.lives.ToString();
            }
        }
    }
}
