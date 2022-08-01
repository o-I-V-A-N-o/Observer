using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public bool GameIsLocked()
        {
            if (GetComponent<Watcher>().IsPlayback)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Logger(string log)
        {
            GetComponent<Watcher>().WtiteToFile(log);
        }

        public void AutoPlay(string data)
        {
            string[] comands = data.Split(' ');

            var parent = GameObject.Find(comands[3]);

            switch (comands[0])
            {
                case "Select":
                    parent.GetComponentInChildren<ChipComponent>().OnClickMe(parent.transform.position);
                    break;
                case "Move":
                    parent.GetComponent<CellComponent>().OnClickMe(parent.transform.position);
                    break;
                case "Delete":
                    parent.GetComponent<CellComponent>().CheckChipDelete();
                    break;
            }
        }
    }
}
