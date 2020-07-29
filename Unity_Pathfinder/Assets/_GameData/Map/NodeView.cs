using UnityEngine;

public class NodeView : MonoBehaviour
{
    //Darstellung der Nodes
    //show nodes
    public GameObject tile;

    //Randeigenschaften / ränder schmaler
    //border behavior
    [Range(0, 0.5f)]
    public float borderSize = 0.15f;

    //neues model view objekt
    //new model view object
    public void Init(Node node)
    {
        if (tile != null)
        {
            //jedem Node ein Tile geben
            //every node gets a tile
            gameObject.name = "Node (" + node.xIndex + "," + node.yIndex + ")";
            gameObject.transform.position = node.position;
            tile.transform.localScale = new Vector3(1f - borderSize, 1f, 1f - borderSize);
        }
    }

    //Nodes farben zuteilen
    //every node gets a color
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
    //Colorr Method
    public void ColorNode(Color color)
    {
        ColorNode(color, tile);
    }

    //GameObjects anzeigen
    //Show game object
    private void EnableObject(GameObject gobj, bool state)
    {
        if (gobj != null)
        {
            //GameObject aktivieren
            //activate game object
            gobj.SetActive(state);
        }
    }
}