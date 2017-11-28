using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Circle : MonoBehaviour
{

    [SerializeField]
    public Toggle toggle;



     int segments = 64;

     float xRadius = 20;
     float yRadius = 20;
     LineRenderer[] line = new LineRenderer[100];
     GameObject[] games = new GameObject[100];
     int count = 0;
    


    

    void Start()
    {

        //toggle.onValueChanged.AddListener(delegate { ToggleGridline(); });
        while (count < 100)
        {
            games[count] = Instantiate(new GameObject());
            line[count] = games[count].AddComponent<LineRenderer>();

            line[count].SetWidth(.5f, .5f);
            line[count].SetVertexCount(segments + 1);
            line[count].useWorldSpace = false;
            line[count].SetColors(Color.black, Color.black);
            line[count].material.color = Color.white;

            CreatePoints();
            xRadius = 20 * count;
            yRadius = 20 * count;
            count++;
        }
    }


    void CreatePoints()
    {
        float x;
        float y;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xRadius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * yRadius;

            line[count].SetPosition(i, new Vector3(x, 2, y));

            angle += (360f / segments);
        }
    }

    public void ToggleGridline()
    {
        Debug.Log("Toggle works");
         
            for (int x = 0; x < 100; x++)
            {
                line[x].enabled = !line[x].enabled;
            }
        
        



    }


}