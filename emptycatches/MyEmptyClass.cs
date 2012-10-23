// Example File this is not compiled into the build.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emptycatches
{
    class MyEmptyClass
    {
        public void Bad()
        {
            try
            {
                int a = 10;
            }
            catch
            {
            }
        }

        public void BadToo()
        {
            try
            {
                int a = 10;
            }
            catch(Exception)
            {
            }
        }

        public void AlsoBad()
        {
            try
            {
                int a = 10;
            }
            catch (Exception ex)
            {
            }
        }

        public void AlsoBadToo()
        {
            try
            {
                int a = 10;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Good()
        {
            try
            {
                int a = 10;
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        public void GoodToo()
        {
            try
            {
                int a = 10;
            }
            catch (Exception ex)
            {
                Exception a = new Exception("", ex);
                throw a;
            }
        }
    }
}
