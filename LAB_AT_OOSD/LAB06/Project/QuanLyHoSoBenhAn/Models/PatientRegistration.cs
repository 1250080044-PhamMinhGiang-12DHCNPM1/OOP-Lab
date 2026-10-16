using System;

namespace QuanLyHoSoBenhAn.Models
{
    public sealed class PatientRegistration
    {
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string PhotoPath { get; set; }
        public decimal AnnualFee { get; set; }
        public int Year { get; set; }
    }
}
