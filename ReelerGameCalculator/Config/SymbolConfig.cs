namespace Calculator2Henry.Mocking
{
    public class SymbolConfig
    {
        public int Id { get; set; }
        public string Name { get; set; } = "NA";

        public bool IsWild { get; set; } = false;
        public bool IsWildSubstitutable { get; set; } = false;

        public bool IsScatter { get; set; } = false;
    }
}
