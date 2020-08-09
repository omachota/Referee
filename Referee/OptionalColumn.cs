namespace Referee
{
    public class OptionalColumn
    {
        public string Header { get; }
        public string Binding { get; }
        public bool IsSelected { get; }

        public OptionalColumn(string header, string binding, bool isSelected)
        {
            Header = header;
            Binding = binding;
            IsSelected = isSelected;
        }
    }
}
