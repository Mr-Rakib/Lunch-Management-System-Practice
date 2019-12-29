using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS
{
    interface CRUDRepository<T , I> 
    {
        List<T> findAll();
        T findById(I id);
        bool deleteById(int id);
        T save(T user);
    }
}
