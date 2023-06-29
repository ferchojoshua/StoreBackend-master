using Store.Entities;

namespace Store.Models.Responses
{
    public class DeptoResponse
    {
        public Department Department { get; set; }
        public int MunicipalitiesCount { get; set; }
    }

    public class MunResponse
    {
        public Municipality Municipality { get; set; }
        public int CommunitiesCount { get; set; }
    }
}
