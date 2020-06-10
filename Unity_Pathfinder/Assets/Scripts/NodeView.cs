using UnityEngine;

public class NodeView : MonoBehaviour
{
    //Darstellung der Nodes
    public GameObject tile;
    public GameObject arrow;

    //Randeigenschaften / ränder schmaler
    [Range(0, 0.5f)]
    public float borderSize = 0.15f;

    //Pfeile und Richtung der Suche
    Node m_node;

    //neues model view objekt
    public void Init(Node node)
    {
        if (tile != null)
        {
            //jedem Node ein Tile geben
            gameObject.name = "Node (" + node.xIndex + "," + node.yIndex + ")";
            gameObject.transform.position = node.position;
            tile.transform.localScale = new Vector3(1f - borderSize, 1f, 1f - borderSize);
            m_node = node;
            EnableObject(arrow, false);
        }
    }

    //Nodes farben zuteilen
    private void ColorNode(Color color, GameObject gobj)
    {
        if (gobj != null)
        {
            Renderer gobjRenderer = gobj.GetComponent<Renderer>();

            if (gobjRenderer != null)
            {
                gobjRenderer.material.color = color;
            }
        }
    }

    //überladene Farbmethode
    public void ColorNode(Color color)
    {
        ColorNode(color, tile);
    }

    //GameObjects anzeigen
    void EnableObject(GameObject gobj, bool state)
    {
        if (gobj != null)
        {
            //GameObject aktivieren
            gobj.SetActive(state);
        }
    }

    //Pfeile anzeigen
    public void ShowArrow()
    {
        //Crash durch fehlende Werte vermeiden
        if (m_node != null && arrow != null && m_node.previous != null)
        {
            //Gameobjekte anzeigen aufrufen
            EnableObject(arrow, true);

            //Rotieren der Pfeile in die richtige Richtung
            //info: durch vector3 node1(x1, y1, z1); node2(x2, y2, z2); differenziert(x2-x1, y2-y2, z2-z1)
            //https://docs.unity3d.com/ScriptReference/Quaternion.html
            Vector3 dirToPrevious = (m_node.previous.position - m_node.position).normalized;
            arrow.transform.rotation = Quaternion.LookRotation(dirToPrevious);
        }
    }
}