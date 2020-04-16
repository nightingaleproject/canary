using System;

namespace canary.Models
{
    public class PlaceCode
    {
        public String State { get; }
        public String County { get; }
        public String CountyCode { get; }
        public String City { get; }
        public String Description { get; }
        public String Code { get; }
        public PlaceCode() {}
        public PlaceCode(String state, String county, String statecode, String city, String description, String code)
        {
            this.State = state;
            this.County = county;
            this.CountyCode = statecode;
            this.City = city;
            this.Description = description;
            this.Code = code;
        }
    }
}
