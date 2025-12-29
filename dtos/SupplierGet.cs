using System.ComponentModel.DataAnnotations;
using ApiRest.models;

namespace ApiRest.dtos
{
    public class SupplierGet
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Telephone { get; set; }
        public Address? Address { get; set; }
        
    }
}