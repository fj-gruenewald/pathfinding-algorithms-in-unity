using UnityEngine;

public class NodeView : MonoBehaviour
{
    //Darstellung der Nodes
    public GameObject tile;

    //Randeigenschaften / ränder schmaler
    [Range(0, 0.5f)]
    public float borderSize = 0.15f;

    //neues model view objekt
    public void Init(Node node)
    {
        if (tile != null)
        {
            //jedem Node ein Tile geben
            gameObject.name = "Node (" + node.xIndex + "," + node.yIndex + ")";
            gameObject.transform.position = node.position;
            tile.transform.localScale = new Vector3(1f - borderSize, 1f, 1f - borderSize);
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

    //üverladene Farbmethode
    public void ColorNode(Color color)
    {
        ColorNode(color, tile);
    }
}