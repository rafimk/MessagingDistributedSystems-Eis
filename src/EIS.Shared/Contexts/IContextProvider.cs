namespace EIS.Shared.Contexts;

public interface IContextProvider
{
    IContext Current();
}