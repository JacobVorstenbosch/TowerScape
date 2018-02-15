using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour {

    public Transform InventoryContainer;
    public Transform InventoryRoot;
    public float itemCellWidth = 50.0f;
    public float verticalOffset;
    private int inventoryCols = 5;
    private int inventoryRows = 3;
    private float colOffset = 5;
    private float rowOffset = 5;

    private int numItemsInBag;
    private int bagSize;

    public GameObject inventoryCellPrefab;

	// Use this for initialization
	void Start ()
    {
        float finalWidth = (inventoryCols - 1) * colOffset + inventoryCols * itemCellWidth;
        float finalHalfWidth = finalWidth / 2 - itemCellWidth / 2;
        //print(finalWidth);
        //float finalHeight = (inventoryRows - 1) * rowOffset + inventoryRows * itemCellWidth;
        //print(finalHeight);

        for (int r = 0; r < inventoryRows; r++)
            for (int c = 0; c < inventoryCols; c++)
            {
                Vector3 pos = new Vector3(-finalHalfWidth + c * colOffset + c * itemCellWidth, verticalOffset - (r * -rowOffset - r * itemCellWidth) + itemCellWidth / 2, 0);
                GameObject n = Instantiate(inventoryCellPrefab, pos, new Quaternion(), InventoryRoot);
                n.transform.localPosition = pos;
            }

        numItemsInBag = 0;
        bagSize = inventoryCols * inventoryRows;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Adds an item to the bag
    /// </summary>
    /// <returns>index that the item was added to, -1 if inventory full</returns>
    public int AddItemToBag(Item i)
    {
        if (numItemsInBag >= bagSize)
            return -1;
        //add the item to the bag
   
        Transform itemCell = CellByIndex(numItemsInBag).transform;
        //set border colour
        itemCell.GetChild(0).GetComponent<UnityEngine.UI.RawImage>().color = i.borderCol;
        itemCell.GetChild(1).gameObject.SetActive(false);
        itemCell.GetChild(2).gameObject.SetActive(true);


        //item added, return numberofitems aka this items index, then increment
        return numItemsInBag++;
    }

    public int GetNumberItemsInBag()
    {
        return numItemsInBag;
    }

    public GameObject CellByXY(int x, int y)
    {
        GameObject ret = null;

        int childNumber = 1;

        if (x > inventoryCols)
            return ret;
        else
            childNumber += x;

        if (y > inventoryRows)
            return ret;
        else
            childNumber += (inventoryRows - 1 - y) * inventoryCols;

        ret = InventoryRoot.GetChild(childNumber).gameObject;
        return ret;
    }

    public GameObject CellByIndex(int index)
    {
        return InventoryRoot.GetChild(index + 1).gameObject;
    }
}
