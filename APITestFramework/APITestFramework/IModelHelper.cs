using System;
using System.Collections.Generic;
using System.Text;

namespace APITestFramework
{
    public interface IModelHelper<T> where T : class
    {
        T Generate();
    }
}
