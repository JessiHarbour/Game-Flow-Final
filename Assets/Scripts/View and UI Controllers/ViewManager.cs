using UnityEngine;

namespace View_and_UI_Controllers
{
    public class ViewManager : MonoBehaviour
    {
        public GameObject deckView;
        public GameObject overboardView;
        public GameObject belowDeckView;

        public GameObject checkWindowView;
        public GameObject checkTableView;

        void Start()
        {
            ShowDeckView();
        }

        public void ShowDeckView()
        {
            deckView.SetActive(true);
            overboardView.SetActive(false);
            belowDeckView.SetActive(false);
            checkWindowView.SetActive(false);
            checkTableView.SetActive(false);
        }

        public void ShowOverboardView()
        {
            deckView.SetActive(false);
            overboardView.SetActive(true);
            belowDeckView.SetActive(false);
            checkWindowView.SetActive(false);
            checkTableView.SetActive(false);
        }

        public void ShowBelowDeck()
        {
            deckView.SetActive(false);
            overboardView.SetActive(false);
            belowDeckView.SetActive(true);
            checkWindowView.SetActive(false);
            checkTableView.SetActive(false);
        }

        public void ShowCheckWindowView()
        {
            deckView.SetActive(false);
            overboardView.SetActive(false);
            belowDeckView.SetActive(false);
            checkWindowView.SetActive(true);
            checkTableView.SetActive(false);
        }

        public void ShowCheckTableView()
        {
            deckView.SetActive(false);
            overboardView.SetActive(false);
            belowDeckView.SetActive(false);
            checkWindowView.SetActive(false);
            checkTableView.SetActive(true);
        }
    }
}