namespace ResxConverter.Core
{
    public interface IResxConverterOutputFactory
    {
        IResxConverterOutput Create(string culture, string outputFolder);
    }
}
