using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BoardGrid : MonoBehaviour
{

    public int width, height;

    public Cell CellPrefab;
    public Text CellLabelPrefab;
    public Cell RoadPrefab;
    public GameData gameData;
    public GameObject SelectPanel;
    
    public int CellSize;

    private Dictionary<Vector2,CellType> boardMap = new Dictionary<Vector2, CellType>();

    private List<Text> selectButtonLabels = new List<Text>();
    public TrackType SelectedTrackType { set { selectedTrackType =value; } get { return selectedTrackType; } }
    private TrackType selectedTrackType;
    List<Cell> cells;
    List<Text> cellLabels;
    Canvas canvas;
    private Cell selectedCellPreview;
    private float selectedRotation = 0.0f;

    private void Awake()
    {
        cells = new List<Cell>();
        cellLabels = new List<Text>();
        canvas = GetComponentInChildren<Canvas>();
        selectedTrackType = TrackType.None;
        var labels = SelectPanel.transform.GetComponentsInChildren<Text>();
        for(int i=0; i < labels.Length;i++)
        {
            selectButtonLabels.Add(labels[i]);
        }
        selectedCellPreview = Instantiate<Cell>(CellPrefab);
        selectedCellPreview.gameObject.SetActive(true);


        GenerateGrid();
    }


    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 pos;
            if ((pos = GetMousePosition()) != Vector2.negativeInfinity)
            {
                pos = new Vector2(RoundToNearest(pos.x, CellSize), RoundToNearest(pos.y, CellSize))/(float) CellSize; // convert position to grid index

                if(boardMap.ContainsKey(pos) && selectedTrackType != TrackType.None)
                {
                    if (boardMap[pos] == CellType.None)
                    {
                        Track track = gameData.tracks[ (int) selectedTrackType];
                        CreateRoad(pos.x, pos.y, track.Texture);
                        boardMap[pos] = CellType.Road;
                    }
                }
                
                
            }
           
        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 pos;
            if ((pos = GetMousePosition()) != Vector2.negativeInfinity)
            {
                pos = new Vector2(RoundToNearest(pos.x, CellSize), RoundToNearest(pos.y, CellSize)) / (float)CellSize;

                if (boardMap.ContainsKey(pos))
                {
                    if (boardMap[pos] == CellType.Road)
                    {
                        int cellID = FindCellID(pos);
                        if (cellID != -1)
                        {
                            boardMap[pos] = CellType.None;
                            RemoveRoad(cells[cellID]);
                        }
                        
                    }
                }
            }
        }
        if (selectedTrackType != TrackType.None)
        {
           
            Vector2 pos;
            if ((pos = GetMousePosition()) != Vector2.negativeInfinity)
            {
                pos = new Vector2(RoundToNearest(pos.x, CellSize), RoundToNearest(pos.y, CellSize));
                if (boardMap.ContainsKey(pos /(float) CellSize))
                {
                    selectedCellPreview.gameObject.SetActive(true);
                }
                else
                {
                    selectedCellPreview.gameObject.SetActive(false);
                }


               
                selectedCellPreview.transform.localPosition = new Vector3(pos.x, pos.y);
                selectedCellPreview.SetTexture(gameData.tracks[(int)selectedTrackType].Texture);
              

            }

        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            selectedRotation += 90.0f;
            selectedRotation = selectedRotation > 360.0f ? selectedRotation - 360.0f : selectedRotation;
            selectedCellPreview.transform.localRotation = Quaternion.AngleAxis(selectedRotation, new Vector3(0.0f,0.0f,1.0f));
            
        }

        }

    public void SelectTrackType(int typeName)
    {
        selectedTrackType =(TrackType) typeName;
    }

    private int FindCellID(Vector2 position)
    {
        int result = -1;
        foreach(var cell in cells)
        {
            if (cell.posX == position.x && cell.posY == position.y)
            {
                result = cells.IndexOf(cell);
                break;
            }
        }
        return result;

    }

    private int RoundToNearest(float number,int scale)
    {
        int a = ((int)(number / scale)) * scale;
        int b = a + scale;
        return (number - a >  b - number) ? b : a;
    }

    private Vector2 GetMousePosition()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit))
        {
            return hit.point;
        }

        return Vector2.negativeInfinity;
    }

    public void GenerateGrid()
    {
        for(int y=0; y < height;y++)
            for(int x=0; x < width;x++)
            {
                boardMap.Add(new Vector2(x, y), CellType.None);
            }
     
        for(int i=0;i < gameData.destinations.Count; i++)
        {
            boardMap[gameData.destinations[i].Position] = CellType.EndStop;
            CreateCell(gameData.destinations[i].Position.x, gameData.destinations[i].Position.y, gameData.destinations[i].Name);
        }

               
          

    }
    public void CreateRoad(float x,float y,Texture texture)
    {
        var instance = Instantiate<Cell>(RoadPrefab);
        instance.transform.localPosition = new Vector3(x*CellSize , y* CellSize, 0);
        instance.transform.localRotation = selectedCellPreview.transform.localRotation;
        instance.transform.SetParent(transform);
        instance.SetTexture( texture);
        instance.posX = (int) x;
        instance.posY =(int) y;

        cells.Add(instance);
    }

    public void RemoveRoad(Cell roadCell)
    {
        cells.Remove(roadCell);
        Destroy(roadCell.gameObject);
    }

    public void CreateCell(float x,float y, string text)
    {
        var instance = Instantiate<Cell>(CellPrefab);
        instance.transform.localPosition = new Vector3(x* CellSize, y* CellSize, 0);
        instance.transform.SetParent(transform);
        instance.posX = (int)x;
        instance.posY = (int)y;
        cells.Add(instance);

        var label = Instantiate<Text>(CellLabelPrefab);
        label.rectTransform.SetParent(canvas.transform,false);
        label.rectTransform.anchoredPosition = new Vector2(x * CellSize, y * CellSize);
        label.text = text;
    }
}
