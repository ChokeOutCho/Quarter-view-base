using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Define.CameraMode _mode = Define.CameraMode.QuarterView;

    [SerializeField]
    Vector3 _delta = new Vector3(0.0f, 10.0f, -6.0f);

    [SerializeField]
    GameObject _player = null;

    MeshRenderer _obstacleRenderer;
    
    public void SetPlayer(GameObject player) { _player = player; }
    void Start()
    {
        
    }

    void LateUpdate()
    { 
        if (_mode == Define.CameraMode.QuarterView)
        {
            if (_player.IsValid() == false)
                return;

            RaycastHit hit;
            transform.position = _player.transform.position + _delta;
				transform.LookAt(_player.transform);

            
            if (Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, LayerMask.GetMask("Block")))
            {
                _obstacleRenderer = hit.collider.GetComponentInChildren<MeshRenderer>();
                if(_obstacleRenderer!=null)
                {
                    Material Mat = _obstacleRenderer.material;
                    Color matColor = Mat.color;
                    matColor.a = 0.3f;
                    Mat.color = matColor;
                    
                }
            }
           else
            {
                if (_obstacleRenderer != null){
                    Material Mat = _obstacleRenderer.material;
                    Color matColor = Mat.color;
                    matColor.a = 1.0f;
                    Mat.color = matColor;
                    _obstacleRenderer = null;
                }
                
            }

            
		}
    }

    public void SetQuarterView(Vector3 delta)
    {
        _mode = Define.CameraMode.QuarterView;
        _delta = delta;
    }

}
