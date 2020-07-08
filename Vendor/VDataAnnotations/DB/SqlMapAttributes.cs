using System;

namespace Vendor.VDataAnnotations
{
    public class IDENTITY_PRIMARY_KEYAttribute : Attribute 
    {
        public string VALUE
        {
            get
            {
                return "AUTO_INCREMENT PRIMARY KEY";
            }
            set { }
        }
    }
    public class AUTOINCREMENTAttribute : Attribute 
    {
        public string VALUE
        {
            get
            {
                return "AUTO_INCREMENT";
            }
            set { }
        }
    }
    public class NOT_NULLAttribute : Attribute
    {
        public string VALUE
        {
            get
            {
                return "NOT NULL";
            }
            set { }
        }
    }
    public class INDEXAttribute : Attribute
    {
        public string NAME { get; set; }
    }

    public class FIELDAttribute : Attribute
    {
        public string VALUE { get; set; }
    }
}
