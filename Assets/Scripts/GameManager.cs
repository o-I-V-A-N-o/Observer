using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System.IO;
using System;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        /*private void Start()
        {
            if (FindObjectOfType<Watcher>().IsPlayback)
            {
                AutoPlay();
            }

            if (FindObjectOfType<Watcher>().IsWatch)
            {
                File.WriteAllText(Environment.CurrentDirectory + "//game.txt", String.Empty);
            }
        }*/
        public void Logger(string log)
        {
            GetComponent<Watcher>().WtiteToFile(log);
        }

        public void AutoPlay(string data)
        {
            string[] comands = data.Split(' ');

            var parent = GameObject.Find(comands[3]);

            Debug.Log("- " + data);

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

        /*public void WtiteToFile(string data)
        {
            var file = Environment.CurrentDirectory + "//game.txt";

            if (!File.Exists(file))
            {
                File.Create(file);
            }
            else
            {
                //File.WriteAllText(file, String.Empty);
            }

            using (StreamWriter writer = File.AppendText(file))
            {
                writer.Write(data + "\n");
            }
        }*/
    }
}
