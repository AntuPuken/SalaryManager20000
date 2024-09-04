using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public interface IPaginableElementData { }

public interface IPaginableElement
{
    Transform transform { get; }
    GameObject gameObject { get; }
    void SetData(IPaginableElementData data);
}

public abstract class PaginableElement<T> : MonoBehaviour, IPaginableElement where T : IPaginableElementData
{
    public T data { get; protected set; }

    public void SetData(T data)
    {
        this.data = data;
        OnDataUpdated(data);
    }
    public void SetData(IPaginableElementData data)
    {
        if (data is T typedData)
        {
            SetData(typedData);
        }
        else
        {
            throw new System.Exception($"Cannot set data of type {data.GetType()} to element of type {typeof(T)}");
        }
    }

    // 
    /// <summary>
    /// Called when the data of the element is updated.
    /// </summary>
    protected virtual void OnDataUpdated(T data)
    {
    }

}
