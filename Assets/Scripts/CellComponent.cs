using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Assets.Scripts
{
    public class CellComponent : BaseClickComponent
    {
        private Dictionary<NeighborType, CellComponent> _neighbors = new Dictionary<NeighborType, CellComponent>();

        ChipComponent _child = null;

        public bool isNeighbor = false;

        /// <summary>
        /// Возвращает соседа клетки по указанному направлению
        /// </summary>
        /// <param name="type">Перечисление направления</param>
        /// <returns>Клетка-сосед или null</returns>
        public CellComponent GetNeighbors(NeighborType type) => _neighbors[type];

        public void SetNeighbors()
        {
            var _cells = transform.parent.gameObject.GetComponent<CellsPool>().CellsInTable;
            foreach (GameObject neighbor in _cells)
            {
                float myX = transform.position.x;           //X координата ячейки
                float nX = neighbor.transform.position.x;   //X координата возможного соседа
                float myZ = transform.position.z;           //Z координата ячейки
                float nZ = neighbor.transform.position.z;   //Z координата возможного соседа
                if ((myX - 1 == nX) && (myZ + 1 == nZ))
                {
                    _neighbors.Add(NeighborType.TopLeft, neighbor.GetComponent<CellComponent>());
                }
                else if ((myX + 1 == nX) && (myZ + 1 == nZ))
                {
                    _neighbors.Add(NeighborType.TopRight, neighbor.GetComponent<CellComponent>());
                }
                else if ((myX - 1 == nX) && (myZ - 1 == nZ))
                {
                    _neighbors.Add(NeighborType.BottomLeft, neighbor.GetComponent<CellComponent>());
                }
                else if ((myX + 1 == nX) && (myZ - 1 == nZ))
                {
                    _neighbors.Add(NeighborType.BottomRight, neighbor.GetComponent<CellComponent>());
                }
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            CallBackEvent(this, true);
            
            if (GetColor == ColorType.Black)
            {
                AddAdditionalMaterial(Resources.Load<Material>("Materials/CellBlackOnPointer"), 1);
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            CallBackEvent(this, false);
            if (GetColor == ColorType.Black)
            {
                RemoveAdditionalMaterial(1);
            }
        }

        public override void OnClickMe(Vector3 eventData)
        {
            ChipComponent _selectedChip = transform.parent.gameObject.GetComponent<CellsPool>().SelectedChip;
            
            
            if (_selectedChip != null)
            {
                if (this.transform.childCount > 0)
                {
                    _child = this.transform.GetChild(0).GetComponent<ChipComponent>();
                    _selectedChip.transform.parent.GetComponent<CellComponent>().NeighborSelect(false);
                    transform.parent.gameObject.GetComponent<CellsPool>().SelectedChip = _child;
                }
                else if (isNeighbor)
                {
                    _selectedChip.GetComponent<ChipComponent>().MoveChipTo(eventData);
                    CheckChipDelete();
                    
                    _selectedChip.transform.parent.GetComponent<CellComponent>().NeighborSelect(false);
                    _selectedChip.transform.SetParent(this.transform);
                    
                    CheckGame(_selectedChip);
                    ReverseStep();

                    FindObjectOfType<GameManager>().Logger("Move " + _selectedChip.name + " on " + this.name);

                    transform.parent.gameObject.GetComponent<CellsPool>().SelectedChip = null;
                }
                else
                {
                    Debug.Log("Nooo");
                }
            }
        }

        /// <summary>
        /// Конфигурирование связей клеток
        /// </summary>
		public void Configuration(Dictionary<NeighborType, CellComponent> neighbors)
        {
            if (_neighbors != null) return;
            _neighbors = neighbors;
        }

        public void NeighborSelect(bool select)
        {
            ChipComponent selectChip = transform.parent.gameObject.GetComponent<CellsPool>().SelectedChip;
            
            foreach (KeyValuePair<NeighborType, CellComponent> neighbor in _neighbors)
            {
                if (((selectChip.GetColor == ColorType.White) && ((neighbor.Key == NeighborType.TopLeft) || (neighbor.Key == NeighborType.TopRight))) || ((selectChip.GetColor == ColorType.Black) && ((neighbor.Key == NeighborType.BottomLeft) || (neighbor.Key == NeighborType.BottomRight))))
                {
                    if (neighbor.Value.transform.childCount == 0)
                    {
                        if (select)
                        {
                            neighbor.Value.isNeighbor = true;
                            neighbor.Value.AddAdditionalMaterial(Resources.Load<Material>("Materials/CellBlackSelected"), 2);
                        }
                        else
                        {
                            neighbor.Value.isNeighbor = false;
                            neighbor.Value.RemoveAdditionalMaterial(2);
                        }
                    }
                    else
                    {
                        BadNeighborSelect(neighbor, select);
                    }
                }
            }
        }

        public void BadNeighborSelect(KeyValuePair<NeighborType, CellComponent> neighbor, bool select)
        {
            ChipComponent child = neighbor.Value.transform.GetChild(0).GetComponent<ChipComponent>();
            ChipComponent selectChip = transform.parent.gameObject.GetComponent<CellsPool>().SelectedChip;
            
            if (child.GetColor != selectChip.GetColor)
            {
                
                CellComponent badNeightbors = neighbor.Value.GetComponent<CellComponent>();
                foreach (KeyValuePair<NeighborType, CellComponent> badNeightbor in badNeightbors._neighbors)
                {
                    if (badNeightbor.Key == neighbor.Key && (badNeightbor.Value.transform.childCount == 0))
                    {
                        if (select)
                        {
                            badNeightbor.Value.isNeighbor = true;
                            badNeightbor.Value.AddAdditionalMaterial(Resources.Load<Material>("Materials/CellBlackSelected"), 2);
                            child.isDelete = true;
                        }
                        else
                        {
                            badNeightbor.Value.isNeighbor = false;
                            badNeightbor.Value.RemoveAdditionalMaterial(2);
                            child.isDelete = false;
                        }
                    }
                }
            }
        }

        public void CheckChipDelete()
        {
            foreach (KeyValuePair<NeighborType, CellComponent> neighbor in _neighbors)
            {
                if (neighbor.Value.transform.childCount > 0)
                {
                    if (neighbor.Value.transform.GetChild(0).GetComponent<ChipComponent>().isDelete)
                    {
                        GameObject child = neighbor.Value.transform.GetChild(0).transform.gameObject;
                        child.SetActive(false);
                        if (child.GetComponent<ChipComponent>().GetColor == ColorType.White)
                        {
                            neighbor.Value.transform.parent.GetComponent<CellsPool>().WhiteCount -= 1;
                        }
                        else if (child.GetComponent<ChipComponent>().GetColor == ColorType.Black)
                        {
                            neighbor.Value.transform.parent.GetComponent<CellsPool>().BlackCount -= 1;
                        }
                        neighbor.Value.transform.GetChild(0).transform.SetParent(FindObjectOfType<ChipsPool>().transform);
                        this.RemoveAdditionalMaterial(2);

                        FindObjectOfType<GameManager>().Logger("Destroy " + child.name + " on " + neighbor.Value.name);
                    }
                }
            }
        }

        public void ReverseStep()
        {
            if (!FindObjectOfType<CellsPool>().TheEnd)
            {
                FindObjectOfType<CellsPool>().LockedGame(true);
                CellsPool pool = FindObjectOfType<CellsPool>();
                pool.IsWhiteStep = !pool.IsWhiteStep;
                FindObjectOfType<CameraController>().RotateCamera();
            }
        }

        public void CheckGame(ChipComponent selectedChip)
        {
            CellsPool pool = FindObjectOfType<CellsPool>();
            if((pool.WhiteCount < 1) || (pool.BlackCount < 1) || (((selectedChip.GetColor == ColorType.White) && (selectedChip.transform.parent.transform.position.z >= 7)) || ((selectedChip.GetColor == ColorType.Black) && (selectedChip.transform.parent.transform.position.z <= 0))))
            {
                pool.EndGame(selectedChip.GetColor.ToString());
            }
        }
    }

    /// <summary>
    /// Тип соседа клетки
    /// </summary>
    public enum NeighborType : byte
    {
        /// <summary>
        /// Клетка сверху и слева от данной
        /// </summary>
        TopLeft,
        /// <summary>
        /// Клетка сверху и справа от данной
        /// </summary>
        TopRight,
        /// <summary>
        /// Клетка снизу и слева от данной
        /// </summary>
        BottomLeft,
        /// <summary>
        /// Клетка снизу и справа от данной
        /// </summary>
        BottomRight
    }
}