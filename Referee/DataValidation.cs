namespace Referee
{
    public class DataValidation
    {
        public string JmenoRozhodciho { get; set; }
        public string PrijmeniRozhodciho { get; set; }
        public string ValidatingDate { get; set; }
        public string AdresaRozhodciho { get; set; }
        public string MestoRozhodciho { get; set; }

        public DataValidation()
        {
            JmenoRozhodciho = "";
            PrijmeniRozhodciho = "";
            ValidatingDate = "";
            AdresaRozhodciho = "";
            MestoRozhodciho = "";
        }
    }
}
