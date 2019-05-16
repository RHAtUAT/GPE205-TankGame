using UnityEngine;

//TODO: Use ticks of current date to avoid repeating maps of the day
//TODO: Refine Seed Generation

public class MapGenerator : MonoBehaviour
{

    public int rows;
    public int columns;
    public float roomWidth;
    public float roomHeight;
    public GameObject[] gridPrefabs;

    private Room[,] grid;

    public GameObject RandomRoomPrefab()
    {
        return gridPrefabs[Random.Range(0, gridPrefabs.Length)];
    }

    public void GenerateGrid()
    {
        grid = new Room[columns, rows];

        //Set the random functions seed
        Random.seed = GameManager.instance.mapSeed;


        for (int i = 0; i < rows; i++)
        {
            //for each column in that row
            for (int j = 0; j < columns; j++)
            {
                //Figure out the location 
                float xPosition = roomWidth * j;
                float zPosition = roomHeight * i;
                Vector3 newPosition = new Vector3(xPosition, 0.0f, zPosition);

                //Create a new grid at the appropropriate location
                GameObject tempRoomObject = Instantiate<GameObject>(RandomRoomPrefab(), newPosition, Quaternion.identity);

                //Set its parent
                tempRoomObject.transform.parent = this.transform;

                //Give it a location name
                tempRoomObject.name = "Room(" + j + ", " + i + ")";

                //Get the room object
                Room tempRoom = tempRoomObject.GetComponent<Room>();

                OpenDoors(tempRoom, i, j);

                //Save it to the grid array
                grid[j, i] = tempRoom;
            }
        }
    }

    void OpenDoors(Room room, int i, int j)
    {
        //Open the north and south doors
        //If we are on the bottom row, open the north door
        if (i == 0)
            room.doorNorth.SetActive(false);
        //If we are on the top row, open the north door
        else if (i == rows - 1)
            room.doorSouth.SetActive(false);
        else
        {
            //Otherwise, we are in the middle, so open both doors
            room.doorNorth.SetActive(false);
            room.doorSouth.SetActive(false);
        }

        //Open the east and west doors
        if (j == 0)
            room.doorEast.SetActive(false);
        //If we are in the last column, open the west door
        else if (j == columns - 1)
            room.doorWest.SetActive(false);
        else
        {
            //Otherwise, we are in the middle, so open both doors
            room.doorEast.SetActive(false);
            room.doorWest.SetActive(false);
        }
    }

}
