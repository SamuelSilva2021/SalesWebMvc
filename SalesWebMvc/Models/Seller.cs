using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace SalesWebMvc.Models
{
    public class Seller
    {
        public int Id { get; set; }

        //Customizar o que vai aparecer no Display(Cabeçalho do formulário) lá na página da Web, usar o anotation [Display]
        [Display(Name = "Nome")]
        //Anotation para requerer o digitação do nome
        [Required(ErrorMessage = "Preencha o {0} ")]
        [StringLength(60, MinimumLength = 3, ErrorMessage ="O nome precisa ter entre {2} e {1} caracteres")]
        public string Name { get; set; }

        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Preencha o {0} ")]
        [EmailAddress(ErrorMessage = "Entre com um {0} Valido")]
        public string Email { get; set; }

        [Display (Name = "Data de Universário")]
        //Usar o anotation DataType pra formatar a data
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Preencha o {0} ")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Salário Base")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        [Required(ErrorMessage = "Preencha o {0} ")]
        public double Salary { get; set; }

        [Display(Name = "Departamento")]
        public Department Department { get; set; }

        public int DepartmentId { get; set; }
        public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();

        public Seller()
        {

        }

        public Seller(int id, string name, string email, DateTime birthDate, double salary, Department department)
        {
            Id = id;
            Name = name;
            Email = email;
            BirthDate = birthDate;
            Salary = Salary;
            Department = department;
        }
        public void AddSales(SalesRecord sr)
        {
            Sales.Add(sr);
        }
        public void RemoveSales(SalesRecord sr)
        {
            Sales.Remove(sr);
        }
        public double TotalSales(DateTime initial, DateTime final)
        {
            return Sales.Where(sr => sr.Date >= initial && sr.Date <= final).Sum(sr => sr.Amount);
        }
    }
}
