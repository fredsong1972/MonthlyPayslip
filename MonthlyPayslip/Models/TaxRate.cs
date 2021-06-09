namespace MonthlyPayslip.Models
{
    public class TaxRate
    {
        public int RangeBegin { get; set; }
        public int? RangeEnd { get; set; }
        public decimal Rate { get; set; }
    }
}
