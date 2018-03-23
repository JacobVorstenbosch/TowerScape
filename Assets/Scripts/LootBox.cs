using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class LootBox : MonoBehaviour {

    SerialPort sp = new SerialPort("COM3", 9600);


    [Tooltip("Canvas interaction pane.")]
    public InteractionPane pane;

    public GameObject playerWeaponHand;
    private SwordGenerator swordGen;
    bool active = false;
    bool openable = false;
    bool waitingForReset = false;
    float cooldown = 0.0f;


    private Vector4 AuraColor;

    void Start()
    {
        sp.Open();
        sp.ReadTimeout = 25;

        swordGen = playerWeaponHand.GetComponentInChildren<SwordGenerator>();
        if (!swordGen)
            print("Unable to find sword gen. Lootbox will not function.");
    }

    void Update()
    {

        if (active)
        {
            if (openable && !waitingForReset)
            {
                pane.SetActive(true);
                pane.SetText("Enjoy your new sword! Press A to close.");

                float ranValR = Random.Range(0.0f,255.0f);
                float ranValG = Random.Range(0.0f, 255.0f);

                float ranValB = Random.Range(0.0f, 255.0f);

                AuraColor = new Vector4(ranValR, ranValG, ranValB, 0.7f);

                swordGen.Generate();
                swordGen.bladeMat.SetColor("_ColorR", AuraColor);
                swordGen.bladeMat.SetColor("_Color2", AuraColor);
                openable = false;
                waitingForReset = true;
            }
            else if (waitingForReset)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    pane.SetText("Tap card to open the chest.");
                    waitingForReset = false;
                    openable = false;
                }
            }
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
            string line;

            try
            {
                line = sp.ReadLine();
            }
            catch
            {
                return;
            }
            //string line = sp.ReadTo("\n");
            //print(line);

            string[] split = line.Split(new char[] { '\n', ',', ':' }, System.StringSplitOptions.RemoveEmptyEntries);

            if (active && split[0].Contains("RFID"))
            {
                if (cooldown > 0)
                {
                    cooldown -= Time.deltaTime;
                    return;
                }
                print("RFID CODE RECIEVED: " + split[1]);
                openable = true;
                cooldown = 0.2f;
                return;
            }
            else if (active)
            {
                openable = false;
                return;
            }
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
        //else
        //    print("port open failed");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PlayerRoot"))
        {
            pane.SetActive(true);
            pane.SetText("Tap card to open the chest.");
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
