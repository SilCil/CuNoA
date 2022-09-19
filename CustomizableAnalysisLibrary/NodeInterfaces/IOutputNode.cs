namespace CustomizableAnalysisLibrary
{
    public interface IOutputNode
    {
        void SetComments(params string[] comments);
        void Output(string path, Table result);
    }
}
