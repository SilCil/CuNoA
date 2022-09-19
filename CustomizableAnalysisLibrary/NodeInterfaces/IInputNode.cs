namespace CustomizableAnalysisLibrary
{
    public interface IInputNode
    {
        InputType InputType { get; }
        Table Load(string path);
    }
}
