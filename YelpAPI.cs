using System;
using System.Collections.Generic;
using System.Globalization;

namespace Program2
{
    class YelpAPI
    {
        public class Rootobject
        {
            public Business[] Businesses { get; set; }
            public long Total { get; set; }
            public Region Region { get; set; }
        }

        public class Business
        {
            public string Id { get; set; }

            public string Alias { get; set; }

            public string Name { get; set; }

            public Uri ImageUrl { get; set; }

            public bool IsClosed { get; set; }

            public Uri Url { get; set; }

            public long ReviewCount { get; set; }

            public Category[] Categories { get; set; }

            public double Rating { get; set; }

            public Center Coordinates { get; set; }

            public string[] Transactions { get; set; }

            public string Price { get; set; }

            public Location Location { get; set; }

            public string Phone { get; set; }

            public string DisplayPhone { get; set; }

            public double Distance { get; set; }
        }

        public class Category
        {
            public string Alias { get; set; }

            public string Title { get; set; }
        }

        public class Center
        {
            public double Latitude { get; set; }

            public double Longitude { get; set; }
        }

        public class Location
        {
            public string Address1 { get; set; }

            public string Address2 { get; set; }

            public string Address3 { get; set; }

            public string City { get; set; }

            public long ZipCode { get; set; }

            public string Country { get; set; }

            public string State { get; set; }

            public string[] DisplayAddress { get; set; }
        }

        public class Region
        {
            public Center Center { get; set; }
        }
    }
    
}
