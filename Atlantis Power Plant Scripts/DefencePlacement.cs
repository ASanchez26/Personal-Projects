using UnityEngine;
using System.Collections;

public class DefencePlacement : MonoBehaviour
{

    public Transform Turret;

    private Transform spawn;
    //private Vector3 spwnpnt;

    [SerializeField]
    private Rect rect;

    void Update()
    {


        if (Input.GetMouseButton(0) && spawn != null)
        {
            var pos = Input.mousePosition;
            pos.z = -Camera.main.transform.position.z;
            spawn.transform.position = Camera.main.ScreenToWorldPoint(pos);
        }

        if (Input.GetMouseButtonUp(0))
        {
            spawn = null;
        }
    }

    void OnGUI()
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
        {
            var pos = Input.mousePosition;
            pos.z = -Camera.main.transform.position.z;
            pos = Camera.main.ScreenToWorldPoint(pos);
            spawn = Instantiate(Turret, pos, Quaternion.identity) as Transform;
        }

        GUI.Button(rect, "Turret 1");
    }
}
