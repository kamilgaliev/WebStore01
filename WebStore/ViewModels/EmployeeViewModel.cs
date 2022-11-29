using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebStore.ViewModels
{
    public class EmployeeViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Необходимо заполнить")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Длина фамилии должна быть от 2 до 20 символов")]
        [RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)",ErrorMessage = "Неверный формат")]
        public string LastName { get; set; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Необходимо заполнить")]
        [StringLength(20,MinimumLength = 2, ErrorMessage = "Длина имени должна быть от 2 до 20 символов")]
        [RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Неверный формат")]
        public string FirstName { get; set; }

        [Display(Name = "Отчество")]
        [RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Неверный формат")]
        public string Patronymic { get; set; }

        [Display(Name = "Возраст")]
        [Required(ErrorMessage = "Необходимо заполнить")]
        [Range(18,80,ErrorMessage = "Сотрудник должен быть в возрасте от 18 до 80 лет")]
        public int Age { get; set; }
    }
}
