using System;
using System.Collections.Generic;
using System.Text;

namespace SignatureSample
{
    public interface  IStorage
    {
        string SaveImage(byte[] byteData);
    }
}
