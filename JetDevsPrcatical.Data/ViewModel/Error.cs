using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetDevsPrcatical.Data.ViewModel
{
    public class Error
    {
        public Error()
        {
                
        }       
        public Error(string propertyName,string message)
        {
            PropertyName = propertyName;    
            Message = message;
        }
        public string PropertyName { get; set; }
        public string Message { get; set; }       
    }
}
