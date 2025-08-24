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
        GameObject m_unitA;
        GameObject m_unitB;
        PhotonView photonView;
        public void SetValues(GameObject unitA, GameObject unitB)
        {
            m_unitA = unitA;
            m_unitB = unitB;
            photonView = GetComponent<PhotonView>();
        }

        public void CreateUnit(SpaceMark target) 
        {
            //target.Unit = PhotonNetwork.Instantiate("Piece", target.transform.position, Quaternion.identity);
            target.Unit = Instantiate(m_unitA, target.transform.position, Quaternion.identity);
            target.Unit.GetComponent<MeshRenderer>().material.color = Color.white;

            photonView.RPC("RPC_CreateUnit", RpcTarget.Others, target.Dimension.x, target.Dimension.y);
        }
        public void SelectUnit(SpaceMark current)
        {
            current.Unit.GetComponent<MeshRenderer>().material.color = Color.red;
            photonView.RPC("RPC_SelectUnit", RpcTarget.Others, current.Dimension.x, current.Dimension.y);
        }
        public void MoveUnit(SpaceMark current, SpaceMark target, bool CanInstantiate)
        {
            current.Unit.GetComponent<MeshRenderer>().material.color = Color.white;
            target.Unit = current.Take();
            target.SetPosition();

            photonView.RPC("RPC_MoveUnit", RpcTarget.Others,
                current.Dimension.x, current.Dimension.y,
                target.Dimension.x, target.Dimension.y, CanInstantiate);
        }
        public void SwapUnits(SpaceMark current, SpaceMark target, bool canInstantiate, bool currentInstantiate)
        {
            current.Unit.GetComponent<MeshRenderer>().material.color = Color.white;
            target.Unit.GetComponent<MeshRenderer>().material.color = Color.white;

            var unit1 = target.Take();
            var unit2 = current.Take();
            var previousCurrentMark = current;

            target.Unit = unit2;
            previousCurrentMark.Unit = unit1;

            target.SetPosition();
            previousCurrentMark.SetPosition();

            photonView.RPC("RPC_SwapUnits", RpcTarget.Others,
                current.Dimension.x, current.Dimension.y,
                target.Dimension.x, target.Dimension.y
                , canInstantiate, currentInstantiate);
        }

        void Activator(bool CanInstantiate, GameObject unit)
        {
            if (CanInstantiate) 
                unit.SetActive(false);
            else
                unit.SetActive(true);
        }

        [PunRPC]
        void RPC_CreateUnit(int dimX, int dimY)
        {
            Vector2Int originalDim = new Vector2Int(dimX, dimY);
            if (PlacementSystem.GridMarksDimension.ContainsKey(originalDim))
            {
                SpaceMark mirroredTarget = PlacementSystem.GridMarksDimension[originalDim].MirroredMark;
                mirroredTarget.Unit = Instantiate(m_unitB, mirroredTarget.transform.position, Quaternion.identity);
                mirroredTarget.Unit.GetComponent<MeshRenderer>().material.color = Color.white;
                Activator(true, mirroredTarget.Unit);
            }
        }

        [PunRPC]
        void RPC_SelectUnit(int dimX, int dimY)
        {
            Vector2Int originalDim = new Vector2Int(dimX, dimY);
            if (PlacementSystem.GridMarksDimension.ContainsKey(originalDim))
            {
                SpaceMark mirroredCurrent = PlacementSystem.GridMarksDimension[originalDim].MirroredMark;
                if (mirroredCurrent.Unit != null)
                    mirroredCurrent.Unit.GetComponent<MeshRenderer>().material.color = Color.blue;
            }
        }

        [PunRPC]
        void RPC_MoveUnit(int currentDimX, int currentDimY, int targetDimX, int targetDimY, bool CanInstantiate)
        {
            Vector2Int originalCurrentDim = new Vector2Int(currentDimX, currentDimY);
            Vector2Int originalTargetDim = new Vector2Int(targetDimX, targetDimY);

            if (PlacementSystem.GridMarksDimension.ContainsKey(originalCurrentDim) &&
                PlacementSystem.GridMarksDimension.ContainsKey(originalTargetDim))
            {
                SpaceMark mirroredCurrent = PlacementSystem.GridMarksDimension[originalCurrentDim].MirroredMark;
                SpaceMark mirroredTarget = PlacementSystem.GridMarksDimension[originalTargetDim].MirroredMark;

                if (mirroredCurrent.Unit != null)
                {
                    mirroredCurrent.Unit.GetComponent<MeshRenderer>().material.color = Color.white;
                    mirroredTarget.Unit = mirroredCurrent.Take();
                    mirroredTarget.SetPosition();
                }
                Activator(CanInstantiate, mirroredTarget.Unit);
            }
        }

        [PunRPC]
        void RPC_SwapUnits(int currentDimX, int currentDimY, int targetDimX, int targetDimY, bool CanInstantiate, bool currentInstantiate)
        {
            Vector2Int originalCurrentDim = new Vector2Int(currentDimX, currentDimY);
            Vector2Int originalTargetDim = new Vector2Int(targetDimX, targetDimY);

            if (PlacementSystem.GridMarksDimension.ContainsKey(originalCurrentDim) &&
                PlacementSystem.GridMarksDimension.ContainsKey(originalTargetDim))
            {
                SpaceMark mirroredCurrent = PlacementSystem.GridMarksDimension[originalCurrentDim].MirroredMark;
                SpaceMark mirroredTarget = PlacementSystem.GridMarksDimension[originalTargetDim].MirroredMark;

                if (mirroredCurrent.Unit != null && mirroredTarget.Unit != null)
                {
                    mirroredCurrent.Unit.GetComponent<MeshRenderer>().material.color = Color.white;
                    mirroredTarget.Unit.GetComponent<MeshRenderer>().material.color = Color.white;

                    var unit1 = mirroredTarget.Take();
                    var unit2 = mirroredCurrent.Take();
                    var previousCurrentMark = mirroredCurrent;

                    mirroredTarget.Unit = unit2;
                    previousCurrentMark.Unit = unit1;

                    mirroredTarget.SetPosition();
                    previousCurrentMark.SetPosition();

                    Activator(CanInstantiate, mirroredTarget.Unit);
                    Activator(currentInstantiate, previousCurrentMark.Unit);
                }
            }
        }
    }
}
