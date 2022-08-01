using UnityEngine;
using System.IO;
using System;

namespace Assets.Scripts
{
    public class Watcher : MonoBehaviour
    {
        public bool IsWatch;
        public bool IsPlayback;

        private void Start()
        {
            Play();
        }

        public void Play()
        {
            if (IsPlayback)
            {
                var file = Environment.CurrentDirectory + "//game.txt";

                if (File.Exists(file))
                {
                    foreach (string data in File.ReadLines(file))
                    {
                        GetComponent<GameManager>().AutoPlay(data);
                    }
                }
                else
                {
                    Debug.Log("Нет файла для воспроизведения!");
                }
            }

            if (IsWatch)
            {
                File.WriteAllText(Environment.CurrentDirectory + "//game.txt", String.Empty);
            }
        }

        public void WtiteToFile(string data)
        {
            if (IsWatch)
            {
                var file = Environment.CurrentDirectory + "//game.txt";

                if (!File.Exists(file))
                {
                    File.Create(file);
                }

                using (StreamWriter writer = File.AppendText(file))
                {
                    writer.Write(data + "\n");
                }
            }
        }
    }
}
