//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GROCERY.Models
{
    using System;
    
    public partial class SpGetUserProfileById_Result
    {
        public int USER_ID { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public string USER_ROLE { get; set; }
        public string BRANCH { get; set; }
        public string MOBILE_NO { get; set; }
        public string ADDRESS { get; set; }
        public string CITY { get; set; }
        public string IMAGE_PATH { get; set; }
        public string EMAIL { get; set; }
        public string IPhoneId { get; set; }
        public Nullable<bool> IsSocialLogin { get; set; }
        public string FaceBookId { get; set; }
        public string CreatedDateString { get; set; }
        public string ModifiedDateString { get; set; }
        public Nullable<System.DateTime> CREATED_ON { get; set; }
        public Nullable<System.DateTime> UPDATED_ON { get; set; }
    }
}