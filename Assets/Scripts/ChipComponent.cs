using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Assets.Scripts
{
    public class ChipComponent : BaseClickComponent
    {
        public bool isDelete = false;
        public override void OnPointerEnter(PointerEventData eventData)
        {
            CallBackEvent((CellComponent)Pair, true);
            
            if (GetColor == ColorType.Black)
            {
                AddAdditionalMaterial(Resources.Load<Material>("Materials/BlackChipOnPointer"), 1);
            }else if (GetColor == ColorType.White)
            {
                AddAdditionalMaterial(Resources.Load<Material>("Materials/WhiteChipOnPointer"), 1);
            }
            transform.parent.gameObject.GetComponent<CellComponent>().OnPointerEnter(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            CallBackEvent((CellComponent)Pair, false);
            RemoveAdditionalMaterial(1);
        }

        public override void OnClickMe(Vector3 eventData)
        {
            CellComponent parent = transform.parent.gameObject.GetComponent<CellComponent>();
            CellsPool pool = parent.transform.parent.gameObject.GetComponent<CellsPool>();
            
            if((pool.IsWhiteStep == true) && (this.GetColor == ColorType.White) || (pool.IsWhiteStep == false) && (this.GetColor == ColorType.Black))
            {
                if (pool.SelectedChip != null)
                {
                    pool.SelectedChip.transform.parent.GetComponent<CellComponent>().NeighborSelect(false);
                }
                pool.SelectedChip = this;
                parent.OnClickMe(eventData);
                parent.NeighborSelect(true);
            }
            else
            {
                Debug.Log("Сейчас ходит другая команда!");
            }

            FindObjectOfType<GameManager>().Logger("Select " + this.name + " in " + parent.name);
        }

        public void MoveChipTo(Vector3 endPosition)
        {
            StartCoroutine(MoveChip(endPosition));
        }

        private IEnumerator MoveChip(Vector3 endPosition)
        {
            var currentTime = 0f;
            var time = 2f;
            while (currentTime < time)
            {
                transform.position = Vector3.Lerp(this.transform.position, endPosition, 1 - (time - currentTime) / time);
                currentTime += Time.deltaTime;
                yield return null;
            }
            transform.position = endPosition;
        }
    }
}
