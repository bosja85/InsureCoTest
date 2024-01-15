namespace InsureCo.Domain
{
    public record CoverageRule
    {
        public string Attribute { get; set; }
        public List<string> Exclude { get; set; }
        public List<string> Include { get; set; }
    }
}
