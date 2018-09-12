using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CanvasRendererSetting : MonoBehaviour
{
    private Renderer m_sourceRenderer = null;
    private CanvasRenderer m_canvasRenderer = null;

    void ResetData()
    {
        if (m_canvasRenderer == null)
        {
            m_canvasRenderer = GetComponent<CanvasRenderer>();
        }

        if (m_canvasRenderer != null)
        {
            if (m_sourceRenderer == null)
            {
                m_sourceRenderer = GetComponent<Renderer>();
            }

            if (m_sourceRenderer != null)
            {
                Mesh meshData = null;
                if (m_sourceRenderer is SkinnedMeshRenderer)
                {
                    meshData = (m_sourceRenderer as SkinnedMeshRenderer).sharedMesh;
                }
                else if (m_sourceRenderer is MeshRenderer)
                {
                    MeshFilter meshFilter = GetComponent<MeshFilter>();
                    if (meshFilter != null)
                    {
                        meshData = meshFilter.sharedMesh;
                    }
                }

                if (meshData != null)
                {
                    m_canvasRenderer.SetMesh(meshData);

                    m_canvasRenderer.materialCount = m_sourceRenderer.sharedMaterials.Length;
                    for (int i = 0; i < m_canvasRenderer.materialCount; i++)
                    {
                        m_canvasRenderer.SetMaterial(m_sourceRenderer.sharedMaterials[i], i);
                    }
                }
            }
        }
    }

    void OnEnable()
    {
        ResetData();
    }

    void OnValidate()
    {
        ResetData();
    }
}
