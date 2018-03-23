using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class LootBox : MonoBehaviour {

    SerialPort sp = new SerialPort("COM6", 9600);


    [Tooltip("Canvas interaction pane.")]
    public InteractionPane pane;

    public GameObject player;
    private SwordGenerator swordGen;
    bool active = false;

    void Start()
    {
        sp.Open();
        sp.ReadTimeout = 25;

        swordGen = player.GetComponent<CollisionTreeManager>().weaponHand.GetChild(1).GetComponent<SwordGenerator>();
        if (!swordGen)
            print("Unable to find sword gen. Lootbox will not function.");
    }

    void Update()
    {
        if (pane.isActiveAndEnabled == true && active)
        {
            if (Input.GetAxis("Fire1") > 0.1f)
            {
                pane.SetText("Tap key to open the chest.");
                pane.SetActive(false);
            }
            return;
        }

        

        if (sp.IsOpen)
        {
            //try
            //{

            //if (sp.BytesToRead < 16)
            //{
            //    print("Waiting... " + sp.BytesToRead);
            //    return;
            //}
            //else
            //    print(sp.ReadExisting());
            string line = sp.ReadLine();
            //string line = sp.ReadTo("\n");
            //print(line);

            string[] split = line.Split(new char[] { '\n', ',', ':' }, System.StringSplitOptions.RemoveEmptyEntries);

            if (split[0].Contains("RFID"))
            {
                print("RFID CODE RECIEVED: " + split[1]);

                //enable pane
                pane.SetActive(true);
                pane.SetText("Enjoy your new sword! Press X to close.");
                swordGen.Generate();

                return;
            }
            else return;
            Vector3 accel = new Vector3(0, 0, 0);
            for (int i = 1; i < 4; i++)
            {
                float tmp = 0;
                float.TryParse(split[i], out tmp);
                switch (i)
                {
                    case 1: accel.x = tmp; break;
                    case 2: accel.y = tmp; break;
                    case 3: accel.z = tmp; break;
                }
            }

            float pitch = (float)(Mathf.Atan2(accel.x, Mathf.Sqrt(accel.y * accel.y + accel.z * accel.z)) * 180.0) / Mathf.PI;
            //print(accel);
            //print(pitch);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -pitch));
            //}
            //catch (System.Exception)
            //{

            //}
        }
        else
            print("port open failed");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PlayerRoot"))
        {
            pane.SetActive(true);
            pane.SetText("Tap key to open the chest.");
            active = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PlayerRoot"))
        {
            pane.SetText(" ");
            pane.SetActive(false);
            active = false;
        }
    }
}
