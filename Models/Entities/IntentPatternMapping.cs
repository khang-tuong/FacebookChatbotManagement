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
    
    public partial class IntentPatternMapping
    {
        public int Id { get; set; }
        public int PatternId { get; set; }
        public int IntentId { get; set; }
        public int Group { get; set; }
        public bool Active { get; set; }
    
        public virtual Intent Intent { get; set; }
        public virtual Pattern Pattern { get; set; }
    }
}
