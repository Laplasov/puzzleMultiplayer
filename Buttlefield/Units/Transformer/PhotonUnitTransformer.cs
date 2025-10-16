using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
namespace Assets.Scripts.Units
{
    public class PhotonUnitTransformer : MonoBehaviour, IUnitTransformer
    {
        UnitPrefabsSO m_units;
        PhotonView photonView;

        GameObject[] m_characterHolder;
        public void SetValues(UnitPrefabsSO units)
        {
            m_units = units;
            photonView = GetComponent<PhotonView>();
        }

        public GameObject CreateUnit(SpaceMark target, string name) 
        {
            var unit = m_units.GetPrefabByName(name);

            target.Unit = Instantiate(unit, target.transform.position, Quaternion.identity);
            target.Unit.GetComponent<MeshRenderer>().material.color = Color.white;

            photonView.RPC("RPC_CreateUnit", RpcTarget.Others, target.Dimension.x, target.Dimension.y, name);
            return target.Unit;
        }
        public void SelectUnit(SpaceMark current, PlacementSystem board)
        {
            current.Unit.GetComponent<MeshRenderer>().material.color = Color.red;
            photonView.RPC("RPC_SelectUnit", RpcTarget.Others, current.Dimension.x, current.Dimension.y, board.CanInstantiate());
        }
        public void MoveUnit(SpaceMark current, SpaceMark target, PlacementSystem board, PlacementSystem currentBoard)
        {
            current.Unit.GetComponent<MeshRenderer>().material.color = Color.white;
            target.Unit = current.Take(target);
            target.SetPosition();

            photonView.RPC("RPC_MoveUnit", RpcTarget.Others,
                current.Dimension.x, current.Dimension.y,
                target.Dimension.x, target.Dimension.y,
                board.CanInstantiate(), currentBoard.CanInstantiate());
        }
        public void SwapUnits(SpaceMark current, SpaceMark target, PlacementSystem board, PlacementSystem currentBoard)
        {
            current.Unit.GetComponent<MeshRenderer>().material.color = Color.white;
            target.Unit.GetComponent<MeshRenderer>().material.color = Color.white;

            var unit1 = target.Take(current);
            var unit2 = current.Take(target);
            var previousCurrentMark = current;

            target.Unit = unit2;
            previousCurrentMark.Unit = unit1;

            target.SetPosition();
            previousCurrentMark.SetPosition();

            photonView.RPC("RPC_SwapUnits", RpcTarget.Others,
                current.Dimension.x, current.Dimension.y,
                target.Dimension.x, target.Dimension.y,
                board.CanInstantiate(), currentBoard.CanInstantiate());
        }

        void Activator(bool CanInstantiate, SpaceMark mark)
        {
            if (CanInstantiate)
                mark.Unit.SetActive(false);
            else
                mark.Unit.SetActive(true);
        }

        [PunRPC]
        void RPC_CreateUnit(int dimX, int dimY, string name)
        {
            Vector2Int originalDim = new Vector2Int(dimX, dimY);
            SpaceMark mirroredTarget = GridRegistry.Instance.GetGrid(PlacementType.UnitHolder)[originalDim].MirroredMark;

            var unit = m_units.GetPrefabByName(name);

            mirroredTarget.Unit = Instantiate(unit, mirroredTarget.transform.position, Quaternion.identity);
            mirroredTarget.Unit.GetComponent<MeshRenderer>().material.color = Color.white;
            Activator(true, mirroredTarget);
        }

        [PunRPC]
        void RPC_SelectUnit(int dimX, int dimY, bool CanInstantiate)
        {
            PlacementType type = CanInstantiate ? PlacementType.UnitHolder : PlacementType.Battlefield;
            Vector2Int originalDim = new Vector2Int(dimX, dimY);
            SpaceMark mirroredCurrent = GridRegistry.Instance.GetGrid(type)[originalDim].MirroredMark;
            if (mirroredCurrent.Unit != null)
                mirroredCurrent.Unit.GetComponent<MeshRenderer>().material.color = Color.blue;
        }

        [PunRPC]
        void RPC_MoveUnit(int currentDimX, int currentDimY, int targetDimX, int targetDimY, bool CanInstantiate, bool currentInstantiate)
        {
            Vector2Int originalCurrentDim = new Vector2Int(currentDimX, currentDimY);
            Vector2Int originalTargetDim = new Vector2Int(targetDimX, targetDimY);

            PlacementType typeNext = CanInstantiate ? PlacementType.UnitHolder : PlacementType.Battlefield;
            PlacementType typeCurrent = currentInstantiate ? PlacementType.UnitHolder : PlacementType.Battlefield;

            SpaceMark mirroredCurrent = GridRegistry.Instance.GetGrid(typeCurrent)[originalCurrentDim].MirroredMark;
            SpaceMark mirroredTarget = GridRegistry.Instance.GetGrid(typeNext)[originalTargetDim].MirroredMark;

            if (mirroredCurrent.Unit != null)
            {
                mirroredCurrent.Unit.GetComponent<MeshRenderer>().material.color = Color.white;
                mirroredTarget.Unit = mirroredCurrent.Take(mirroredTarget);
                mirroredTarget.SetPosition();
            }
            Activator(CanInstantiate, mirroredTarget);
        }

        [PunRPC]
        void RPC_SwapUnits(int currentDimX, int currentDimY, int targetDimX, int targetDimY, bool CanInstantiate, bool currentInstantiate)
        {
            Vector2Int originalCurrentDim = new Vector2Int(currentDimX, currentDimY);
            Vector2Int originalTargetDim = new Vector2Int(targetDimX, targetDimY);

            PlacementType typeNext = CanInstantiate ? PlacementType.UnitHolder : PlacementType.Battlefield;
            PlacementType typeCurrent = currentInstantiate ? PlacementType.UnitHolder : PlacementType.Battlefield;

            SpaceMark mirroredCurrent = GridRegistry.Instance.GetGrid(typeCurrent)[originalCurrentDim].MirroredMark;
            SpaceMark mirroredTarget = GridRegistry.Instance.GetGrid(typeNext)[originalTargetDim].MirroredMark;

            if (mirroredCurrent.Unit != null && mirroredTarget.Unit != null)
            {
                mirroredCurrent.Unit.GetComponent<MeshRenderer>().material.color = Color.white;
                mirroredTarget.Unit.GetComponent<MeshRenderer>().material.color = Color.white;

                var unit1 = mirroredTarget.Take(mirroredCurrent);
                var unit2 = mirroredCurrent.Take(mirroredTarget);
                var previousCurrentMark = mirroredCurrent;

                mirroredTarget.Unit = unit2;
                previousCurrentMark.Unit = unit1;

                mirroredTarget.SetPosition();
                previousCurrentMark.SetPosition();

                Activator(CanInstantiate, mirroredTarget);
                Activator(currentInstantiate, previousCurrentMark);
            }
        }
    }
}
