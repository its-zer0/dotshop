using System;
using System.Runtime.Serialization;


namespace DotShop.Exceptions
{
  [Serializable]
  public abstract class AppException : Exception
  {
    protected AppException() { }
    protected AppException(string message) : base(message) { }
    protected AppException(string message, Exception inner) : base(message, inner) { }
    protected AppException(SerializationInfo info, StreamingContext context) : base(info, context) { }
  }
  [Serializable]
  public class DataAccessException : AppException
  {
    public DataAccessException() { }
    public DataAccessException(string message) : base(message) { }
    public DataAccessException(string message, Exception inner) : base(message, inner) { }
    protected DataAccessException(SerializationInfo info, StreamingContext context) : base(info, context) { }
  }
  [Serializable]
  public class BusinessException : AppException
  {
    public BusinessException() { }
    public BusinessException(string message) : base(message) { }
    public BusinessException(string message, Exception inner) : base(message, inner) { }
    protected BusinessException(SerializationInfo info, StreamingContext context) : base(info, context) { }
  }

  [Serializable]
  public class ProductNotFoundException : AppException
  {
    public Guid ProductId { get; }
    public ProductNotFoundException(Guid productId)
        : base($"Product with id {productId} not found.")
    {
      ProductId = productId;
    }
    public ProductNotFoundException(Guid productId, Exception inner) : base($"Product with id {productId} not found.", inner)
    {
      ProductId = productId;
    }
    protected ProductNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
      ProductId = Guid.Parse(ProductId.ToString());
    }



    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue(nameof(ProductId), ProductId);
    }

  }

  [Serializable]
  public class OrderNotFoundException : AppException
  {
    public Guid OrderId { get; }
    public OrderNotFoundException(Guid orderId) : base($"Order with ID {orderId} not found.")
    {
      OrderId = orderId;
    }
    public OrderNotFoundException(Guid orderId, Exception ex) : base($"Order with ID {orderId} not found.", ex)
    {
      OrderId = orderId;
    }
    protected OrderNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
      OrderId = (Guid)info.GetValue(nameof(OrderId), typeof(Guid));
    }
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue(nameof(OrderId), OrderId, typeof(Guid));
    }


  }
}