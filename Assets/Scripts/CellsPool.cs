using UnityEngine;

namespace Assets.Scripts
{
    public class CellsPool : MonoBehaviour
    {
        public GameObject[] CellsInTable;
        public ChipComponent SelectedChip;
        public bool IsWhiteStep = true;
        public int WhiteCount = 12;
        public int BlackCount = 12;
        public bool LockGame = false;
        public bool TheEnd = false;

        private void Start()
        {
            InitCells();
        }

        public void EndGame(string WinnerColor)
        {
            TheEnd = true;
            Debug.Log(WinnerColor + " teams WIN ! ! !");
        }

        public void LockedGame(bool isLocked)
        {
            LockGame = isLocked;
        }

        private void InitCells()
        {
            foreach (GameObject _cells in CellsInTable)
            {
                CellComponent _cell = _cells.GetComponent<CellComponent>();
                _cell.SetNeighbors();
            }
        }
    }
}
