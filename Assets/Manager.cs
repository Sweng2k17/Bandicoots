using UnityEngine;
using System.Collections;
using UnityEngine.UI; //Need this for calling UI scripts

public class Manager : MonoBehaviour
{

    [SerializeField]
    Transform UIPanel; //Will assign our panel to this variable so we can enable/disable it



    [SerializeField]
    Text timeText;
    [SerializeField]
    MeshRenderer beam;
    [SerializeField]
    Button readButton;
    [SerializeField]
    Slider slider;
    [SerializeField]
    Camera camera;

    bool isPaused; //Used to determine paused state
    public double time;
    private double speed;
    double interval;
    private CSVReader instance;
    private int position = 1;
    private float difference =-1;
    private int maxPosition;
    private string[,] data;

    void Start()
    {
        UIPanel.gameObject.SetActive(false); //make sure our pause menu is disabled when scene starts
        isPaused = false; //make sure isPaused is always false when our scene opens
        time = 0;
        timeText.text = time.ToString();

        //adjust speed here
        speed = 1;
        interval = speed;

    }

    void Update()
    {

        if (readButton.GetComponent<CSVReader>().data != null)
        {


            data = readButton.GetComponent<CSVReader>().data;

            if (difference < 0)
            {
                difference = float.Parse(data[0, 2]);
            }

            //number of milliseconds per step
            //double difference = float.Parse(data[2, 0]);



            //number of lines in csv file
            maxPosition = data.GetLength(1);
            Debug.Log("The value of maaxPosition is " + data.GetLength(1));


            


                float distance = float.Parse(data[2, position]);
                Debug.Log("The value of distance is " + data[2, position]);

                //works accross both distances as the speed is light based
                difference = .0107364f * distance;
                Debug.Log("The value of difference is " + difference);
            if (position < maxPosition)
            {
                position = (int)(time / 60 / difference * 1000);
                Debug.Log("The value of position is " + position);


                float degreesRotation = float.Parse(data[6, position]);
                Debug.Log("The value of rotation is " + data[6, position]);
                float degreesElevation = float.Parse(data[7, position]);
                Debug.Log("The value of elevation is is " + data[7, position]);

                if (data[3, position].Equals("1"))
                {
                    //high powered
                    beam.material.color = Color.red;
                }
                else
                {
                    //low powered
                    beam.material.color = Color.cyan;
                }




                //move slider bar with time
                //slider.value = (float)position / (float)maxPosition;




                Vector3 scale = new Vector3();
                scale.x = 1;
                scale.y = 1;
                scale.z = distance / 100;

                //nextPos.Scale(scale);

                Vector3 rotation = new Vector3();
                rotation.x = degreesElevation;
                rotation.y = degreesRotation;
                rotation.z = 0;



                beam.transform.transform.rotation = Quaternion.Euler(degreesElevation, degreesRotation, 0);
                beam.transform.transform.localScale = scale;

            }
            else
            {
                //file is done reading
                
            }   

            //update postion of beam 8 lines of data per column
            //time, start range, end range, high power, number of pulses, width, azimuth degrees, Elvation degrees
        }
        else
        {
            position = 1;
        }

        //If player presses escape and game is not paused. Pause game. If game is paused and player presses escape, unpause.
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            Pause();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
            {
                UnPause();
            }
        }
        time += interval;
        timeText.text = ((int)time).ToString();

    }

    public void Pause()
    {
        isPaused = true;
        pauseTime();
        UIPanel.gameObject.SetActive(true); //turn on the pause menu
        camera.GetComponent<CameraMovement_script>().isPaused = false;

    }

    public void UnPause()
    {
        isPaused = false;
        UIPanel.gameObject.SetActive(false); //turn off pause menu
        camera.GetComponent<CameraMovement_script>().isPaused = true;

        play();
    }

    public void ExitProgram()
    {
        Application.Quit();
    }

    public void Fastforward()
    {
        interval = 2 * speed;
    }

    public void Rewind()
    {
        interval = -2 * speed;
    }

    public void play()
    {
        interval = speed;

    }

    public void pauseTime()
    {
        interval = 0;
    }

    //called from slider object
    public void setTime()
    {


        double sliderPosition = slider.value;
        position = (int)(sliderPosition * maxPosition);
        time = position * 60 / 1000 * difference;
    }
}