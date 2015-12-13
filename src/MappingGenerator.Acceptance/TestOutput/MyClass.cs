public class MyClass
{
    private Services.IMyDependency _myDependency;
    public MyClass(Services.IMyDependency myDependency)
    {
        _myDependency = myDependency;
    }
}

