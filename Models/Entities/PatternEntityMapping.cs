//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FacebookChatbotManagement.Models.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class PatternEntityMapping
    {
        public int Id { get; set; }
        public int PatternId { get; set; }
        public Nullable<int> EntityId { get; set; }
        public int Position { get; set; }
        public bool Active { get; set; }
    
        public virtual Entity Entity { get; set; }
        public virtual Pattern Pattern { get; set; }
    }
}
