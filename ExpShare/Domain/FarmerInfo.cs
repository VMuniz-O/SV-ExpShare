namespace ExpShare.Domain
{
    public class FarmerInfo
    {
        public long Id { get;set; }
        public string Name { get; set; }
        public int[] Experience { get; set; }
        public FarmerInfo(long _id, string _name, int[] _experience)
        {
            this.Id = _id;
            this.Name = _name;
            this.Experience = _experience;
        }
    }
}