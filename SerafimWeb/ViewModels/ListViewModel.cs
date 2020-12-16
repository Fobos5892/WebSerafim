using SerafimeWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SerafimeWeb.ViewModels
{
    public class ListViewModel
    {

        public IEnumerable<User> Users { get; set; }
        public PagingInfo PagingInfo { get; set; }

        [Display(Name = "Поиск")]
        public string SearchString { get; set; }

        public SearchType SearchType { get; set; }

    }

    public enum SearchType
    {
        [Display(Name = "Логин")]
        SearchByLogin,

        [Display(Name = "ФИО")]
        SearchByFullName,

        [Display(Name = "Адрес")]
        SearchByAddress,

        [Display(Name = "E-mail")]
        SearchByEmail
    }
}

