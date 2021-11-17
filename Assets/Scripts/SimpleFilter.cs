using UnityEngine;

public class SimpleFilter : MonoBehaviour
{
    [SerializeField] private Shader _shader;

    private Material _material;
    private bool _useFilter;

    private void Awake()
    {
        _material = new Material(_shader);
        _useFilter = false;
    }

    public void UseFilter()
    {
        _useFilter = true;
    }
    
    public void DontUseFilter()
    {
        _useFilter = false;
    }
    
    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (_useFilter)
        {
            UseFilter(src, dst);
        }
        else
        {
            Graphics.Blit(src, dst);
        }
    }
    
    protected virtual void UseFilter(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, _material);
    }
}