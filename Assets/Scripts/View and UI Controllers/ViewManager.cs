using UnityEngine;

namespace View_and_UI_Controllers
{
    public class ViewManager : MonoBehaviour
    {
        public static ViewManager Instance; 

        public enum PlayerView { Deck, Overboard, BelowDeck, Window, Table }

        public PlayerView currentView;

        public GameObject deckView;
        public GameObject overboardView;
        public GameObject belowDeckView;

        public GameObject checkWindowView;
        public GameObject checkTableView;

        void Awake()
        {
            // Singleton setup
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        void Start()
        {
            ShowDeckView();
        }

        public void ShowDeckView()
        {
            currentView = PlayerView.Deck;

            deckView.SetActive(true);
            overboardView.SetActive(false);
            belowDeckView.SetActive(false);
            checkWindowView.SetActive(false);
            checkTableView.SetActive(false);
        }

        public void ShowOverboardView()
        {
            currentView = PlayerView.Overboard;

            deckView.SetActive(false);
            overboardView.SetActive(true);
            belowDeckView.SetActive(false);
            checkWindowView.SetActive(false);
            checkTableView.SetActive(false);

            MonsterManager.Instance.EnterView(ThreatType.Overboard);
        }

        public void ShowBelowDeck()
        {
            currentView = PlayerView.BelowDeck;

            deckView.SetActive(false);
            overboardView.SetActive(false);
            belowDeckView.SetActive(true);
            checkWindowView.SetActive(false);
            checkTableView.SetActive(false);
        }

        public void ShowCheckWindowView()
        {
            currentView = PlayerView.Window;

            deckView.SetActive(false);
            overboardView.SetActive(false);
            belowDeckView.SetActive(false);
            checkWindowView.SetActive(true);
            checkTableView.SetActive(false);

            MonsterManager.Instance.EnterView(ThreatType.Window);
        }

        public void ShowCheckTableView()
        {
            currentView = PlayerView.Table;

            deckView.SetActive(false);
            overboardView.SetActive(false);
            belowDeckView.SetActive(false);
            checkWindowView.SetActive(false);
            checkTableView.SetActive(true);

            MonsterManager.Instance.EnterView(ThreatType.Table);
        }
    }
}
