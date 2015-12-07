### A compile-time object mapping library
Object mapping is a tedious job, you have to set many properties of an object
in your own domain A using many properties' values of an object from an external domain B.
```csharp
public InternalObject Work()
{
  var externalObject = _service.GetExternalObject();
  var internalObject = new InternalObject
  {
    A = externalObject.A,
    B = externalObject.B,
    //...
    Z = externalObject.Z,
  }
}
```
*Mapping Generator* relieves you from the pain of writing such a dull code by generating
mapping code for you, while allowing you to customize it directly at the code that
_does_ the mapping.
